﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaLog.Core.Types
{
    public enum AquariumWaterType
    {
        FreshWater,
        BrackishWater,
        SeaWater,
    }

    // FIXME
    public enum AquariumTemperatureType
    {
        Coldwater,
        Tropical,
        ReefMarine, // only SeaWater
    }
}
