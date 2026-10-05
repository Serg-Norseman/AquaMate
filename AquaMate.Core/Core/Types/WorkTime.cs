/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace AquaMate.Core.Types
{
    public enum TankState
    {
        Normal,
        Warning,
        Alert,
        Inactive
    }


    public struct WorkTime
    {
        public readonly DateTime Start;

        public readonly DateTime Stop;

        public WorkTime(DateTime start, DateTime stop)
        {
            this.Start = start;
            this.Stop = stop;
        }

        public bool IsInactive()
        {
            return !ALCore.IsZeroDate(Stop);
        }

        public bool WasStarted()
        {
            return !ALCore.IsZeroDate(Start);
        }

        public bool IsActive()
        {
            return !ALCore.IsZeroDate(Start) && ALCore.IsZeroDate(Stop);
        }

        public string GetWorkDays()
        {
            string works;
            if (IsInactive()) {
                TimeSpan span = Stop - Start;
                int days = span.Days;
                works = string.Format(Localizer.LS(LSID.AquaWorked), Start.ToString("dd/MM/yyyy"), Stop.ToString("dd/MM/yyyy"), days);
            } else {
                if (WasStarted()) {
                    TimeSpan span = DateTime.Now - Start;
                    int days = span.Days;
                    works = string.Format(Localizer.LS(LSID.AquaWorks), Start.ToString("dd/MM/yyyy"), days);
                } else {
                    works = "---";
                }
            }
            return works;
        }
    }
}
