/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.UI.Components;
using OpenTK.GLControl;
using OpenTK.Windowing.Common;

namespace AquaMate.UI.Panels
{
    public sealed class M3DViewerPanel : Form
    {
        private readonly GLControl fViewer;
        private readonly OGLViewer fViewerController;

        public M3DViewerPanel(object extData)
        {
            Width = 800;
            Height = 600;

            var infoPanel = new ToolStripStatusLabel();
            infoPanel.Spring = true;
            infoPanel.Text = "Free-rotate (R); Water visible (W); Aeration (A)";

            var statusBar = new StatusStrip();
            statusBar.Items.AddRange(new ToolStripStatusLabel[] { infoPanel });

            fViewer = new GLControl(new GLControlSettings {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 3, 0, 0),
                Profile = ContextProfile.Compatability,
                Flags = ContextFlags.Default,
                AutoLoadBindings = true,
                RedBits = 8,
                GreenBits = 8,
                BlueBits = 8,
                AlphaBits = 8,
                DepthBits = 24,
                StencilBits = 8,
                NumberOfSamples = 0
            });
            fViewer.Dock = DockStyle.Fill;

            fViewerController = new OGLViewer(fViewer);
            fViewerController.Aquarium = (Aquarium)extData;

            Controls.AddRange(new Control[] { fViewer, statusBar });

            this.Load += (s, e) => fViewerController.StartTimer();
            this.Closed += (s, e) => fViewerController.StopTimer();
        }
    }
}
