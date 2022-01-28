/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
