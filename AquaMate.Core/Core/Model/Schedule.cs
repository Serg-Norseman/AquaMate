/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Schedule : AquariumDetails
    {
        public DateTime Timestamp { get; set; }
        public string Event { get; set; }
        public string Note { get; set; }
        public bool Reminder { get; set; }
        public ScheduleType Type { get; set; }
        public TaskStatus Status { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Schedule;
            }
        }


        public Schedule()
        {
        }

        public override string ToString()
        {
            return Event;
        }
    }
}
