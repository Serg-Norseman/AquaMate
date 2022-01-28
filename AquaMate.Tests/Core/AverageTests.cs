/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
