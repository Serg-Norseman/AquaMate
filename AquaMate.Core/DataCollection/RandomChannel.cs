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
    /// <summary>
    /// 
    /// </summary>
    public sealed class RandomChannel : BaseChannel
    {
        private readonly Random fRandom;


        public override bool IsConnected
        {
            get { return true; }
        }


        public RandomChannel()
        {
            fRandom = new Random();
        }

        public override void Send(string text)
        {
            if (text == "Q:temp;2") {
                // temperature query & response
                float val = 20.0f + fRandom.Next(1000) / 100.0f;
                string response = string.Format("R:temp;sid:0000000000000000;val:{0};", val);
                ReceiveData(response);
            }
        }
    }
}
