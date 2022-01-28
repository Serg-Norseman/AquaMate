/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.Core.Types
{
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
