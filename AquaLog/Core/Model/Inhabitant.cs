﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using SQLite;

namespace AquaLog.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Inhabitant : AquariumDetails
    {
        [Indexed]
        public int SpeciesId { get; set; }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        [Ignore]
        public DateTime CheckIn { get; set; }
        [Ignore]
        public DateTime CheckOut { get; set; }


        public Inhabitant()
        {
        }
    }
}