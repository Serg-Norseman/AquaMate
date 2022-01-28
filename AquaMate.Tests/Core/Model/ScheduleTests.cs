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
