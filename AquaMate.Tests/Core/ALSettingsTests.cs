/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Types;
using BSLib;
using NUnit.Framework;

namespace AquaMate.Core
{
    [TestFixture]
    public class ALSettingsTests
    {
        [Test]
        public void Test_Common()
        {
            var instance = ALSettings.Instance;
            Assert.IsNotNull(instance);

            instance.HideClosedTanks = true;
            Assert.AreEqual(true, instance.HideClosedTanks);

            instance.ExitOnClose = true;
            Assert.AreEqual(true, instance.ExitOnClose);

            instance.CurrentLocale = 1033;
            Assert.AreEqual(1033, instance.CurrentLocale);

            instance.HideAtStartup = true;
            Assert.AreEqual(true, instance.HideAtStartup);


            instance.LengthUoM = MeasurementUnit.Inch;
            Assert.AreEqual(MeasurementUnit.Inch, instance.LengthUoM);

            instance.VolumeUoM = MeasurementUnit.USGallon;
            Assert.AreEqual(MeasurementUnit.USGallon, instance.VolumeUoM);

            var ini = new IniFile();

            instance.SaveToFile(ini);
            instance.LoadFromFile(ini);

            ini = null;
            Assert.Throws(typeof(ArgumentNullException), () => { instance.SaveToFile(ini); });
            Assert.Throws(typeof(ArgumentNullException), () => { instance.LoadFromFile(ini); });

            string iniFile = null;
            Assert.Throws(typeof(ArgumentNullException), () => { instance.SaveToFile(iniFile); });
            Assert.Throws(typeof(ArgumentNullException), () => { instance.LoadFromFile(iniFile); });
        }
    }
}
