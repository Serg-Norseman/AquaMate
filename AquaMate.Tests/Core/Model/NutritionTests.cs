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
    public class NutritionTests
    {
        [Test]
        public void Test_Common()
        {
            var nutrition = new Nutrition();
            Assert.IsNotNull(nutrition);

            nutrition.Name = "nutrition";
            Assert.AreEqual("nutrition", nutrition.Name);
            Assert.AreEqual("nutrition", nutrition.ToString());

            nutrition.Amount = 2.5f;
            Assert.AreEqual(2.5f, nutrition.Amount);
        }
    }
}
