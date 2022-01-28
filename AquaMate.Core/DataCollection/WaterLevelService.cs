/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.DataCollection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WaterLevelService : SensorService
    {
        public override string Name
        {
            get { return "WaterLevel"; }
        }

        public override string SensorName
        {
            get { return "watlev"; }
        }


        public WaterLevelService(IChannel channel, double interval) : base(channel, interval)
        {
        }
    }
}
