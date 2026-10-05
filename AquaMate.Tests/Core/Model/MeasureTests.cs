/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
