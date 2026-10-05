/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
