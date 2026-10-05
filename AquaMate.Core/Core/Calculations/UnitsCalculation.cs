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
