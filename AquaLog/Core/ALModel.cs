﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using AquaLog.Core.Model;
using AquaLog.Core.Types;
using SQLite;

namespace AquaLog.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ALModel
    {
        private readonly SQLiteConnection fDB;

        public ALModel()
        {
            var databasePath = Path.Combine(ALCore.GetAppDataPath(), "ALData.db");
            fDB = new SQLiteConnection(databasePath);

            fDB.CreateTable<Aquarium>();

            fDB.CreateTable<Fish>();
            fDB.CreateTable<Invertebrate>();
            fDB.CreateTable<Plant>();
            fDB.CreateTable<Species>();

            fDB.CreateTable<Light>();
            fDB.CreateTable<Pump>();

            fDB.CreateTable<Expense>();
            fDB.CreateTable<Note>();
            fDB.CreateTable<WaterChange>();
            fDB.CreateTable<History>();
            fDB.CreateTable<Maintenance>();

            fDB.CreateTable<Transfer>();
        }

        /// <summary>
        /// Cleaning waste space
        /// </summary>
        public void CleanSpace()
        {
            fDB.Execute("VACUUM;");
        }

        public void Execute(string query, params object[] args)
        {
            fDB.Execute(query, args);
        }

        public int AddRecord(Entity obj)
        {
            return fDB.Insert(obj);
        }

        public void UpdateRecord(Entity obj)
        {
            fDB.Update(obj);
        }

        public void DeleteRecord(Entity obj)
        {
            fDB.Delete(obj);
        }

        public void DeleteRecord<T>(int objId)
        {
            fDB.Delete<T>(objId);
        }

        public T GetRecord<T>(int objId) where T : new()
        {
            return (objId == 0) ? default(T) : fDB.Get<T>(objId);
        }

        public IEnumerable<T> QueryRecords<T>(string query, params object[] args) where T : new()
        {
            return fDB.Query<T>(query, args); // "select * from Aquarium"
        }

        public Entity GetRecord(ItemType itemType, int itemId)
        {
            Entity result = null;
            switch (itemType) {
                case ItemType.Aquarium:
                    break;
                case ItemType.Fish:
                    result = GetRecord<Fish>(itemId);
                    break;
                case ItemType.Invertebrate:
                    result = GetRecord<Invertebrate>(itemId);
                    break;
                case ItemType.Light:
                    break;
                case ItemType.Plant:
                    result = GetRecord<Plant>(itemId);
                    break;
                case ItemType.Pump:
                    break;
            }
            return result;
        }

        public string GetRecordName(ItemType itemType, int itemId)
        {
            Entity itemRec = GetRecord(itemType, itemId);
            string itName = string.Empty;
            switch (itemType) {
                case ItemType.Aquarium:
                    break;
                case ItemType.Fish:
                    itName = (itemRec as Fish).Name;
                    break;
                case ItemType.Invertebrate:
                    itName = (itemRec as Invertebrate).Name;
                    break;
                case ItemType.Light:
                    break;
                case ItemType.Plant:
                    itName = (itemRec as Plant).Name;
                    break;
                case ItemType.Pump:
                    break;
            }
            return itName;
        }

        #region Aquarium functions

        public IEnumerable<Aquarium> QueryAquariums()
        {
            return fDB.Query<Aquarium>("select * from Aquarium");
        }

        #endregion

        #region Fish functions

        public IEnumerable<Fish> QueryFishes()
        {
            return fDB.Query<Fish>("select * from Fish");
        }

        public IEnumerable<Fish> QueryFishes(Aquarium aquarium)
        {
            return fDB.Query<Fish>("select * from Fish where AquariumId = ?", aquarium.Id);
        }

        #endregion

        #region Invertebrate functions

        public IEnumerable<Invertebrate> QueryInvertebrates()
        {
            return fDB.Query<Invertebrate>("select * from Invertebrate");
        }

        public IEnumerable<Invertebrate> QueryInvertebrates(Aquarium aquarium)
        {
            return fDB.Query<Invertebrate>("select * from Invertebrate where AquariumId = ?", aquarium.Id);
        }

        #endregion

        #region Plant functions

        public IEnumerable<Plant> QueryPlants()
        {
            return fDB.Query<Plant>("select * from Plant");
        }

        public IEnumerable<Plant> QueryPlants(Aquarium aquarium)
        {
            return fDB.Query<Plant>("select * from Plant where AquariumId = ?", aquarium.Id);
        }

        #endregion

        public IList<Species> QuerySpecies()
        {
            return fDB.Query<Species>("select * from Species");
        }

        public IList<Species> QuerySpecies(int type)
        {
            return fDB.Query<Species>("select * from Species where [Type] = ?", type);
        }

        // FIXME: debug table name
        public IList<Device> QueryDevices()
        {
            return fDB.Query<Device>("select * from Pump");
        }

        public IList<WaterChange> QueryWaterChanges()
        {
            return fDB.Query<WaterChange>("select * from WaterChange order by [ChangeDate]");
        }

        public IList<WaterChange> QueryWaterChanges(int aquariumId)
        {
            return fDB.Query<WaterChange>("select * from WaterChange where (AquariumId = ?) order by [ChangeDate]", aquariumId);
        }

        public IList<Transfer> QueryTransfers()
        {
            return fDB.Query<Transfer>("select * from Transfer order by [Date]");
        }

        public IList<Transfer> QueryTransfers(int aquariumId)
        {
            return fDB.Query<Transfer>("select * from Transfer where (SourceId = ? or TargetId = ?) order by [Date]", aquariumId, aquariumId);
        }

        public IList<Transfer> QueryTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Date]", itemId, itemType);
        }

        public IList<Transfer> QueryLastTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Date] desc", itemId, itemType);
        }

        #region Note functions

        public IEnumerable<Note> QueryNotes(Aquarium aquarium)
        {
            return fDB.Query<Note>("select * from Note where AquariumId = ?", aquarium.Id);
        }

        #endregion

        #region Expense functions

        public IEnumerable<Expense> QueryExpenses()
        {
            return fDB.Query<Expense>("select * from Expense");
        }

        public float GetTotalExpense()
        {
            // "select total(Price) as total_expense from Expenses"
            // "select distinct Type as element from Expenses"
            // "select distinct Shop as element from Expenses"
            return 0.0f;
        }

        #endregion
    }
}
