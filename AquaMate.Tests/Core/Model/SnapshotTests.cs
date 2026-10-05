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
    public class SnapshotTests
    {
        [Test]
        public void Test_Common()
        {
            var snapshot = new Snapshot();
            Assert.IsNotNull(snapshot);

            snapshot.Name = "snapshot";
            Assert.AreEqual("snapshot", snapshot.Name);
            Assert.AreEqual("snapshot", snapshot.ToString());

            snapshot.Image = null;
            Assert.AreEqual(null, snapshot.Image);

            snapshot.Timestamp = ALCore.ZeroDate;
            Assert.IsTrue(ALCore.IsZeroDate(snapshot.Timestamp));

            snapshot.ItemType = ItemType.Maintenance;
            Assert.AreEqual(ItemType.Maintenance, snapshot.ItemType);
        }
    }
}
