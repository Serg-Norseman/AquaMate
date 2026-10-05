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
