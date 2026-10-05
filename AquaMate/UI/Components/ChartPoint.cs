/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
