/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
