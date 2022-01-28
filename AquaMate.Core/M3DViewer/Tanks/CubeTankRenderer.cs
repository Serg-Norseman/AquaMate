/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Model.Tanks;

namespace AquaMate.M3DViewer.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class CubeTankRenderer : TankRenderer<CubeTank>
    {
        public CubeTankRenderer(SceneRenderer sceneRenderer, CubeTank tank) : base(sceneRenderer, tank)
        {
        }

        public override void Render(bool showWater = true, bool aeration = false, bool showInfo = false)
        {
            DrawRectangularTank(fTank.EdgeSize, fTank.EdgeSize, fTank.EdgeSize, fTank.GlassThickness, showWater, aeration, showInfo);
        }
    }
}
