/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface INoteEditorView : IEditorView<Note>
    {
        IComboBox AquariumCombo { get; }
        IDateTimeBox TimestampField { get; }
        ITextBox EventField { get; }
        ITextBox NoteField { get; }
    }


    public class NoteEditorPresenter : EditorPresenter<IModel, Note, INoteEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "NoteEditorPresenter");


        public NoteEditorPresenter(INoteEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.AquariumCombo.AddRange(fModel.QueryAquariumsList(!fRecord.IsNewRecord));
                fView.AquariumCombo.SetSelectedTag(fRecord.AquariumId);

                if (!ALCore.IsZeroDate(fRecord.Timestamp)) {
                    fView.TimestampField.Value = fRecord.Timestamp;
                }

                fView.EventField.Text = fRecord.Event;
                fView.NoteField.Text = fRecord.Content;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.AquariumId = fView.AquariumCombo.GetSelectedTag<int>();
                fRecord.Timestamp = fView.TimestampField.Value;
                fRecord.Event = fView.EventField.Text;
                fRecord.Content = fView.NoteField.Text;

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
