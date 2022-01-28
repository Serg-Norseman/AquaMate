/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Windows.Forms;
using AquaMate.UI.Components;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChartPanel : DataPanel
    {
        private readonly ZChart fChart;
        private object fData;

        public ChartPanel()
        {
            fChart = new ZChart();
            fChart.Dock = DockStyle.Fill;
            Controls.Add(fChart);
        }

        public override void SetExtData(object extData)
        {
            fData = extData;
        }

        public override void UpdateContent()
        {
            fChart.ShowData("", "Labels", "", fData);
        }
    }
}
