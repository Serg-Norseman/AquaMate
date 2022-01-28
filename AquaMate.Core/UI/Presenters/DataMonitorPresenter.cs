/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.UI
{
    public interface IDataMonitorView : IView
    {
    }


    /// <summary>
    /// 
    /// </summary>
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
