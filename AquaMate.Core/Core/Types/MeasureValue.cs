/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
