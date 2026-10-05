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
