/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
