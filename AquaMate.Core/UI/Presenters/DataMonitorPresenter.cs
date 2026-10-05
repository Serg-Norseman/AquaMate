/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using AquaMate.Core;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.UI
{
    public interface IDataMonitorView : IView
    {
    }


    public class DataMonitorPresenter : ViewerPresenter<IModel, IDataMonitorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "DataMonitorPresenter");


        public DataMonitorPresenter(IDataMonitorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
        }
    }
}
