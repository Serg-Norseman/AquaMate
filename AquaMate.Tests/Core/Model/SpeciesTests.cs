/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using NUnit.Framework;

namespace AquaMate.Core.Model
{
    [TestFixture]
    public class SpeciesTests
    {
        [Test]
        public void Test_Common()
        {
            var species = new Species();
            Assert.IsNotNull(species);

            species.Name = "Siamese fighting fish";
            Assert.AreEqual("Siamese fighting fish", species.Name);
            Assert.AreEqual("Siamese fighting fish", species.ToString());

            species.BioFamily = "Hydrocharitaceae";
            Assert.AreEqual("Hydrocharitaceae", species.BioFamily);
        }

        [Test]
        public void Test_GetGHRange()
        {
            var species = new Species();

            species.GHMin = 0.0f;
            species.GHMax = 0.0f;
            Assert.AreEqual(string.Empty, species.GetGHRange());

            species.GHMin = 5.5f;
            species.GHMax = 8.5f;
            Assert.AreEqual("5.50 - 8.50", species.GetGHRange());
        }

        [Test]
        public void Test_GetPHRange()
        {
            var species = new Species();

            species.PHMin = 0.0f;
            species.PHMax = 0.0f;
            Assert.AreEqual(string.Empty, species.GetPHRange());

            species.PHMin = 5.5f;
            species.PHMax = 8.5f;
            Assert.AreEqual("5.50 - 8.50", species.GetPHRange());
        }

        [Test]
        public void Test_GetTempRange()
        {
            var species = new Species();

            species.TempMin = 0.0f;
            species.TempMax = 0.0f;
            Assert.AreEqual(string.Empty, species.GetTempRange());

            species.TempMin = 5.5f;
            species.TempMax = 8.5f;
            Assert.AreEqual("5.50 - 8.50", species.GetTempRange());
        }
    }
}
