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
    public class MeasureValue
    {
        public readonly string Name;
        public readonly double Value;
        public readonly string Unit;

        public readonly string ValText;
        public readonly string Text;

        public readonly Color Color;

        public readonly ValueRange[] Ranges;

        public MeasureValue(string name, double value, string unit, string valText, string text, Color color, ValueRange[] ranges)
        {
            Name = name;
            Value = value;
            Unit = unit;
            ValText = valText;
            Text = text;
            Color = color;
            Ranges = ranges;
        }
    }
}
