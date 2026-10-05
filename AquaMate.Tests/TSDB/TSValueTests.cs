/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
