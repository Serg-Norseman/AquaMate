/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    public enum MaintenanceType
    {
        /* 0 */ Restart,
        /* 1 */ WaterAdded,
        /* 2 */ WaterReplaced,
        /* 3 */ WaterRemoved,
        /* 4 */ Clean,
        /* 5 */ Other,
        /* 6 */ Fertilize,
        /* 7 */ Cure,
        /* 8 */ AquariumStarted,
        /* 9 */ AquariumStopped,
    }
}
