/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

#if !NETCOREAPP30

using System;
using System.IO.Ports;
using AquaMate.Core;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.DataCollection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SerialChannel : BaseChannel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "SerialChannel");


        private SerialPort fPort;


        public override bool IsConnected
        {
            get { return (fPort != null) && fPort.IsOpen; }
        }


        public SerialChannel() : base()
        {
        }

        protected override void OpenMethod()
        {
            try {
                fPort = new SerialPort(fParameters, 115200, Parity.None, 8, StopBits.One);
                // This option ensures the arduino restarts when we run the program
                fPort.DtrEnable = true;
                fPort.ReadTimeout = 500;
                fPort.WriteTimeout = 500;
                fPort.DataReceived += DataReceived;
                fPort.Open();
            } catch (Exception ex) {
                fLogger.WriteError("OpenMethod(): ", ex);
            }
        }

        public override void Open(string parameters)
        {
            try {
                StartOpenThread(parameters);

                base.Open(parameters);
            } catch (Exception ex) {
                fLogger.WriteError("Open(): ", ex);
            }
        }

        public override void Close()
        {
            base.Close();

            fPort.DtrEnable = false;
            fPort.Close();
            fPort = null;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string response = sp.ReadLine();
            ReceiveData(response);
        }

        public override void Send(string text)
        {
            if (IsConnected) {
                fPort.Write(text);
            }
        }
    }
}

#endif
