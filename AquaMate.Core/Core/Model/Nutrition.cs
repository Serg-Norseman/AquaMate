/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
