/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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


    public sealed class InhabitantDispItem : Inhabitant
    {
        public string AquariumName { get; set; }
        public string SpeciesName { get; set; }
        public string SexName { get; set; }
        public string StateStr { get; set; }
        public ItemType ItemType { get; set; }
        public bool Fin { get; set; }
        public string InclusionDate { get; set; }
        public string ExclusionDate { get; set; }
        public string LifeSpan { get; set; }
        public string Temp { get; set; }
        public string PH { get; set; }
        public string GH { get; set; }
        public int iDays { get; set; }


        public InhabitantDispItem()
        {
        }
    }
}
