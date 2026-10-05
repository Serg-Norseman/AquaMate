/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using NUnit.Framework;

namespace AquaMate.Core.Model
{
    [TestFixture]
    public class MaintenanceTests
    {
        [Test]
        public void Test_Common()
        {
            var maintenance = new Maintenance();
            Assert.IsNotNull(maintenance);

            maintenance.Value = 2.5;
            Assert.AreEqual(2.5, maintenance.Value);
        }
    }
}
