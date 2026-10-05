/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AquaMate.Core.Model;
using AquaMate.Core.Types;

namespace AquaMate.Core
{
    /// <summary>
    /// This is a generic cache subsystem based on key/value pairs, where key is generic, too. Key must be unique.
    /// Every cache entry has its own timeout.
    /// Cache is thread safe and will delete expired entries on its own using System.Threading.Timers
    /// (which run on <see cref="ThreadPool"/> threads).
    /// </summary>
    public class Cache<K, T> : IDisposable
    {
        private bool fDisposed;
        private readonly Dictionary<K, T> fCache = new Dictionary<K, T>();
        private readonly ReaderWriterLockSlim fLocker = new ReaderWriterLockSlim();
        private readonly Dictionary<K, Timer> fTimers = new Dictionary<K, Timer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache{K,T}"/> class.
        /// </summary>
        public Cache()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!fDisposed) {
                fDisposed = true;

                if (disposing) {
                    // Dispose managed resources.
                    Clear();
                    fLocker.Dispose();
                }
                // Dispose unmanaged resources
            }
        }

        /// <summary>
        /// Clears the entire cache and disposes all active timers.
        /// </summary>
        public void Clear()
        {
            fLocker.EnterWriteLock();
            try {
                try {
                    foreach (Timer t in fTimers.Values)
                        t.Dispose();
                } catch {
                }

                fTimers.Clear();
                fCache.Clear();
            } finally {
                fLocker.ExitWriteLock();
            }
        }

        // Checks whether a specific timer already exists and adds a new one, if not
        private void CheckTimer(K key, int cacheTimeout, bool restartTimerIfExists)
        {
            int dueTime = (cacheTimeout == Timeout.Infinite ? Timeout.Infinite : cacheTimeout * 1000);

            Timer timer;
            if (fTimers.TryGetValue(key, out timer)) {
                if (restartTimerIfExists) {
                    timer.Change(dueTime, Timeout.Infinite);
                }
            } else
                fTimers.Add(key, new Timer(new TimerCallback(RemoveByTimer), key, dueTime, Timeout.Infinite));
        }

        private void RemoveByTimer(object state)
        {
            Remove((K)state);
        }

        /// <summary>
        /// Adds or updates the specified cache-key with the specified cacheObject and applies a specified timeout (in seconds) to this key.
        /// </summary>
        /// <param name="key">The cache-key to add or update.</param>
        /// <param name="cacheObject">The cache object to store.</param>
        /// <param name="cacheTimeout">The cache timeout (lifespan) of this object. Must be 1 or greater.
        /// Specify Timeout.Infinite to keep the entry forever.</param>
        /// <param name="restartTimerIfExists">(Optional). If set to <c>true</c>, the timer for this cacheObject will be reset if the object already
        /// exists in the cache. (Default = false).</param>
        public void AddOrUpdate(K key, T cacheObject, int cacheTimeout, bool restartTimerIfExists = false)
        {
            if (fDisposed)
                return;

            if (cacheTimeout != Timeout.Infinite && cacheTimeout < 1)
                throw new ArgumentOutOfRangeException("cacheTimeout", "cacheTimeout must be greater than zero.");

            fLocker.EnterWriteLock();
            try {
                CheckTimer(key, cacheTimeout, restartTimerIfExists);

                if (!fCache.ContainsKey(key))
                    fCache.Add(key, cacheObject);
                else
                    fCache[key] = cacheObject;
            } finally {
                fLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds or updates the specified cache-key with the specified cacheObject and applies <c>Timeout.Infinite</c> to this key.
        /// </summary>
        /// <param name="key">The cache-key to add or update.</param>
        /// <param name="cacheObject">The cache object to store.</param>
        public void AddOrUpdate(K key, T cacheObject)
        {
            AddOrUpdate(key, cacheObject, Timeout.Infinite);
        }

        /// <summary>
        /// Gets the cache entry with the specified key or returns <c>default(T)</c> if the key is not found.
        /// </summary>
        /// <param name="key">The cache-key to retrieve.</param>
        /// <returns>The object from the cache or <c>default(T)</c>, if not found.</returns>
        public T this[K key]
        {
            get { return Get(key); }
        }

        /// <summary>
        /// Gets the cache entry with the specified key or return <c>default(T)</c> if the key is not found.
        /// </summary>
        /// <param name="key">The cache-key to retrieve.</param>
        /// <returns>The object from the cache or <c>default(T)</c>, if not found.</returns>
        public virtual T Get(K key)
        {
            if (fDisposed)
                return default(T);

            fLocker.EnterReadLock();
            try {
                T rv;
                return (fCache.TryGetValue(key, out rv) ? rv : default(T));
            } finally {
                fLocker.ExitReadLock();
            }
        }

        /// <summary>
        /// Tries to gets the cache entry with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">(out) The value, if found, or <c>default(T)</c>, if not.</param>
        /// <returns><c>True</c>, if <c>key</c> exists, otherwise <c>false</c>.</returns>
        public bool TryGet(K key, out T value)
        {
            if (fDisposed) {
                value = default(T);
                return false;
            }

            fLocker.EnterReadLock();
            try {
                return fCache.TryGetValue(key, out value);
            } finally {
                fLocker.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes a series of cache entries in a single call for all key that match the specified key pattern.
        /// </summary>
        /// <param name="keyPattern">The key pattern to remove. The Predicate has to return true to get key removed.</param>
        public void Remove(Predicate<K> keyPattern)
        {
            if (fDisposed)
                return;

            fLocker.EnterWriteLock();
            try {
                var removers = (from k in fCache.Keys.Cast<K>() where keyPattern(k) select k).ToList();

                foreach (K workKey in removers) {
                    try {
                        fTimers[workKey].Dispose();
                    } catch {
                    }
                    fTimers.Remove(workKey);
                    fCache.Remove(workKey);
                }
            } finally {
                fLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the specified cache entry with the specified key.
        /// If the key is not found, no exception is thrown, the statement is just ignored.
        /// </summary>
        /// <param name="key">The cache-key to remove.</param>
        public void Remove(K key)
        {
            if (fDisposed)
                return;

            fLocker.EnterWriteLock();
            try {
                if (fCache.ContainsKey(key)) {
                    try {
                        fTimers[key].Dispose();
                    } catch {
                    }
                    fTimers.Remove(key);
                    fCache.Remove(key);
                }
            } finally {
                fLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Checks if a specified key exists in the cache.
        /// </summary>
        /// <param name="key">The cache-key to check.</param>
        /// <returns><c>True</c> if the key exists in the cache, otherwise <c>False</c>.</returns>
        public bool Exists(K key)
        {
            if (fDisposed)
                return false;

            fLocker.EnterReadLock();
            try {
                return fCache.ContainsKey(key);
            } finally {
                fLocker.ExitReadLock();
            }
        }
    }


    public struct EntityKey
    {
        public readonly ItemType ItemType;
        public readonly int ItemId;

        public EntityKey(ItemType itemType, int itemId)
        {
            ItemType = itemType;
            ItemId = itemId;
        }
    }


    public class EntitiesCache : Cache<EntityKey, Entity>
    {
        private readonly IModel fModel;

        public EntitiesCache(IModel model) : base()
        {
            fModel = model;
        }

        public void Remove(ItemType itemType, int itemId)
        {
            base.Remove(new EntityKey(itemType, itemId));
        }

        public T Get<T>(ItemType itemType, int itemId) where T : Entity
        {
            return (T)Get(new EntityKey(itemType, itemId));
        }

        public void Put(ItemType itemType, int itemId, Entity entity)
        {
            if (entity != null) {
                AddOrUpdate(new EntityKey(itemType, itemId), entity);
            }
        }

        public override Entity Get(EntityKey key)
        {
            var result = base.Get(key);
            if (result == null) {
                result = fModel.GetRecord(key.ItemType, key.ItemId);
                if (result != null) {
                    AddOrUpdate(key, result);
                }
            }
            return result;
        }
    }
}
