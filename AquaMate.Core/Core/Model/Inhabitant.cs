/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core.Types;
using SQLite;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Inhabitant : AquariumDetails, IStateItem
    {
        [Indexed]
        public int SpeciesId { get; set; }

        public string Name { get; set; }
        public Sex Sex { get; set; }

        public string Note { get; set; }

        public ItemState State { get; set; }


        /// <summary>
        /// Runtime property for rendering.
        /// </summary>
        [Ignore]
        public int Quantity { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Inhabitant;
            }
        }


        public Inhabitant()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
