/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;

namespace AquaMate.DataCollection
{
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);


    /// <summary>
    /// The interface of data channels.
    /// </summary>
    public interface IChannel : IDisposable
    {
        bool IsConnected { get; }
        List<BaseService> Services { get; }

        event DataReceivedEventHandler ReceivedData;

        void Close();
        void Open(string parameters);
        void Send(string text);
    }
}
