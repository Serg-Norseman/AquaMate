/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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

        public override void Render()
        {
            float height = fTank.Height;
            float bottomDiameter = fTank.BottomDiameter;
            float topDiameter = fTank.TopDiameter;
            float thickness = fTank.GlassThickness;
            float underfillHeight = fTank.UnderfillHeight;

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

            if (ShowWater) {
                SetWaterMaterial();
                float watHeight = height - thickness - (underfillHeight * ScaleFactor);

                //M3DHelper.DrawCylinder(36, height, bottomDiameter / 2.0f, 0.0f, 360.0f);

                if (Aeration) {
                    var aeraPt = new Point3D(0.0f, 0.0f, bottomDiameter / 2.0f);
                    var surfacedBubbles = new List<M3DBubble>();
                    fAeration.DrawBubbles(fScene, aeraPt, watHeight, surfacedBubbles);
                }
            }
        }
    }
}
