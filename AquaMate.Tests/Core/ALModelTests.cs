/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Model;
using NUnit.Framework;

namespace AquaMate.Core
{
    [TestFixture]
    public class ALModelTests
    {
        [Test]
        public void Test_ctor()
        {
            var instance = new ALModel(null);
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Test_CleanSpace()
        {
            var instance = new ALModel(null);
            instance.CleanSpace();
        }

        [Test]
        public void Test_AddRecord()
        {
            var instance = new ALModel(null);

            var aqm = new Aquarium();

            instance.AddRecord(aqm);
            Assert.IsNotNull(instance.GetRecord<Aquarium>(aqm.Id));
        }

        [Test]
        public void Test_UpdateRecord()
        {
            var instance = new ALModel(null);

            var aqm = new Aquarium();
            aqm.Name = "test aquarium";
            instance.AddRecord(aqm);
            aqm = instance.GetRecord<Aquarium>(aqm.Id);
            Assert.AreEqual("test aquarium", aqm.Name);

            aqm.Name = "test2";
            instance.UpdateRecord(aqm);
            var aqm2 = instance.GetRecord<Aquarium>(aqm.Id);
            Assert.AreEqual("test2", aqm2.Name);
        }

        [Test]
        public void Test_CollectData()
        {
            var instance = new ALModel(null);

            instance.CollectData(null);

            var aqm = new Aquarium();
            aqm.Name = "test aquarium";
            instance.AddRecord(aqm);

            instance.CollectData(aqm);
        }
    }
}
