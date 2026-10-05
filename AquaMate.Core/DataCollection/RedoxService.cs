/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
