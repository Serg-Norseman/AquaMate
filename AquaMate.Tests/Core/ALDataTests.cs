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
    public class ALDataTests
    {
        [Test]
        public void Test_CalcBaseArea()
        {
            var tank = new RectangularTank(17.5f, 20.5f, 26.5f);
            Assert.AreEqual(358.75d, tank.CalcBaseArea());
        }

        [Test]
        public void Test_CalcTankVolume()
        {
            var tank = new RectangularTank(17.5f, 20.5f, 26.5f);
            Assert.AreEqual(9.506875d, tank.CalcTankVolume());
        }

        [Test]
        public void Test_CalcWaterVolume()
        {
            //Assert.AreEqual(17.0f, ALData.CalcWaterVolume(20.0f));
        }
    }
}
