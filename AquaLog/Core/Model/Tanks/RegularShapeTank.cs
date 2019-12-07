﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.ComponentModel;
using AquaLog.Core.Types;

namespace AquaLog.Core.Model.Tanks
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
