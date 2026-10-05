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
    public class TransferTests
    {
        [Test]
        public void Test_Common()
        {
            var transfer = new Transfer();
            Assert.IsNotNull(transfer);

            transfer.Type = TransferType.Relocation;
            Assert.AreEqual(TransferType.Relocation, transfer.Type);
        }
    }
}
