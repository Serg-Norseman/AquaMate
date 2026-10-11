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
    public class QDecimal
    {
        public double value { get; set; }
    }

    public class QMeasureValue
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
