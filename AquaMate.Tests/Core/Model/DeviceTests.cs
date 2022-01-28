/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Types;
using NUnit.Framework;

namespace AquaMate.Core.Model
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public void Test_Common()
        {
            var light = new Device();
            Assert.IsNotNull(light);

            light.Name = "Light";
            Assert.AreEqual("Light", light.Name);
            Assert.AreEqual("Light", light.ToString());

            light.Brand = "brand";
            Assert.AreEqual("brand", light.Brand);

            light.Digital = true;
            Assert.AreEqual(true, light.Digital);

            light.Enabled = true;
            Assert.AreEqual(true, light.Enabled);

            light.Power = 8.0d;
            Assert.AreEqual(8.0d, light.Power);

            light.State = ItemState.InUse;
            Assert.AreEqual(ItemState.InUse, light.State);
        }
    }
}
