/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    public class Nutrition : Entity, IStateItem, IBrandedItem
    {
        public string Name { get; set; }
        public string Brand { get; set; }

        public float Amount { get; set; }
        public string Note { get; set; }

        // not used
        public ItemState State { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Nutrition;
            }
        }


        public Nutrition()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
