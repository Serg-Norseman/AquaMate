/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace AquaMate.M3DViewer
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SceneRenderer
    {
        public static readonly float[] LightAmbient = {0.5f, 0.5f, 0.5f, 0.95f};
        public static readonly float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};
        public static readonly float[] LightSpecular = {1.0f, 1.0f, 1.0f, 1.0f};
        public static readonly float[] LightPosition = {0.0f, 5.0f, -5.0f, 1.0f};

        public abstract void PushMatrix();

        public abstract void PopMatrix();

        public abstract void DrawSolidSphere(double radius, int slices, int stacks);

        public abstract void DrawTriangle(Point3D point1, Point3D point2, Point3D point3, Point3D normal);

        public abstract void Translatef(float x, float y, float z);

        public abstract void Rotatef(float angle, float x, float y, float z);

        public abstract void Vertex3f(float x, float y, float z);

        public abstract void Normal3f(float nx, float ny, float nz);

        public abstract void Color4f(float red, float green, float blue, float alpha);

        public abstract void SetLight(uint index, float[] ambiParams, float[] diffParams, float[] specParams, float[] pos);

        public abstract void SetMaterial(float[] diffParams, float[] specParams, float[] shin);

        public abstract void Begin(uint mode);

        public abstract void End();

        public abstract void BeginTriangleStrip();

        public abstract void BeginPolygon();

        public abstract void BeginTriangleFan();

        public abstract void SetViewport(int width, int height, float fovY, float zNear, float zFar);


        public Point3D CalculateSurfaceNormal(Point3D p1, Point3D p2, Point3D p3)
        {
            Vector3D u = p2.Sub(p1);
            Vector3D v = p3.Sub(p1);

            Point3D normal;
            normal.X = (u.Y * v.Z) - (u.Z * v.Y);
            normal.Y = (u.Z * v.X) - (u.X * v.Z);
            normal.Z = (u.X * v.Y) - (u.Y * v.X);

            float distance = (float)Math.Sqrt((normal.X * normal.X) + (normal.Y * normal.Y) + (normal.Z * normal.Z));
            normal.X = normal.X / distance;
            normal.Y = normal.Y / distance;
            normal.Z = normal.Z / distance;

            return normal;
        }

        public abstract void InitScene();

        public abstract void BeginDrawing();

        public abstract void EndDrawing();

        public abstract void DrawText(string text, float x, float y, float z);

        public abstract void DrawSphere(Point3D pt, double radius, int slices, int stacks);
    }
}
