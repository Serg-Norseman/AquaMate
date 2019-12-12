﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaLog.Core.Types
{
    public enum MeasurementUnit
    {
        Unknown,

        // Length
        Centimeter,
        Inch,

        // Volume
        Litre,
        UKGallon,
        USGallon,

        // Mass
        Kilogram,
        Pound,

        First = Centimeter,
        Last = Pound
    }
}
