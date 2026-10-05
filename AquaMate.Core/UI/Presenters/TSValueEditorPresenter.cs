/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.Logging;
using AquaMate.TSDB;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface ITSValueEditorView : IEditorView<TSValue>
    {
        IDateTimeBox TimestampField { get; }
        ITextBox ValueField { get; }
    }


    public class TSValueEditorPresenter : EditorPresenter<IModel, TSValue, ITSValueEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TSValueEditorPresenter");


        public TSValueEditorPresenter(ITSValueEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                if (!ALCore.IsZeroDate(fRecord.Timestamp)) {
                    fView.TimestampField.Value = fRecord.Timestamp;
                }
                fView.ValueField.SetDecimalVal(fRecord.Value);
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Timestamp = fView.TimestampField.Value;
                fRecord.Value = fView.ValueField.GetDecimalVal();

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
