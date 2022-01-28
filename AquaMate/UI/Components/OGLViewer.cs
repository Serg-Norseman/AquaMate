/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2021 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Timers;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.Core.Model.Tanks;
using AquaMate.Core.Types;
using AquaMate.M3DViewer;
using AquaMate.M3DViewer.Tanks;
using CsGL.OpenGL;

namespace AquaMate.UI.Components
{
    public sealed class OGLViewer : OpenGLControl
    {
        private bool fAeration;
        private System.Timers.Timer fAnimTimer;
        private Aquarium fAquarium;
        private bool fBusy;
        private bool fFreeRotate;
        private int fLastX;
        private int fLastY;
        private bool fMouseDrag;
        private Vector3D fRotation;
        private readonly OGLRenderer fSceneRenderer;
        private ITankRenderer fTankRenderer;
        private bool fWaterVisible;
        private float fZ;

        private DeviceModel fAquaLight;


        public Aquarium Aquarium
        {
            get {
                return fAquarium;
            }
            set {
                fAquarium = value;
                Reset();
            }
        }


        public OGLViewer()
        {
            fSceneRenderer = new OGLRenderer(this);

            Reset();

            fSceneRenderer.InitScene();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                StopTimer();
            }
            base.Dispose(disposing);
        }

        public void Reset()
        {
            fRotation.X = +25.0f;
            fRotation.Y = +25.0f;
            fRotation.Z = 0.0f;
            fZ = -2.0f;

            fFreeRotate = true;
            fWaterVisible = false;
            fAeration = false;

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

                    // debug, only for `Eheim Aquastar 54 LED`
                    //fAquaLight = fSceneRenderer.ObjLoad(@".\common\eheim_classic_led_55.m3d");
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

                Invalidate(false);

                fBusy = false;
            }
        }

        public void StartTimer()
        {
            if (fAnimTimer != null) return;

            fAnimTimer = new System.Timers.Timer();
            fAnimTimer.AutoReset = true;
            fAnimTimer.Interval = 20;
            fAnimTimer.Elapsed += UpdateTV;
            fAnimTimer.Start();
        }

        public void StopTimer()
        {
            if (fAnimTimer == null) return;

            fAnimTimer.Stop();
            fAnimTimer = null;
        }
 
        public override void glDraw()
        {
            fSceneRenderer.BeginDrawing();

            fSceneRenderer.SetLight(0, new float[] { 0.5f, 0.5f, 0.5f, 1.0f }, null, SceneRenderer.LightSpecular, null);
            if (fAquaLight == null) {
                fSceneRenderer.SetLight(1, SceneRenderer.LightAmbient, SceneRenderer.LightDiffuse, SceneRenderer.LightSpecular, SceneRenderer.LightPosition);
            }

            fSceneRenderer.Translatef(0.0f, 0.0f, fZ);
            fSceneRenderer.Rotatef(fRotation.X, 1.0f, 0.0f, 0.0f);
            fSceneRenderer.Rotatef(fRotation.Y, 0.0f, 1.0f, 0.0f);
            fSceneRenderer.Rotatef(fRotation.Z, 0.0f, 0.0f, 1.0f);

            if (fTankRenderer != null) {
                fTankRenderer.Render(fWaterVisible, fAeration, true);
            }

            if (fAquaLight != null) {
                fSceneRenderer.ObjDraw(fAquaLight);
            }

            fSceneRenderer.EndDrawing();
        }

        protected override OpenGLContext CreateContext()
        {
            ControlGLContext context = new ControlGLContext(this);
            DisplayType displayType = new DisplayType(32, 32, 32, 32);
            context.Create(displayType, null);
            return context;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            GrabContext();
            var sz = Size;
            fSceneRenderer.SetViewport(sz.Width, sz.Height, 45.0f, 0.0f, 100.0f);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

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
                    fAeration = !fAeration;
                    break;

                case Keys.W:
                    fWaterVisible = !fWaterVisible;
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Focused) Focus();

            if (e.Button == MouseButtons.Left) {
                fMouseDrag = true;
                fLastX = e.X;
                fLastY = e.Y;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left) {
                fMouseDrag = false;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0) {
                fZ += 0.001f * e.Delta;
            }
        }
    }
}
