/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
