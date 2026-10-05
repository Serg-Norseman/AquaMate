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
    public class InventoryTests
    {
        [Test]
        public void Test_Common()
        {
            var invent = new Inventory();
            Assert.IsNotNull(invent);

            invent.Name = "inventory";
            Assert.AreEqual("inventory", invent.Name);
            Assert.AreEqual("inventory", invent.ToString());

            invent.Brand = "brand";
            Assert.AreEqual("brand", invent.Brand);

            invent.Note = "note";
            Assert.AreEqual("note", invent.Note);

            invent.Type = InventoryType.Chemistry;
            Assert.AreEqual(InventoryType.Chemistry, invent.Type);

            invent.State = ItemState.InUse;
            Assert.AreEqual(ItemState.InUse, invent.State);

            invent.Type = InventoryType.Decoration;
            Decoration decor = invent.Properties as Decoration;
            Assert.IsNotNull(decor);

            decor.Size = 1.5f;
            Assert.AreEqual(1.5f, decor.Size);

            decor.Weight = 2.7f;
            Assert.AreEqual(2.7f, decor.Weight);
        }
    }
}
