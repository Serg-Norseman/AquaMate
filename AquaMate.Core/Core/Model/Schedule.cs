/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
