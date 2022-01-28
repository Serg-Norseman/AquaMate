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
        public void Test_tsp2cc()
        {
            Assert.AreEqual(14.786765, UnitConverter.tsp2cc(1.0f));
        }

        [Test]
        public void Test_cc2tsp()
        {
            Assert.AreEqual(0.067628, UnitConverter.cc2tsp(1.0f));
        }

        [Test]
        public void Test_tsp2g()
        {
            Assert.AreEqual(5.0, UnitConverter.tsp2g(1.0f));
        }

        [Test]
        public void Test_g2tsp()
        {
            Assert.AreEqual(0.2, UnitConverter.g2tsp(1.0f));
        }

        [Test]
        public void Test_g2oz()
        {
            Assert.AreEqual(0.035274, UnitConverter.g2oz(1.0f));
        }

        [Test]
        public void Test_oz2g()
        {
            Assert.AreEqual(28.3495, UnitConverter.oz2g(1.0f));
        }

        [Test]
        public void Test_kg2lb()
        {
            Assert.AreEqual(2.204623, UnitConverter.kg2lb(1.0f));
        }

        [Test]
        public void Test_lb2kg()
        {
            Assert.AreEqual(0.9071839, UnitConverter.lb2kg(2.0f), 0.0000001);
        }

        [Test]
        public void Test_ml2drops()
        {
            Assert.AreEqual(20.0, UnitConverter.ml2drops(1.0f));
        }

        [Test]
        public void Test_drops2ml()
        {
            Assert.AreEqual(0.05, UnitConverter.drops2ml(1.0f));
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
