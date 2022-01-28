/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Types;
using NUnit.Framework;

namespace AquaMate.TSDB
{
    [TestFixture]
    public class TSValueTests
    {
        [Test]
        public void Test_TSPoint_Common()
        {
            var instance = new TSPoint();
            Assert.IsNotNull(instance);

            Assert.AreEqual(EntityType.TSPoint, instance.EntityType);
        }

        [Test]
        public void Test_ctor()
        {
            var instance = new TSValue();
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Test_ctor2()
        {
            var instance = new TSValue(DateTime.Now, 12345f);
            Assert.IsNotNull(instance);
        }
    }
}
