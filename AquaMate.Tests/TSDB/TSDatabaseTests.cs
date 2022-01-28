/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Types;
using NUnit.Framework;

namespace AquaMate.TSDB
{
    [TestFixture]
    public class TSDatabaseTests
    {
        [Test]
        public void Test_Common()
        {
            var instance = new TSDatabase();
            Assert.IsNotNull(instance);

            TSPoint point = new TSPoint();
            point.Name = "temperature test";
            point.Type = MeasurementType.Temperature;
            Assert.AreEqual("temperature test", point.ToString());
            instance.AddPoint(point);

            point.Name = "temperature test 2";
            instance.UpdatePoint(point);

            point = instance.GetPoint(point.Id);
            Assert.IsNotNull(point);
            instance.DeletePoint(point);
        }
    }
}
