/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Timers;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.Core.Model.Tanks;
using AquaMate.Core.Types;
using AquaMate.M3DViewer;
using AquaMate.M3DViewer.Tanks;
using AquaMate.UI.Components;
using OpenTK.GLControl;
using OpenTK.Windowing.Common;

namespace AquaMate.UI.Panels
{
    public sealed class M3DViewerPanel : Form
    {
        private readonly GLControl fViewer;

        private System.Timers.Timer fAnimTimer;
        private Aquarium fAquarium;
        private bool fBusy;
        private bool fFreeRotate;
        private int fLastX;
        private int fLastY;
        private bool fMouseDrag;
        private Vector3D fRotation;
        private OGLRenderer fSceneRenderer;
        private ITankRenderer fTankRenderer;
        private float fZ;

        private DeviceModel fAquaLight;


        public M3DViewerPanel(object extData)
        {
            fAquarium = (Aquarium)extData;

            Width = 800;
            Height = 600;

            var infoPanel = new ToolStripStatusLabel();
            infoPanel.Spring = true;
            infoPanel.Text = "Free-rotate (R); Water visible (W); Aeration (A); Lighter (L)";

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
            fViewer.HandleCreated += OnHandleCreated;
            fViewer.Paint += OnPaint;
            fViewer.SizeChanged += OnSizeChanged;
            fViewer.KeyDown += OnKeyDown;
            fViewer.MouseDown += OnMouseDown;
            fViewer.MouseUp += OnMouseUp;
            fViewer.MouseMove += OnMouseMove;
            fViewer.MouseWheel += OnMouseWheel;

            Controls.AddRange(new Control[] { fViewer, statusBar });

            this.Load += (s, e) => StartTimer();
            this.Closed += (s, e) => StopTimer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                StopTimer();
            }
            base.Dispose(disposing);
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            fSceneRenderer = new OGLRenderer();
            fSceneRenderer.InitScene();
            Reset();

            var sz = fViewer.Size;
            fSceneRenderer?.SetViewport(sz.Width, sz.Height, 45.0f, 0.0f, 100.0f);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            var sz = fViewer.Size;
            fSceneRenderer?.SetViewport(sz.Width, sz.Height, 45.0f, 0.0f, 100.0f);
        }

        private void Reset()
        {
            fRotation.X = +25.0f;
            fRotation.Y = +25.0f;
            fRotation.Z = 0.0f;
            fZ = -2.0f;

            fFreeRotate = true;
            fAquaLight = null;
            fTankRenderer = null;
            if (fAquarium == null) return;

            ITank tank = fAquarium.Tank;
            switch (tank.GetTankShape()) {
                case TankShape.Unknown:
                    break;

                case TankShape.Bowl:
                    fTankRenderer = new BowlTankRenderer(fSceneRenderer, (BowlTank)tank);
                    break;

                case TankShape.Cube:
                    fTankRenderer = new CubeTankRenderer(fSceneRenderer, (CubeTank)tank);
                    break;

                case TankShape.Rectangular:
                    fTankRenderer = new RectangularTankRenderer(fSceneRenderer, (RectangularTank)tank);
                    break;

                case TankShape.BowFront:
                    fTankRenderer = new BowfrontTankRenderer(fSceneRenderer, (BowFrontTank)tank);
                    break;

                case TankShape.PlateFrontCorner:
                case TankShape.BowFrontCorner:
                    break;

                case TankShape.Cylinder:
                    fTankRenderer = new CylinderTankRenderer(fSceneRenderer, (CylinderTank)tank);
                    break;
            }
        }

        private void UpdateTV(object sender, ElapsedEventArgs e)
        {
            if (!fBusy) {
                fBusy = true;

                if (!fFreeRotate) {
                    fRotation.Y -= 0.3f;
                }

                fViewer.Invalidate();

                fBusy = false;
            }
        }

        private void StartTimer()
        {
            if (fAnimTimer != null) return;

            fAnimTimer = new System.Timers.Timer();
            fAnimTimer.AutoReset = true;
            fAnimTimer.Interval = 20;
            fAnimTimer.Elapsed += UpdateTV;
            fAnimTimer.Start();
        }

        private void StopTimer()
        {
            if (fAnimTimer == null) return;

            fAnimTimer.Stop();
            fAnimTimer = null;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            fViewer.MakeCurrent();
            fSceneRenderer.BeginDrawing();

            fSceneRenderer.SetLight(0, new float[] { 0.5f, 0.5f, 0.5f, 1.0f }, null, M3DMaterials.LightSpecular, null);
            if (fAquaLight == null) {
                fSceneRenderer.SetLight(1, M3DMaterials.LightAmbient, M3DMaterials.LightDiffuse, M3DMaterials.LightSpecular, M3DMaterials.LightPosition);
            }

            fSceneRenderer.Translatef(0.0f, 0.0f, fZ);
            fSceneRenderer.Rotatef(fRotation.X, 1.0f, 0.0f, 0.0f);
            fSceneRenderer.Rotatef(fRotation.Y, 0.0f, 1.0f, 0.0f);
            fSceneRenderer.Rotatef(fRotation.Z, 0.0f, 0.0f, 1.0f);

            if (fTankRenderer != null) {
                fTankRenderer.Render();
            }

            if (fAquaLight != null) {
                fSceneRenderer.ObjDraw(fAquaLight);
            }

            fSceneRenderer.EndDrawing();
            fViewer.SwapBuffers();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.PageDown:
                    fZ -= 0.5f;
                    break;

                case Keys.PageUp:
                    fZ += 0.5f;
                    break;

                case Keys.R:
                    fFreeRotate = !fFreeRotate;
                    break;

                case Keys.A:
                    fTankRenderer.Aeration = !fTankRenderer.Aeration;
                    break;

                case Keys.W:
                    fTankRenderer.ShowWater = !fTankRenderer.ShowWater;
                    break;

                case Keys.L:
                    fTankRenderer.ShowLighter = !fTankRenderer.ShowLighter;
                    break;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!fViewer.Focused) fViewer.Focus();

            if (e.Button == MouseButtons.Left) {
                fMouseDrag = true;
                fLastX = e.X;
                fLastY = e.Y;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                fMouseDrag = false;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (fMouseDrag) {
                int dx = e.X - fLastX;
                int dy = e.Y - fLastY;

                if (fFreeRotate) {
                    fRotation.X += 0.005f * dy;
                    fRotation.Y += 0.005f * dx;
                } else {
                    fRotation.Y += 0.005f * dx;
                }
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0) {
                fZ += 0.001f * e.Delta;
            }
        }
    }
}
