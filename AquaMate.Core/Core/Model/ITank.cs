/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITank : IEntity
    {
        ITank Clone();
        TankShape GetTankShape();
        void SetPropNames();


        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        double CalcBaseArea();

        /// <summary>
        /// Calculate the volume of a tank (litres, all sizes in cm).
        /// </summary>
        double CalcTankVolume();

        /// <summary>
        /// Estimated water volume.
        /// </summary>
        double CalcWaterVolume(double underfillHeight, double soilHeight);

        /// <summary>
        /// Estimated soil volume.
        /// </summary>
        double CalcSoilVolume(double soilHeight);
    }
}
