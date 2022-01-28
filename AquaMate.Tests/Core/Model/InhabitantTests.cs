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
    public class InhabitantTests
    {
        [Test]
        public void Test_Common()
        {
            var fish = new Inhabitant();
            Assert.IsNotNull(fish);

            fish.Name = "Siamese fighting fish";
            Assert.AreEqual("Siamese fighting fish", fish.Name);
            Assert.AreEqual("Siamese fighting fish", fish.ToString());
        }
    }
}
