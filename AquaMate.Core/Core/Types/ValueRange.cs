/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Drawing;

namespace AquaMate.Core.Types
{
    public sealed class ValueRange
    {
        public double Min { get; private set; }
        public double Max { get; private set; }
        public Color Color { get; private set; }
        public string Name { get; private set; }

        public ValueRange(double min, double max, Color color, string name)
        {
            Min = min;
            Max = max;
            Color = color;
            Name = name;
        }
    }
}
