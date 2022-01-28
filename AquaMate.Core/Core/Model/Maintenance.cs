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
    public class Maintenance : AquariumDetails, IEventEntity
    {
        public DateTime Timestamp { get; set; }
        public MaintenanceType Type { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Maintenance;
            }
        }


        public Maintenance()
        {
        }
    }
}
