/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IScheduleEditorView : IEditorView<Schedule>
    {
        IComboBox AquariumCombo { get; }
        IDateTimeBox TimestampField { get; }
        ITextBox EventField { get; }
        ICheckBox ReminderCheck { get; }
        IComboBox TypeCombo { get; }
        IComboBox StatusCombo { get; }
        ITextBox NoteField { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ScheduleEditorPresenter : EditorPresenter<IModel, Schedule, IScheduleEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ScheduleEditorPresenter");


        public ScheduleEditorPresenter(IScheduleEditorView view) : base(view)
        {
            var scheduleTypesList = ALData.GetNamesList<ScheduleType>(ALData.ScheduleTypes);
            fView.TypeCombo.AddRange(scheduleTypesList, false);

            var taskStatusesList = ALData.GetNamesList<TaskStatus>(ALData.TaskStatuses);
            fView.StatusCombo.AddRange(taskStatusesList, false);
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
                fView.ReminderCheck.Checked = fRecord.Reminder;
                fView.TypeCombo.SetSelectedTag(fRecord.Type);
                fView.StatusCombo.SetSelectedTag(fRecord.Status);
                fView.NoteField.Text = fRecord.Note;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.AquariumId = fView.AquariumCombo.GetSelectedTag<int>();
                fRecord.Timestamp = fView.TimestampField.Value;
                fRecord.Event = fView.EventField.Text;
                fRecord.Reminder = fView.ReminderCheck.Checked;
                fRecord.Type = fView.TypeCombo.GetSelectedTag<ScheduleType>();
                fRecord.Status = fView.StatusCombo.GetSelectedTag<TaskStatus>();
                fRecord.Note = fView.NoteField.Text;

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
