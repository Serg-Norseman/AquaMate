/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core.Types
{
    public enum ItemType
    {
        /* 00 */ None,

        /* 01 */ Aquarium,

        // Inhabitants
        /* 02 */ Fish,
        /* 03 */ Invertebrate,
        /* 04 */ Plant,
        /* 05 */ Coral,

        // Lifesupport
        /* 06 */ Nutrition,
        /* 07 */ Device,

        // Inventory, Lifesupport
        /* 08 */ Additive,
        /* 09 */ Chemistry,

        // Inventory
        /* 10 */ Equipment,
        /* 11 */ Maintenance,
        /* 12 */ Furniture,
        /* 13 */ Decoration,
        /* 14 */ Soil,
    }
}
