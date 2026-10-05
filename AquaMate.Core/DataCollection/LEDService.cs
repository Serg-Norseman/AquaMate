/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.DataCollection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LEDService : BaseService
    {
        private bool fLED;


        public override string Name
        {
            get { return "LED"; }
        }


        public LEDService(IChannel channel, double interval) : base(channel, interval)
        {
        }

        protected override void OnTimedEvent()
        {
            if (Channel.IsConnected) {
                fLED = !fLED;

                if (fLED) {
                    Channel.Send("Q:setled;13;1");
                } else {
                    Channel.Send("Q:setled;13;0");
                }
            }
        }
    }
}
