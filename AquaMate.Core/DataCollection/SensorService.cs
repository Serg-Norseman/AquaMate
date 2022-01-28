/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;

namespace AquaMate.DataCollection
{
    public class SensorService : BaseService
    {
        protected SensorService(IChannel channel, double interval) : base(channel, interval)
        {
        }

        protected void WriteQuery(string sensor)
        {
            // pin = 2, "Q:temp;2"
            string query = string.Format("Q:{0};2", sensor);
            Channel.Send(query);
        }

        protected internal override DataReceivedEventArgs TryReadResponse(string response)
        {
            if (!string.IsNullOrEmpty(response)) {
                // "R:temp;sid:" + rom + ";val:" + celsius + ";"
                string[] parts = response.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 6 && parts[0] == "R" && parts[1] == SensorName) {
                    string sid;
                    float value;
                    if (parts[2] == "sid" && parts[4] == "val") {
                        sid = parts[3];
                        value = (float)ALCore.GetDecimalVal(parts[5]);
                        return new DataReceivedEventArgs(sid, SensorName, value);
                    }
                }
            }
            return null;
        }

        protected override void OnTimedEvent()
        {
            if (Channel.IsConnected) {
                WriteQuery(SensorName);
            }
        }
    }
}
