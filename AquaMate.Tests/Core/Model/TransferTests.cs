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
