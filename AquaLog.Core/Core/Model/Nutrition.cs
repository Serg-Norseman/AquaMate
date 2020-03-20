﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaLog.Core.Types;

namespace AquaLog.Core.Model
{
    public class Nutrition : Entity, IStateItem, IBrandedItem
    {
        public string Name { get; set; }
        public string Brand { get; set; }

        public float Amount { get; set; }
        public string Note { get; set; }

        // not used
        public ItemState State { get; set; }


        public Nutrition()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
