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
    public class ScheduleTests
    {
        [Test]
        public void Test_Common()
        {
            var schedule = new Schedule();
            Assert.IsNotNull(schedule);

            schedule.Type = ScheduleType.Weekly;
            Assert.AreEqual(ScheduleType.Weekly, schedule.Type);
        }
    }
}
