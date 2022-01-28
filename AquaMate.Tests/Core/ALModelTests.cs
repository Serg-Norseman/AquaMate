/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
