/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Brand : Entity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Brand;
            }
        }


        public Brand()
        {
        }

        public Brand(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
