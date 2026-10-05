/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Windows.Forms;
using AquaMate.UI.Components;

namespace AquaMate.UI.Panels
{
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
