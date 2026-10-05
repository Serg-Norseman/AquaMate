/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using System.Drawing;
using AquaMate.UI.Charts;

namespace AquaMate.UI.Components
{
    public class ChartSeries
    {
        public readonly string AxisName;
        public readonly ChartStyle Style;
        public readonly IList<ChartPoint> Data;
        public readonly Color Color;

        public ChartSeries(string axisName, ChartStyle style, IList<ChartPoint> data, Color color)
        {
            AxisName = axisName;
            Style = style;
            Data = data;
            Color = color;
        }
    }
}
