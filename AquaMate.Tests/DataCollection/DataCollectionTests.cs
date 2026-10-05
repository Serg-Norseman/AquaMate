/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Threading;
using NUnit.Framework;

namespace AquaMate.DataCollection
{
    internal sealed class TestTempChannel : BaseChannel
    {
        public override bool IsConnected
        {
            get { return true; }
        }

        public TestTempChannel() : base()
        {
        }

        public override void Send(string text)
        {
            if (text == "Q:temp;2") {
                // temperature query
                string response = "R:temp;sid:0000000000000000;val:25.1111;"; // temperature response
                ReceiveData(response);
            } else if (text == "Q:watlev;2") {
                // temperature query
                string response = "R:watlev;sid:0000000000000000;val:30.2222;"; // water level response
                ReceiveData(response);
            } else if (text == "Q:redox;2") {
                // temperature query
                string response = "R:redox;sid:0000000000000000;val:11.5555;"; // redox response
                ReceiveData(response);
            } else if (text == "Q:ph;2") {
                // temperature query
                string response = "R:ph;sid:0000000000000000;val:7.3333;"; // ph response
                ReceiveData(response);
            } else {
            }
        }
    }

    [TestFixture]
    public class DataCollectionTests
    {
#if !NETCOREAPP30
        [Test]
        public void Test_Common()
        {
            var serialChannel = new SerialChannel();
            Assert.IsNotNull(serialChannel);

            var tempService = new TemperatureService(serialChannel, 5000);
            Assert.IsNotNull(tempService);
            Assert.AreEqual(tempService.Channel, serialChannel);

            var ledService = new LEDService(serialChannel, 1000);
            Assert.IsNotNull(ledService);
            Assert.AreEqual(ledService.Channel, serialChannel);
        }
#endif

        [Test]
        public void Test_TemperatureService()
        {
            float temperature = 0.0f;

            var tempChannel = new TestTempChannel();
            tempChannel.ReceivedData += delegate (object sender, DataReceivedEventArgs e) {
                temperature = e.Value;
            };
            Assert.IsNotNull(tempChannel);
            tempChannel.Open(string.Empty);

            var tempService = new TemperatureService(tempChannel, 1000);
            tempChannel.Services.Add(tempService);
            Assert.IsNotNull(tempService);
            tempService.Enabled = true;
            Thread.Sleep(2000);
            Assert.AreEqual(25.1111f, temperature);

            Assert.AreEqual("Temperature", tempService.Name);
            Assert.AreEqual("temp", tempService.SensorName);

            tempChannel.Close();
        }

        [Test]
        public void Test_WaterLevelService()
        {
            float level = 0.0f;

            var tempChannel = new TestTempChannel();
            tempChannel.ReceivedData += delegate (object sender, DataReceivedEventArgs e) {
                level = e.Value;
            };
            Assert.IsNotNull(tempChannel);
            tempChannel.Open(string.Empty);

            var levelService = new WaterLevelService(tempChannel, 1000);
            tempChannel.Services.Add(levelService);
            Assert.IsNotNull(levelService);
            levelService.Enabled = true;
            Thread.Sleep(2000);
            Assert.AreEqual(30.2222f, level);

            Assert.AreEqual("WaterLevel", levelService.Name);
            Assert.AreEqual("watlev", levelService.SensorName);

            tempChannel.Close();
        }

        [Test]
        public void Test_RedoxService()
        {
            float level = 0.0f;

            var tempChannel = new TestTempChannel();
            tempChannel.ReceivedData += delegate (object sender, DataReceivedEventArgs e) {
                level = e.Value;
            };
            Assert.IsNotNull(tempChannel);
            tempChannel.Open(string.Empty);

            var levelService = new RedoxService(tempChannel, 1000);
            tempChannel.Services.Add(levelService);
            Assert.IsNotNull(levelService);
            levelService.Enabled = true;
            Thread.Sleep(2000);
            Assert.AreEqual(11.5555f, level);

            Assert.AreEqual("Redox", levelService.Name);
            Assert.AreEqual("redox", levelService.SensorName);

            tempChannel.Close();
        }

        [Test]
        public void Test_PHService()
        {
            float level = 0.0f;

            var tempChannel = new TestTempChannel();
            tempChannel.ReceivedData += delegate (object sender, DataReceivedEventArgs e) {
                level = e.Value;
            };
            Assert.IsNotNull(tempChannel);
            tempChannel.Open(string.Empty);

            var levelService = new PHService(tempChannel, 1000);
            tempChannel.Services.Add(levelService);
            Assert.IsNotNull(levelService);
            levelService.Enabled = true;
            Thread.Sleep(2000);
            Assert.AreEqual(7.3333f, level);

            Assert.AreEqual("pH", levelService.Name);
            Assert.AreEqual("ph", levelService.SensorName);

            tempChannel.Close();
        }

        [Test]
        public void Test_LEDService()
        {
            var tempChannel = new TestTempChannel();
            Assert.IsNotNull(tempChannel);
            tempChannel.Open(string.Empty);

            var ledService = new LEDService(tempChannel, 100);
            Assert.IsNotNull(ledService);
            ledService.Enabled = true;
            Thread.Sleep(2000);

            tempChannel.Close();
        }
    }
}
