/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Model.Tanks;
using NUnit.Framework;

namespace AquaMate.Core
{
    [TestFixture]
    public class StringSerializerTests
    {
        [Test]
        public void Test_Common()
        {
            var tank = new RectangularTank(10.5f, 20.5f, 30.5f);

            string result = StringSerializer.Serialize(null);
            Assert.AreEqual(string.Empty, result);

            result = StringSerializer.Serialize(tank);
            Assert.AreEqual("Width=20.5;Length=10.5;Height=30.5;GlassThickness=0", result);

            var tank2 = StringSerializer.Deserialize<RectangularTank>(result);
            Assert.AreEqual(10.5f, tank2.Length);
            Assert.AreEqual(20.5f, tank2.Width);
            Assert.AreEqual(30.5f, tank2.Height);
        }
    }
}
