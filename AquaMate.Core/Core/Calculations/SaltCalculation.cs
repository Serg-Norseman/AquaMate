/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
