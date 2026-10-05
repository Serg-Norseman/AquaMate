/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using NUnit.Framework;

namespace AquaMate.Core
{
    [TestFixture]
    public class AverageTests
    {
        [Test]
        public void Test_Common()
        {
            var avg = Average.Create();

            avg.AddValue(1.0);
            avg.AddValue(3.0);
            avg.AddValue(5.0);
            avg.AddValue(7.0);

            Assert.AreEqual(4.0, avg.GetResult());
        }
    }
}
