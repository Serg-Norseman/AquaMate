/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface ITankEditorView : IEditorView<ITank>
    {
        IPropertyGrid PropsGrid { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TankEditorPresenter : EditorPresenter<IModel, ITank, ITankEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TankEditorPresenter");


        public TankEditorPresenter(ITankEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fRecord.SetPropNames();
                fView.PropsGrid.SelectedObject = fRecord;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
