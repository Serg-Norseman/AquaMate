/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using AquaMate.Core.Model;

namespace AquaMate.M3DViewer.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RoundedTankRenderer<T> : TankRenderer<T> where T : ITank
    {
        protected RoundedTankRenderer(SceneRenderer sceneRenderer, T tank) : base(sceneRenderer, tank)
        {
        }

        protected void DrawCylinderFace(IList<Point3D> points1, IList<Point3D> points2, float y)
        {
            fScene.PushMatrix();
            fScene.BeginTriangleStrip();
            for (int j = 0; j < points1.Count; ++j) {
                var pt1 = points1[j];
                var pt2 = points2[j];
                //OpenGL.glNormal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt1.X, y, pt1.Z);
                //OpenGL.glNormal3f(pt.X / radius, 0.0f, pt.Z / radius);
                fScene.Vertex3f(pt2.X, y, pt2.Z);
            }
            fScene.End();
            fScene.PopMatrix();
        }

        protected void DrawDisk(IList<Point3D> points, float y)
        {
            fScene.BeginTriangleFan();
            for (int j = 0; j < points.Count; ++j) {
                var pt = points[j];
                fScene.Vertex3f(pt.X, y, pt.Z);
            }
            fScene.End();
        }
    }
}
