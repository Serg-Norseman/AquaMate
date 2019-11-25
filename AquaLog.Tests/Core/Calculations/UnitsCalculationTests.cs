﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using NUnit.Framework;

namespace AquaLog.Core.Calculations
{
    [TestFixture]
    public class UnitsCalculationTests
    {
        [Test]
        public void Test_Common()
        {
            var instance = new UnitsCalculation(CalculationType.Units_inch2cm);
            Assert.IsNotNull(instance);

            instance.SourceValue = 1.0f;
            instance.Calculate();
            Assert.AreEqual(2.54f, instance.ResultValue, 0.01);
        }
    }
}
