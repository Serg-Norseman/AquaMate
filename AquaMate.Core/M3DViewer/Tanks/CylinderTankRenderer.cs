/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core.Model.Tanks;

namespace AquaMate.M3DViewer.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class CylinderTankRenderer : RoundedTankRenderer<CylinderTank>
    {
        public CylinderTankRenderer(SceneRenderer sceneRenderer, CylinderTank tank) : base(sceneRenderer, tank)
        {
        }

        public override void Render(bool showWater = true, bool aeration = false, bool showInfo = false)
        {
            float height = fTank.Height;
            float bottomDiameter = fTank.BottomDiameter;
            float thickness = fTank.GlassThickness;

            bottomDiameter *= ScaleFactor;
            height *= ScaleFactor;
            thickness *= ScaleFactor;

            fScene.Translatef(0.0f, -height / 2, 0.0f);

            SetGlassMaterial();

            // bottom
            var points = GetArcPoints(36, bottomDiameter / 2.0f, 0.0f, 360.0f);
            DrawDisk(points, 0.0f);
            DrawDisk(points, 0.0f + thickness);

            fScene.PushMatrix();
            // cylinder
            fScene.Translatef(0.0f, +thickness, 0.0f);
            var radI = (bottomDiameter / 2.0f) - thickness;
            var points1i = GetArcPoints(36, radI, 0.0f, 360.0f);
            DrawCylinder(points1i, height - thickness, radI);
            fScene.PopMatrix();

            var radO = bottomDiameter / 2.0f;
            var points1o = GetArcPoints(36, radO, 0.0f, 360.0f);
            DrawCylinder(points1o, height, radO);

            DrawCylinderFace(points1i, points1o, 0.0f + height);

            if (showWater) {
                SetWaterMaterial();
                float watHeight = height - thickness - (StdWaterOffset * ScaleFactor);

                DrawDisk(points1i, 0.0f + thickness);
                DrawDisk(points1i, 0.0f + thickness + watHeight);

                fScene.Translatef(0.0f, +thickness, 0.0f);
                DrawCylinder(36, watHeight, radI, 0.0f, 360.0f);

                if (aeration) {
                    var aeraPt = new Point3D(0.0f, 0.0f, bottomDiameter / 2.0f);
                    var surfacedBubbles = new List<M3DBubble>();
                    fAeration.DrawBubbles(fScene, aeraPt, watHeight, surfacedBubbles);
                }
            }
        }
    }
}
