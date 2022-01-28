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
    public class NoteTests
    {
        [Test]
        public void Test_Common()
        {
            var instance = new Note();
            Assert.IsNotNull(instance);

            instance.Event = "event";
            Assert.AreEqual("event", instance.Event);

            instance.Content = "content";
            Assert.AreEqual("content", instance.Content);

            Assert.AreEqual("event", instance.ToString());
        }
    }
}
