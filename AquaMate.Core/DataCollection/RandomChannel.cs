/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
