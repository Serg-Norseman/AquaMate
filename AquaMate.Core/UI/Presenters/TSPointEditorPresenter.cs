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
    public interface ITSPointEditorView : IEditorView<TSPoint>
    {
        ITextBox NameField { get; }
        ITextBox MeasureUnitField { get; }
        ITextBox MinField { get; }
        ITextBox MaxField { get; }
        ITextBox DeviationField { get; }
        ITextBox SIDField { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TSPointEditorPresenter : EditorPresenter<IModel, TSPoint, ITSPointEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TSPointEditorPresenter");


        public TSPointEditorPresenter(ITSPointEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.NameField.Text = fRecord.Name;
                fView.MeasureUnitField.Text = fRecord.MeasureUnit;
                fView.MinField.SetDecimalVal(fRecord.Min);
                fView.MaxField.SetDecimalVal(fRecord.Max);
                fView.DeviationField.SetDecimalVal(fRecord.Deviation);
                fView.SIDField.Text = fRecord.SID;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.MeasureUnit = fView.MeasureUnitField.Text;
                fRecord.Min = fView.MinField.GetDecimalVal();
                fRecord.Max = fView.MaxField.GetDecimalVal();
                fRecord.Deviation = fView.DeviationField.GetDecimalVal();
                fRecord.SID = fView.SIDField.Text;

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
