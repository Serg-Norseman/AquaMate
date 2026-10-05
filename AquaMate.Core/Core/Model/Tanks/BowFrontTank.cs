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
    public class BowFrontTank : RectangularTank
    {
        /// <summary>
        /// The centre width of an aquarium is the distance from front to back in the center (cm).
        /// </summary>
        [Browsable(true), DisplayName("CentreWidth")]
        public float CentreWidth { get; set; }


        public BowFrontTank()
        {
        }

        public BowFrontTank(float length, float width, float centreWidth, float height, float glassThickness = 0.0f)
        {
            Length = length;
            Width = width;
            CentreWidth = centreWidth;
            Height = height;
            GlassThickness = glassThickness;
        }

        public override TankShape GetTankShape()
        {
            return TankShape.BowFront;
        }

        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "Length", ALData.GetLSuom(LSID.Length, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "Width", ALData.GetLSuom(LSID.Width, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "Height", ALData.GetLSuom(LSID.Height, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "CentreWidth", ALData.GetLSuom(LSID.CentreWidth, MeasurementType.Length));
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public override double CalcBaseArea()
        {
            double glassThickness = GlassThickness;
            double width = Width;
            double centreWidth = CentreWidth;
            double length = Length;

            if (glassThickness > 0.0d) {
                double thicknessX2 = glassThickness * 2.0d;

                width -= thicknessX2; // two sides
                centreWidth -= thicknessX2; // two sides
                length -= thicknessX2; // two sides
            }

            double chordWidth = centreWidth - width;

            float radius, wedgeAngle;
            ALData.CalcSegmentParams((float)chordWidth, (float)length, out radius, out wedgeAngle);

            double segmSquare = (radius * radius * (wedgeAngle - Math.Sin(wedgeAngle))) / 2.0d;
            double rectSquare = (length * width);
            double baseArea = segmSquare + rectSquare;

            return baseArea;
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
    }
}
