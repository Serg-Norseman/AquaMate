/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
