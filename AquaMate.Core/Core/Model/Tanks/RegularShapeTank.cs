/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core.Model.Tanks
{
    public class RegularShapeTank : BaseTank
    {
        public RegularShapeTank()
        {
        }

        /// <summary>
        /// Estimated soil volume.
        /// </summary>
        public override double CalcSoilVolume(double soilHeight)
        {
            double ccVolume = CalcBaseArea() * soilHeight;
            return UnitConverter.cc2l(ccVolume);
        }
    }
}
