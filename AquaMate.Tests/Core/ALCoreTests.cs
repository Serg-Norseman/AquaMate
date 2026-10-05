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

namespace AquaMate.Core
{
    [TestFixture]
    public class ALCoreTests
    {
        [Test]
        public void Test_Common()
        {
        }

        [Test]
        public void Test_GetItemType_IT()
        {
            Assert.AreEqual(ItemType.Additive, ALCore.GetItemType(InventoryType.Additive));
            Assert.AreEqual(ItemType.Chemistry, ALCore.GetItemType(InventoryType.Chemistry));
            Assert.AreEqual(ItemType.Equipment, ALCore.GetItemType(InventoryType.Equipment));
            Assert.AreEqual(ItemType.Maintenance, ALCore.GetItemType(InventoryType.Maintenance));
            Assert.AreEqual(ItemType.Furniture, ALCore.GetItemType(InventoryType.Furniture));
            Assert.AreEqual(ItemType.Decoration, ALCore.GetItemType(InventoryType.Decoration));
        }

        [Test]
        public void Test_GetItemType_ST()
        {
            Assert.AreEqual(ItemType.Fish, ALCore.GetItemType(SpeciesType.Fish));
            Assert.AreEqual(ItemType.Invertebrate, ALCore.GetItemType(SpeciesType.Invertebrate));
            Assert.AreEqual(ItemType.Plant, ALCore.GetItemType(SpeciesType.Plant));
            Assert.AreEqual(ItemType.Coral, ALCore.GetItemType(SpeciesType.Coral));
        }

        [Test]
        public void Test_IsAnimal()
        {
            Assert.AreEqual(true, ALCore.IsAnimal(SpeciesType.Fish));
            Assert.AreEqual(true, ALCore.IsAnimal(SpeciesType.Invertebrate));
            Assert.AreEqual(false, ALCore.IsAnimal(SpeciesType.Plant));
            Assert.AreEqual(false, ALCore.IsAnimal(SpeciesType.Coral));
        }

        [Test]
        public void Test_IsInhabitant()
        {
            Assert.AreEqual(true, ALCore.IsInhabitant(ItemType.Fish));
            Assert.AreEqual(true, ALCore.IsInhabitant(ItemType.Invertebrate));
            Assert.AreEqual(true, ALCore.IsInhabitant(ItemType.Plant));
            Assert.AreEqual(true, ALCore.IsInhabitant(ItemType.Coral));
        }

        [Test]
        public void Test_IsLifesupport()
        {
            Assert.AreEqual(true, ALCore.IsLifesupport(ItemType.Nutrition));
            Assert.AreEqual(true, ALCore.IsLifesupport(ItemType.Device));
            Assert.AreEqual(true, ALCore.IsLifesupport(ItemType.Additive));
            Assert.AreEqual(true, ALCore.IsLifesupport(ItemType.Chemistry));
            Assert.AreEqual(false, ALCore.IsLifesupport(ItemType.Equipment));
        }
    }
}
