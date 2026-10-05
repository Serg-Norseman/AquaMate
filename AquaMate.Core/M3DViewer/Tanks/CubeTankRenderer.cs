/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
