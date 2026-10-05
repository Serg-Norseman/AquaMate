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
    public enum ItemState
    {
        Unknown, // all item types

        Alive, // Inhabitant
        Dead, // Inhabitant
        Sick, // Inhabitant

        InUse, // (or InWork) Device, Nutrition, Inventory
        Stopped, // Device

        Finished, // Nutrition, Additive, Chemistry
        Broken, // Device, Equipment, Maintenance, Furniture, Decoration,

        Sold, // all item types
    }
}
