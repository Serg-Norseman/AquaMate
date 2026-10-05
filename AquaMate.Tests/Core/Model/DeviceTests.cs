/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
