/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
