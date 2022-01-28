/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;

namespace AquaMate.UI.Components
{
    public sealed class ChartPoint
    {
        public string Caption { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public Color Color { get; private set; }

        public ChartPoint(string caption, double value)
        {
            Caption = caption;
            Value = value;
        }

        public ChartPoint(string caption, double value, Color color)
        {
            Caption = caption;
            Value = value;
            Color = color;
        }

        public ChartPoint(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
