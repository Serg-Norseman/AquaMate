/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;

namespace AquaMate.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpeciesProps : IProps
    {
        public LSID Name { get; private set; }
        public Color Color { get; private set; }

        public SpeciesProps(LSID name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
