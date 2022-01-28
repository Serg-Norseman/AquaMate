/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core;
using AquaMate.Core.Model.Tanks;
using BSLib;

namespace AquaMate.M3DViewer.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BowfrontTankRenderer : TankRenderer<BowFrontTank>
    {
        public BowfrontTankRenderer(SceneRenderer sceneRenderer, BowFrontTank tank) : base(sceneRenderer, tank)
        {
        }

        public override void Render(bool showWater = true, bool aeration = false, bool showInfo = false)
        {
            float length = fTank.Length;
            float width = fTank.Width;
            float fullWidth = fTank.CentreWidth;
            float height = fTank.Height;
            float thickness = fTank.GlassThickness;

            length *= ScaleFactor;
            width *= ScaleFactor;
            fullWidth *= ScaleFactor;
            height *= ScaleFactor;
            thickness *= ScaleFactor;

            fScene.Translatef(0.0f, -height / 2, -width / 2);

            SetGlassMaterial();

            var ld2 = length / 2.0f;
            var x1s = 0 - ld2;
            var x2s = 0 + ld2;

            var x1 = x1s;
            var x2 = x2s;

            // bottom
            var y1 = 0.0f - thickness;
            var y2 = 0.0f;
            var z1 = 0.0f;
            var z2 = z1 + width;
            DrawBowBox(x1, x2, y1, y2, z1, z2, fullWidth - width);

            // back
            y1 = 0.0f + height;
            y2 = 0.0f;
            z1 = 0.0f;
            z2 = z1 + thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // sides
            y1 = 0.0f + height;
            y2 = 0.0f;
            z1 = 0.0f + thickness;
            z2 = z1 + (width - thickness);

            // left side
            x1 = x1s;
            x2 = x1s + thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // right side
            x1 = x2s;
            x2 = x2s - thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // front
            DrawBowfrontPlate(x1s, x2s, 0.0f, width, fullWidth, height, thickness);

            if (showWater) {
                SetWaterMaterial();
                float watHeight = height - thickness - (StdWaterOffset * ScaleFactor);

                var x1w = x1s + thickness;
                var x2w = x2s - thickness;
                var y1w = watHeight;
                var y2w = 0.0f;
                var z1w = 0.0f + thickness;
                var z2w = 0.0f + width;
                DrawBowBox(x1w, x2w, y1w, y2w, z1w, z2w, fullWidth - width - thickness);

                if (aeration) {
                    var aeraPt = new Point3D(0.0f, 0.0f, width / 2.0f);
                    var surfacedBubbles = new List<M3DBubble>();
                    fAeration.DrawBubbles(fScene, aeraPt, watHeight, surfacedBubbles);
                }
            }
        }

        private void DrawBowfrontPlate(float x1, float x2, float z1, float width, float fullWidth, float height, float thickness)
        {
            IList<Point3D> points1, points2;
            float centerZ1, centerZ2;

            // outer plate
            DrawBowfront(x1, x2, z1, width, fullWidth, height, out centerZ1, out points1);
            // inner plate
            DrawBowfront(x1 + thickness, x2 - thickness, z1, width - thickness, fullWidth - thickness, height, out centerZ2, out points2);

            float y1 = 0.0f;
            float y2 = y1 + height;
            DrawBowfrontFace(points1, points2, y1, centerZ1, centerZ2);
            DrawBowfrontFace(points1, points2, y2, centerZ1, centerZ2);
        }

        private void DrawBowfrontFace(IList<Point3D> points1, IList<Point3D> points2, float y, float centerZ1, float centerZ2)
        {
            fScene.PushMatrix();
            fScene.BeginTriangleStrip();
            for (int j = 0; j < points1.Count; ++j) {
                var pt1 = points1[j];
                var pt2 = points2[j];
                //OpenGL.glNormal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt1.X, y, centerZ1 + pt1.Z);
                //OpenGL.glNormal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt2.X, y, centerZ2 + pt2.Z);
            }
            fScene.End();
            fScene.PopMatrix();
        }

        // Draw an arc strip of a given height from y=0
        private void DrawBowfront(float x1, float x2, float z1, float width, float fullWidth, float height, out float centerZ, out IList<Point3D> points)
        {
            float chordLength = x2 - x1;
            float chordWidth = fullWidth - width;

            float radius, wedgeAngle;
            ALData.CalcSegmentParams(chordWidth, chordLength, out radius, out wedgeAngle);
            wedgeAngle = (float)MathHelper.RadiansToDegrees(wedgeAngle);

            centerZ = (z1 + fullWidth - radius);
            Point3D p0 = new Point3D(0.0f, 0.0f, centerZ);
            Point3D p1 = new Point3D(x2, 0.0f, centerZ);
            Point3D p2 = new Point3D(x2, 0.0f, z1 + width);
            float startAngle = Vector3D.GetAngle(p1.Sub(p0), p2.Sub(p0));

            fScene.PushMatrix();
            fScene.Translatef(0.0f, 0.0f, centerZ);
            points = GetArcPoints(30, radius, startAngle, wedgeAngle);
            DrawCylinder(points, height, radius);
            fScene.PopMatrix();
        }

        private void DrawBowBox(float x1, float x2, float y1, float y2, float z1, float z2, float bowWidth)
        {
            var p1 = new Point3D(x1, y1, z1);
            var p2 = new Point3D(x2, y1, z1);
            var p3 = new Point3D(x2, y1, z2);
            var p4 = new Point3D(x1, y1, z2);
            var p5 = new Point3D(x1, y2, z1);
            var p6 = new Point3D(x2, y2, z1);
            var p7 = new Point3D(x2, y2, z2);
            var p8 = new Point3D(x1, y2, z2);

            DrawRect(p1, p2, p6, p5, new Point3D(0.0f, 0.0f, -1.0f)); // back
            DrawRect(p2, p3, p7, p6, new Point3D(+1.0f, 0.0f, 0.0f)); // right
            DrawRect(p1, p4, p8, p5, new Point3D(-1.0f, 0.0f, 0.0f)); // left

            float height = y2 - y1;
            float width = z2 - z1;

            IList<Point3D> points;
            float centerZ;

            fScene.PushMatrix();
            fScene.Translatef(0.0f, -height, 0.0f);
            DrawBowfront(x1, x2, z1, width, width + bowWidth, height, out centerZ, out points);
            fScene.PopMatrix();

            // top
            fScene.BeginPolygon();
            fScene.Vertex3f(p4.X, y1, p4.Z);
            fScene.Vertex3f(p1.X, y1, p1.Z);
            fScene.Vertex3f(p2.X, y1, p2.Z);
            fScene.Vertex3f(p3.X, y1, p3.Z);
            for (int j = 0; j < points.Count; j++) {
                var pt = points[j];
                fScene.Vertex3f(pt.X, y1, pt.Z + centerZ);
            }
            fScene.End();

            // bottom
            fScene.BeginPolygon();
            fScene.Vertex3f(p8.X, y2, p8.Z);
            fScene.Vertex3f(p5.X, y2, p5.Z);
            fScene.Vertex3f(p6.X, y2, p6.Z);
            fScene.Vertex3f(p7.X, y2, p7.Z);
            for (int j = 0; j < points.Count; j++) {
                var pt = points[j];
                fScene.Vertex3f(pt.X, y2, pt.Z + centerZ);
            }
            fScene.End();
        }
    }
}
