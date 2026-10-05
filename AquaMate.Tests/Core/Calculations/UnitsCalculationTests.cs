/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using NUnit.Framework;

namespace AquaMate.Core.Calculations
{
    [TestFixture]
    public class UnitsCalculationTests
    {
        public UnitsCalculationTests()
        {
            Localizer.DefInit();
        }

        [Test]
        public void Test_Common()
        {
            var instance = new UnitsCalculation(CalculationType.Units_inch2cm);
            Assert.IsNotNull(instance);

            instance.SourceValue = 1.0f;
            instance.Calculate();
            Assert.AreEqual(2.54f, instance.ResultValue, 0.01);

            Assert.IsNotNull(instance.Description);
        }
    }
}
