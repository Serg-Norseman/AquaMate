/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.ComponentModel;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class BowlTank : BaseTank
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

        /// <summary>
        ///  (cm).
        /// </summary>
        [Browsable(true), DisplayName("TopDiameter")]
        public float TopDiameter { get; set; }


        public BowlTank()
        {
        }

        public BowlTank(float height, float bottomDiameter, float topDiameter, float glassThickness = 0.0f)
        {
            Height = height;
            BottomDiameter = bottomDiameter;
            TopDiameter = topDiameter;
            GlassThickness = glassThickness;
        }

        public override TankShape GetTankShape()
        {
            return TankShape.Bowl;
        }

        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "Height", ALData.GetLSuom(LSID.Height, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "BottomDiameter", ALData.GetLSuom(LSID.BottomDiameter, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "TopDiameter", ALData.GetLSuom(LSID.TopDiameter, MeasurementType.Length));
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
            double bottomRadius = BottomDiameter / 2.0f;
            double topRadius = TopDiameter / 2.0f;

            if (glassThickness > 0.0d) {
                height -= glassThickness;
                bottomRadius -= glassThickness;
                topRadius -= glassThickness;
            }

            double ccVolume = ((Math.PI * height * height * height) / 6.0f)
                + ((Math.PI * height * (bottomRadius * bottomRadius + topRadius * topRadius)) / 2.0f);
            return UnitConverter.cc2l(ccVolume);
        }

        public override double CalcWaterVolume(double underfillHeight, double soilHeight)
        {
            // FIXME
            return 0.0d;
        }
    }
}
