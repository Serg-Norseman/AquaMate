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
    /// <summary>
    /// 
    /// </summary>
    public class Shop : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Shop;
            }
        }


        public Shop()
        {
        }

        public Shop(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
