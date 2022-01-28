/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using NUnit.Framework;

namespace AquaMate.Core.Model
{
    [TestFixture]
    public class MeasureTests
    {
        [Test]
        public void Test_Common()
        {
            var measure = new Measure();
            Assert.IsNotNull(measure);

            measure.Temperature = 2.5f;
            Assert.AreEqual(2.5f, measure.Temperature);

            measure.pH = 7.5f;
            Assert.AreEqual(7.5f, measure.pH);

            Assert.AreEqual("T=2.50, pH=7.50", measure.ToString());
        }
    }
}
