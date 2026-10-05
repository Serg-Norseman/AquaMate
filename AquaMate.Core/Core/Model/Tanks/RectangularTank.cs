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
    public class RectangularTank : RegularShapeTank
    {
        /// <summary>
        /// The width of an aquarium is the distance from front to back (cm).
        /// </summary>
        [Browsable(true), DisplayName("Width")]
        public float Width { get; set; }

        /// <summary>
        /// The length of an aquarium is the distance across the front (cm).
        /// </summary>
        [Browsable(true), DisplayName("Length")]
        public float Length { get; set; }

        /// <summary>
        /// The height of an aquarium is the distance from top to bottom (cm).
        /// </summary>
        [Browsable(true), DisplayName("Height")]
        public float Height { get; set; }


        public RectangularTank()
        {
        }

        public RectangularTank(float length, float width, float height, float glassThickness = 0.0f)
        {
            Length = length;
            Width = width;
            Height = height;
            GlassThickness = glassThickness;
        }

        public override TankShape GetTankShape()
        {
            return TankShape.Rectangular;
        }

        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "Length", ALData.GetLSuom(LSID.Length, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "Width", ALData.GetLSuom(LSID.Width, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "Height", ALData.GetLSuom(LSID.Height, MeasurementType.Length));
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public override double CalcBaseArea()
        {
            double glassThickness = GlassThickness;
            double width = Width;
            double length = Length;

            if (glassThickness > 0.0d) {
                double thicknessX2 = glassThickness * 2.0d;

                width -= thicknessX2; // two sides
                length -= thicknessX2; // two sides
            }

            return width * length;
        }

        /// <summary>
        /// Calculate the volume of a tank (litres, all sizes in cm).
        /// </summary>
        public override double CalcTankVolume()
        {
            double glassThickness = GlassThickness;
            double height = Height;

            if (glassThickness > 0.0d) {
                height -= glassThickness; // only bottom
            }

            double baseArea = CalcBaseArea();
            double ccVolume = baseArea * height; // cubic cm (cc)
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
