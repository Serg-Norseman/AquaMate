/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
