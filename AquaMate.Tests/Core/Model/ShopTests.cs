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
