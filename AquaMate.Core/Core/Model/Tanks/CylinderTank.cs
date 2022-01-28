/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.ComponentModel;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class CylinderTank : RegularShapeTank
    {
        /// <summary>
        /// The height of an aquarium is the distance from top to bottom (cm).
        /// </summary>
        [Browsable(true), DisplayName("Height")]
        public float Height { get; set; }

        /// <summary>
        ///  (cm).
        /// </summary>
        [Browsable(true), DisplayName("BottomDiameter")]
        public float BottomDiameter { get; set; }


        public CylinderTank()
        {
        }

        public CylinderTank(float height, float bottomDiameter, float glassThickness = 0.0f)
        {
            Height = height;
            BottomDiameter = bottomDiameter;
            GlassThickness = glassThickness;
        }

        public override TankShape GetTankShape()
        {
            return TankShape.Cylinder;
        }

        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "Height", ALData.GetLSuom(LSID.Height, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "BottomDiameter", ALData.GetLSuom(LSID.BottomDiameter, MeasurementType.Length));
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public override double CalcBaseArea()
        {
            double glassThickness = GlassThickness;
            double bottomRadius = BottomDiameter / 2.0f;

            if (glassThickness > 0.0d) {
                bottomRadius -= glassThickness;
            }

            return Math.PI * bottomRadius * bottomRadius;
        }

        /// <summary>
        /// Calculate the volume of a tank (litres, all sizes in cm).
        /// </summary>
        public override double CalcTankVolume()
        {
            double glassThickness = GlassThickness;
            double height = Height;

            if (glassThickness > 0.0d) {
                height -= glassThickness;
            }

            var baseArea = CalcBaseArea();
            double ccVolume = baseArea * height;
            return UnitConverter.cc2l(ccVolume);
        }

        public override double CalcWaterVolume(double underfillHeight, double soilHeight)
        {
            double waterHeight = (Height - GlassThickness) - underfillHeight - soilHeight;
            double ccVolume = CalcBaseArea() * waterHeight;
            return UnitConverter.cc2l(ccVolume);
        }
    }
}
