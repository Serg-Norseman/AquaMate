/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
