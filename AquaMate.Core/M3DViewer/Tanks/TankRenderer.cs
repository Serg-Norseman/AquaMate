/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core.Model;
using BSLib;

namespace AquaMate.M3DViewer.Tanks
{
    [Flags]
    public enum BoxSides
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 4,
        Back = 8,
        Front = 16,
    }


    public interface ITankRenderer
    {
        bool ShowWater { get; set; }
        bool Aeration { get; set; }
        bool ShowInfo { get; set; }
        bool ShowLighter { get; set; }

        void Render();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class TankRenderer<T> : ITankRenderer where T : ITank
    {
        public const BoxSides AllSides = BoxSides.Top | BoxSides.Bottom | BoxSides.Left | BoxSides.Right | BoxSides.Back | BoxSides.Front;
        public const BoxSides AllSidesWF = BoxSides.Top | BoxSides.Bottom | BoxSides.Left | BoxSides.Right | BoxSides.Back;
        public const BoxSides AllSidesWT = BoxSides.Bottom | BoxSides.Left | BoxSides.Right | BoxSides.Back | BoxSides.Front;

        public const float ScaleFactor = 0.01f;

        protected readonly M3DAeration fAeration;
        protected readonly SceneRenderer fScene;
        protected readonly T fTank;
        protected readonly M3DWaterSurface fWater;


        public bool ShowWater { get; set; }
        public bool Aeration { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowLighter { get; set; }


        public T Tank
        {
            get { return fTank; }
        }


        protected TankRenderer(SceneRenderer sceneRenderer, T tank)
        {
            fScene = sceneRenderer;
            fTank = tank;
            fAeration = new M3DAeration();
            fWater = new M3DWaterSurface();

            ShowWater = false;
            ShowLighter = false;
            Aeration = false;
            ShowInfo = false;
        }

        public abstract void Render();

        public void SetGlassMaterial()
        {
            fScene.SetMaterial(M3DMaterials.GlassDiffuse, M3DMaterials.GlassSpecular, M3DMaterials.GlassShininess);
        }

        public void SetWaterMaterial()
        {
            fScene.SetMaterial(M3DMaterials.Water2Diffuse, M3DMaterials.Water2Specular, M3DMaterials.Water2Shininess);
        }

        protected void DrawRectangularTank(float length, float width, float height, float thickness, float underfillHeight)
        {
            float fltX, fltY, fltZ;

            length *= ScaleFactor;
            width *= ScaleFactor;
            height *= ScaleFactor;
            thickness *= ScaleFactor;

            fScene.Translatef(0.0f, -height / 2, -width / 2);

            fScene.BeginTransparentRendering();

            SetGlassMaterial();

            var ld2 = length / 2.0f;
            var x1s = 0 - ld2;
            var x2s = 0 + ld2;

            var x1 = x1s;
            var x2 = x2s;

            // bottom
            var y1 = 0.0f;
            var y2 = y1 - thickness;
            var z1 = 0.0f;
            var z2 = z1 + width;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // front
            y1 = 0.0f + height;
            y2 = 0.0f;
            z1 = 0.0f + width;
            z2 = z1 - thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            fltX = x1;
            fltY = y1 + thickness;
            fltZ = z1;

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
            z2 = z1 + (width - thickness * 2);

            // left side
            x1 = x1s;
            x2 = x1s + thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // right side
            x1 = x2s;
            x2 = x2s - thickness;
            DrawBox(x1, x2, y1, y2, z1, z2);

            // water cube
            var surfacedBubbles = new List<M3DBubble>();
            float watHeight = height - thickness - (underfillHeight * ScaleFactor);
            var x1w = x1s + thickness;
            var x2w = x2s - thickness;
            var y1w = 0;
            var y2w = 0 + watHeight;
            var z1w = 0 + thickness;
            var z2w = 0 + width - thickness;

            if (!fWater.IsInitiated) {
                // top surface
                Point3D pt1t = new Point3D(x1w, y2w, z1w);
                Point3D pt2t = new Point3D(x2w, y2w, z1w);
                Point3D pt3t = new Point3D(x2w, y2w, z2w);
                Point3D pt4t = new Point3D(x1w, y2w, z2w);

                Point3D offset = new Point3D(0.0f, -height / 2, -width / 2);
                fWater.Initialize(new Point3D[] { pt1t, pt2t, pt3t, pt4t }, offset);
            }

            if (ShowWater) {
                SetWaterMaterial();
                DrawBox(x1w, x2w, y1w, y2w, z1w, z2w, AllSidesWT); // without top (water) surface

                if (Aeration) {
                    var aeraPt = new Point3D(0.0f, 0.0f, width / 2.0f);
                    fAeration.DrawBubbles(fScene, aeraPt, watHeight, surfacedBubbles);
                    // surfacedBubbles: from aeration to water surface
                }

                fWater.Draw(fScene);
            }

            // required without condition!
            fWater.Next(surfacedBubbles, !Aeration);

            fScene.EndTransparentRendering();

            // Add a light fixture - a silver (aluminum) narrow and thin plate in the middle of the aquarium
            // The plate is positioned at the top center of the aquarium
            if (ShowLighter) {
                // Save the current matrix
                fScene.PushMatrix();

                // Position the light fixture at the top center of the aquarium
                float lightY = height + thickness * 4; // At the top, slightly below the lid
                fScene.Translatef(0.0f, lightY, 0.0f);

                // Set aluminum material for the light fixture
                fScene.SetMaterial(M3DMaterials.AlumDiffuse, M3DMaterials.AlumSpecular, M3DMaterials.AlumShininess);

                // Light fixture dimensions
                float lightLength = length - 2 * thickness; // Slightly smaller than the internal length
                float lightWidth = width * 0.1f;   // Narrow strip
                float lightHeight = thickness * 2;       // Very thin plate (approximately twice as thick as the wall glass)

                // x <- length, z <- width
                var zoffs = (width - 2 * thickness - lightWidth) / 2;

                // Draw the light fixture as a thin plate
                var x1l = x1w;
                var x2l = x2w;
                var y1l = 0.0f;
                var y2l = -lightHeight; // Thin plate downward from the current position
                var z1l = z1w + zoffs;
                var z2l = z2w - zoffs;

                // Top surface of the light fixture (main)
                DrawBox(x1l, x2l, y1l, y2l, z1l, z2l);

                // Add a light source along the entire bottom surface of the light fixture
                fScene.SetLight(2,
                    new float[] { 0.8f, 0.8f, 0.8f, 1.0f }, // ambient
                    new float[] { 1.0f, 1.0f, 1.0f, 1.0f }, // diffuse
                    new float[] { 1.0f, 1.0f, 1.0f, 1.0f }, // specular
                    new float[] { 0.0f, lightY - lightHeight / 2, 0.0f, 1.0f } // position
                );

                // Restore the matrix
                fScene.PopMatrix();
            } else {
                fScene.UnsetLight(2);
            }

            // front left top point - for temperature
            if (ShowInfo) {
                fScene.DrawText("T: 25 °C", fltX, fltY, fltZ);
            }
        }

        public void DrawRect(Point3D p1, Point3D p2, Point3D p3, Point3D p4, Point3D normal)
        {
            Point3D pm = Point3D.GetLineMidpoint(p1, p3);

            fScene.DrawTriangle(p1, pm, p2, normal);
            fScene.DrawTriangle(p2, pm, p3, normal);
            fScene.DrawTriangle(p3, pm, p4, normal);
            fScene.DrawTriangle(p4, pm, p1, normal);

            /*OpenGL.glBegin(OpenGL.GL_POLYGON);
            OpenGL.glVertex3f(p1.X, p1.Y, p1.Z);
            OpenGL.glVertex3f(p2.X, p2.Y, p2.Z);
            OpenGL.glVertex3f(p3.X, p3.Y, p3.Z);
            OpenGL.glVertex3f(p4.X, p4.Y, p4.Z);
            OpenGL.glEnd();*/
        }

        public void DrawBox(Point3D p1, Point3D p2, Point3D p3, Point3D p4,
                                   Point3D p5, Point3D p6, Point3D p7, Point3D p8,
                                   BoxSides sides = AllSides)
        {
            // GL_CCW(default), GL_CW
            //GL.glFrontFace(GL.GL_CW);

            if (sides.HasFlag(BoxSides.Top)) DrawRect(p1, p2, p3, p4, new Point3D(0.0f, 1.0f, 0.0f));
            if (sides.HasFlag(BoxSides.Bottom)) DrawRect(p5, p6, p7, p8, new Point3D(0.0f, -1.0f, 0.0f));

            if (sides.HasFlag(BoxSides.Back)) DrawRect(p1, p2, p6, p5, new Point3D(0.0f, 0.0f, -1.0f));
            if (sides.HasFlag(BoxSides.Right)) DrawRect(p2, p3, p7, p6, new Point3D(+1.0f, 0.0f, 0.0f));
            if (sides.HasFlag(BoxSides.Front)) DrawRect(p3, p4, p8, p7, new Point3D(0.0f, 0.0f, +1.0f));
            if (sides.HasFlag(BoxSides.Left)) DrawRect(p1, p4, p8, p5, new Point3D(-1.0f, 0.0f, 0.0f));
        }

        public void DrawBox(float x1, float x2, float y1, float y2, float z1, float z2, BoxSides sides = AllSides)
        {
            DrawBox(
                new Point3D(x1, y1, z1), new Point3D(x2, y1, z1), new Point3D(x2, y1, z2), new Point3D(x1, y1, z2),
                new Point3D(x1, y2, z1), new Point3D(x2, y2, z1), new Point3D(x2, y2, z2), new Point3D(x1, y2, z2), sides);
        }

        protected static IList<Point3D> GetArcPoints(int slices, float radius, float startAngle, float wedgeAngle)
        {
            var points = new List<Point3D>();

            float angleStep = wedgeAngle / slices;
            for (int j = 0; j <= slices; ++j) {
                double a = MathHelper.DegreesToRadians(startAngle + j * angleStep);
                float x = radius * (float)Math.Cos(a);
                float z = radius * (float)Math.Sin(a);
                points.Add(new Point3D(x, 0.0f, z));
            }

            return points;
        }

        protected void DrawCylinder(IList<Point3D> points, float height, float radius)
        {
            float y1 = 0.0f;
            float y2 = y1 + height;

            fScene.BeginTriangleStrip();
            for (int j = 0; j < points.Count; ++j) {
                var pt = points[j];

                fScene.Normal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt.X, y1, pt.Z);

                fScene.Normal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt.X, y2, pt.Z);
            }
            fScene.End();
        }

        protected void DrawCylinder(int slices, float height, float radius, float startAngle, float wedgeAngle)
        {
            var points = GetArcPoints(slices, radius, startAngle, wedgeAngle);
            DrawCylinder(points, height, radius);
        }
    }
}
