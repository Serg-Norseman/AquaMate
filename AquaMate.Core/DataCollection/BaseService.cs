/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Timers;
using BSLib;

namespace AquaMate.DataCollection
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseService : BaseObject
    {
        private readonly IChannel fChannel;
        private readonly Timer fTimer;


        public event ElapsedEventHandler Elapsed;


        public IChannel Channel
        {
            get { return fChannel; }
        }

        public bool Enabled
        {
            get { return fTimer.Enabled; }
            set { fTimer.Enabled = value; }
        }

        public virtual string Name
        {
            get { return string.Empty; }
        }

        public virtual string SensorName
        {
            get { return string.Empty; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="interval">interval in milliseconds</param>
        protected BaseService(IChannel channel, double interval)
        {
            fChannel = channel;

            fTimer = new Timer(interval);
            fTimer.Elapsed += OnTimerElapsed;
            fTimer.AutoReset = true;
            fTimer.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (fTimer != null) {
                    fTimer.Enabled = false;
                    fTimer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected virtual void OnTimedEvent()
        {
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnTimedEvent();

            ElapsedEventHandler handler = Elapsed;
            if (handler != null) handler(sender, e);
        }

        protected internal virtual DataReceivedEventArgs TryReadResponse(string response)
        {
            return null;
        }
    }
}
