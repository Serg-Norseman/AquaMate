/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
