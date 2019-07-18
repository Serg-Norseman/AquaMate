﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using SQLite;

namespace AquaLog.Core
{
    public enum Sex
    {
        None,
        Female,
        Male,
        Hermaphrodite,
    }


    /// <summary>
    /// 
    /// </summary>
    public class Fish : Inhabitant
    {
        public Sex Sex { get; set; }


        public Fish()
        {
        }
    }
}
