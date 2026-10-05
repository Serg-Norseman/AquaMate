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
    public class SaltCalculationTests
    {
        public SaltCalculationTests()
        {
            Localizer.DefInit();
        }

        [Test]
        public void Test_Common()
        {
            var instance = new SaltCalculation(CalculationType.NitriteSaltCalculator);
            Assert.IsNotNull(instance);

            instance.Volume = 10.0f;
            instance.Nitrite = 57.0f;
            instance.Calculate();
            Assert.AreEqual(4.275f, instance.ResultValue, 0.001);

            Assert.IsNotNull(instance.Description);
        }
    }
}
