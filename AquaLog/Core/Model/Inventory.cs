﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaLog.Core.Types;

namespace AquaLog.Core.Model
{
    /// <summary>
    /// Inventory: chemistry, equipment, maintenance, furniture, decoration.
    /// </summary>
    public class Inventory : Entity
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Note { get; set; }

        public InventoryType Type { get; set; }


        #region Decoration properties

        public float Size { get; set; }
        public float Weight { get; set; }

        #endregion


        public Inventory()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
