/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
