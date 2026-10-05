/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
