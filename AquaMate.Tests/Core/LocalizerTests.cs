/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Model.Tanks;
using NUnit.Framework;

namespace AquaMate.Core
{
    [TestFixture]
    public class LocalizerTests
    {
        [Test]
        public void Test_LocaleFile_ctor()
        {
            var instance = new LocaleFile(1033, "en", "English", "english.lng");
            Assert.IsNotNull(instance);
            Assert.AreEqual("English", instance.ToString());
        }

        [Test]
        public void Test_Common()
        {
            Localizer.DefInit();

            Assert.AreEqual("File", Localizer.LS(LSID.File));

            Localizer.FindLocales();
            Assert.AreEqual(null, Localizer.GetLocaleByCode(1033));
        }
    }
}
