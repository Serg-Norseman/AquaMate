/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.ComponentModel;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model.Tanks
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseTank : ITank
    {
        [Browsable(true), DisplayName("GlassThickness")]
        public float GlassThickness { get; set; }

        public BaseTank()
        {
        }

        public ITank Clone()
        {
            return (ITank)this.MemberwiseClone();
        }

        public virtual TankShape GetTankShape()
        {
            return TankShape.Unknown;
        }

        public virtual void SetPropNames()
        {
            ALCore.SetDisplayNameValue(this, "GlassThickness", ALData.GetLSuom(LSID.GlassThickness, MeasurementType.Length));
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public virtual double CalcBaseArea()
        {
            return 0.0d;
        }

        /// <summary>
        /// Calculate the volume of a tank (litres, all sizes in cm).
        /// </summary>
        public virtual double CalcTankVolume()
        {
            return 0.0d;
        }

        public virtual double CalcWaterVolume(double underfillHeight, double soilHeight)
        {
            return 0.0d;
        }

        /// <summary>
        /// Estimated soil volume.
        /// </summary>
        public virtual double CalcSoilVolume(double soilHeight)
        {
            return 0.0d;
        }
    }
}
