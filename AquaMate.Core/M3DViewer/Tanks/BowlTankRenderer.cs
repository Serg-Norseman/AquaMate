/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using AquaMate.Core.Model.Tanks;

namespace AquaMate.M3DViewer.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class BowlTankRenderer : RoundedTankRenderer<BowlTank>
    {
        public BowlTankRenderer(SceneRenderer sceneRenderer, BowlTank tank) : base(sceneRenderer, tank)
        {
        }

        public override void Render(bool showWater = true, bool aeration = false, bool showInfo = false)
        {
            float height = fTank.Height;
            float bottomDiameter = fTank.BottomDiameter;
            float topDiameter = fTank.TopDiameter;
            float thickness = fTank.GlassThickness;

            bottomDiameter *= ScaleFactor;
            topDiameter *= ScaleFactor;
            height *= ScaleFactor;
            thickness *= ScaleFactor;

            fScene.Translatef(0.0f, -height / 2, 0.0f);

            SetGlassMaterial();

            // bottom
            var points = GetArcPoints(36, bottomDiameter / 2.0f, 0.0f, 360.0f);
            DrawDisk(points, 0.0f);
            DrawDisk(points, 0.0f + thickness);

            // top face
            var points1i = GetArcPoints(36, (topDiameter / 2.0f) - thickness, 0.0f, 360.0f);
            var points1o = GetArcPoints(36, topDiameter / 2.0f, 0.0f, 360.0f);
            DrawCylinderFace(points1i, points1o, 0.0f + height);

            if (showWater) {
                SetWaterMaterial();
                float watHeight = height - thickness - (StdWaterOffset * ScaleFactor);

                //M3DHelper.DrawCylinder(36, height, bottomDiameter / 2.0f, 0.0f, 360.0f);

                if (aeration) {
                    var aeraPt = new Point3D(0.0f, 0.0f, bottomDiameter / 2.0f);
                    var surfacedBubbles = new List<M3DBubble>();
                    fAeration.DrawBubbles(fScene, aeraPt, watHeight, surfacedBubbles);
                }
            }
        }
    }
}
