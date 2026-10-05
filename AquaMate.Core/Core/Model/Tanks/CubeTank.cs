/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.ComponentModel;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class CubeTank : RegularShapeTank
    {
        [Browsable(true), DisplayName("EdgeSize")]
        public float EdgeSize { get; set; }


        public CubeTank()
        {
        }

        public CubeTank(float edgeSize, float glassThickness = 0.0f)
        {
            EdgeSize = edgeSize;
            GlassThickness = glassThickness;
        }

        public override TankShape GetTankShape()
        {
            return TankShape.Cube;
        }

        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "EdgeSize", ALData.GetLSuom(LSID.EdgeSize, MeasurementType.Length));
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public override double CalcBaseArea()
        {
            double glassThickness = GlassThickness;
            double edgeSize = EdgeSize;

            if (glassThickness > 0.0d) {
                double thicknessX2 = glassThickness * 2.0d;

                edgeSize -= thicknessX2; // two sides
            }

            return edgeSize * edgeSize;
        }

        /// <summary>
        /// Calculate the volume of a tank (litres, all sizes in cm).
        /// </summary>
        public override double CalcTankVolume()
        {
            double glassThickness = GlassThickness;
            double height = EdgeSize;

            if (glassThickness > 0.0d) {
                height -= glassThickness; // only bottom
            }

            double baseArea = CalcBaseArea();
            double ccVolume = baseArea * height; // cubic cm (cc)
            return UnitConverter.cc2l(ccVolume);
        }

        public override double CalcWaterVolume(double underfillHeight, double soilHeight)
        {
            double waterHeight = (EdgeSize - GlassThickness) - underfillHeight - soilHeight;
            double ccVolume = CalcBaseArea() * waterHeight;
            return UnitConverter.cc2l(ccVolume);
        }
    }
}
