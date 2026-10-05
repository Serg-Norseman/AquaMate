/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Model;
using SQLite;

namespace AquaMate.TSDB
{
    /// <summary>
    /// The value of a point in a time series (tag).
    /// </summary>
    public class TSValue : IEntity
    {
        [Unique]
        public DateTime Timestamp { get; set; }

        public double Value { get; set; }


        public TSValue()
        {
        }

        public TSValue(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }
    }
}
