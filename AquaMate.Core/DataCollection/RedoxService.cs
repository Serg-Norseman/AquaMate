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
    public sealed class RedoxService : SensorService
    {
        public override string Name
        {
            get { return "Redox"; }
        }

        public override string SensorName
        {
            get { return "redox"; }
        }


        public RedoxService(IChannel channel, double interval) : base(channel, interval)
        {
        }
    }
}
