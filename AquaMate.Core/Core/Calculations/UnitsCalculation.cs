/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.ComponentModel;

namespace AquaMate.Core.Calculations
{
    public sealed class UnitsCalculation : BaseCalculation
    {
        [Browsable(true), DisplayName("SourceValue"), Category("Arguments"), Description("Value of argument")]
        public double SourceValue { get; set; }

        public UnitsCalculation(CalculationType type) : base(type)
        {
        }

        public override void Calculate()
        {
            var calcProps = CalculationData[(int)Type];
            ResultValue = calcProps.Handler(SourceValue);
        }
    }
}
