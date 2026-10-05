/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.ComponentModel;

namespace AquaMate.Core.Calculations
{
    public sealed class SaltCalculation : BaseCalculation
    {
        [Browsable(true), DisplayName("Volume"), Category("Arguments"), Description("")]
        public double Volume { get; set; }

        [Browsable(true), DisplayName("Nitrite"), Category("Arguments"), Description("")]
        public double Nitrite { get; set; }

        public SaltCalculation(CalculationType type) : base(type)
        {
        }

        public override void Calculate()
        {
            ResultValue = ALData.CalcSalt(Volume, Nitrite);
        }
    }
}
