/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace AquaMate.DataCollection
{
    public class DataReceivedEventArgs : EventArgs
    {
        private readonly string fSensorId;
        private readonly string fSensorName;
        private readonly float fValue;


        public string SensorId
        {
            get { return fSensorId; }
        }

        public string SensorName
        {
            get { return fSensorName; }
        }

        public float Value
        {
            get { return fValue; }
        }


        internal DataReceivedEventArgs(string sensorId, string sensorName, float value)
        {
            fSensorId = sensorId;
            fSensorName = sensorName;
            fValue = value;
        }
    }
}
