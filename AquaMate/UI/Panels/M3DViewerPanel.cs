/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

#if !NET8_0_OR_GREATER

using System;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.UI.Components;

namespace AquaMate.UI.Panels
{
    public sealed class M3DViewerPanel : DataPanel
    {
        private readonly OGLViewer fViewer;

        public M3DViewerPanel()
        {
            var infoPanel = new ToolStripStatusLabel();
            infoPanel.Spring = true;
            infoPanel.Text = "Free-rotate (R); Water visible (W); Aeration (A)";

            var statusBar = new StatusStrip();
            statusBar.Items.AddRange(new ToolStripStatusLabel[] { infoPanel });
            //statusBar.ShowPanels = true;

            fViewer = new OGLViewer();
            fViewer.Dock = DockStyle.Fill;
            fViewer.VisibleChanged += Panel_VisibleChanged;

            Controls.AddRange(new Control[] { fViewer, statusBar });
        }

        public override void SetExtData(object extData)
        {
            fViewer.Aquarium = (Aquarium)extData;
        }

        private void Panel_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible) {
                fViewer.StartTimer();
            } else {
                fViewer.StopTimer();
            }
        }
    }
}

#endif
