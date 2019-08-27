﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using NUnit.Framework;

namespace AquaLog.Core
{
    [TestFixture]
    public class UnitConverterTests
    {
        [Test]
        public void Test_cm2inch()
        {
            Assert.AreEqual(0.393701, UnitConverter.cm2inch(1.0f));
        }

        [Test]
        public void Test_inch2cm()
        {
            Assert.AreEqual(2.54, UnitConverter.inch2cm(1.0f));
        }

        [Test]
        public void Test_feet2cm()
        {
            Assert.AreEqual(30.48, UnitConverter.feet2cm(1.0f));
        }

        [Test]
        public void Test_cm2feet()
        {
            Assert.AreEqual(0.0328, UnitConverter.cm2feet(1.0f));
        }

        [Test]
        public void Test_gal2l()
        {
            Assert.AreEqual(3.78541178, UnitConverter.gal2l(1.0f));
        }

        [Test]
        public void Test_l2gal()
        {
            Assert.AreEqual(0.264172, UnitConverter.l2gal(1.0f));
        }

        [Test]
        public void Test_cc2l()
        {
            Assert.AreEqual(0.001, UnitConverter.cc2l(1.0f));
        }

        [Test]
        public void Test_l2cc()
        {
            Assert.AreEqual(1000, UnitConverter.l2cc(1.0f));
        }

        [Test]
        public void Test_mg2g()
        {
            Assert.AreEqual(0.001, UnitConverter.mg2g(1.0f));
        }

        [Test]
        public void Test_g2mg()
        {
            Assert.AreEqual(1000, UnitConverter.g2mg(1.0f));
        }

        [Test]
        public void Test_C2K()
        {
            Assert.AreEqual(274.15, UnitConverter.C2K(1.0f));
        }

        [Test]
        public void Test_K2C()
        {
            Assert.AreEqual(-272.15, UnitConverter.K2C(1.0f));
        }
    }
}
