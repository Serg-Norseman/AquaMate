/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
