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
    public class ShopTests
    {
        [Test]
        public void Test_Common()
        {
            var instance = new Shop();
            Assert.IsNotNull(instance);

            instance.Name = "Zoo-1";
            Assert.AreEqual("Zoo-1", instance.Name);

            instance.Address = "Lezh-100";
            Assert.AreEqual("Lezh-100", instance.Address);

            instance = new Shop("Zoo-1");
            Assert.IsNotNull(instance);
            Assert.AreEqual("Zoo-1", instance.ToString());
        }
    }
}
