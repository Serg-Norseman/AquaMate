/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    public partial class ScheduleEditDlg : EditDialog, IScheduleEditorView
    {
        private readonly ScheduleEditorPresenter fPresenter;

        public ScheduleEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new ScheduleEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.ScheduleS);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblAquarium.Text = Localizer.LS(LSID.Aquarium);
            lblDate.Text = Localizer.LS(LSID.Date);
            lblEvent.Text = Localizer.LS(LSID.Event);
            chkReminder.Text = Localizer.LS(LSID.Reminder);
            lblType.Text = Localizer.LS(LSID.Type);
            lblStatus.Text = Localizer.LS(LSID.Status);
            lblNote.Text = Localizer.LS(LSID.Note);
        }

        public void SetContext(IModel model, Schedule record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        IComboBox IScheduleEditorView.AquariumCombo
        {
            get { return GetControlHandler<IComboBox>(cmbAquarium); }
        }

        IDateTimeBox IScheduleEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpDateTime); }
        }

        ITextBox IScheduleEditorView.EventField
        {
            get { return GetControlHandler<ITextBox>(txtEvent); }
        }

        ICheckBox IScheduleEditorView.ReminderCheck
        {
            get { return GetControlHandler<ICheckBox>(chkReminder); }
        }

        IComboBox IScheduleEditorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbSchedule); }
        }

        IComboBox IScheduleEditorView.StatusCombo
        {
            get { return GetControlHandler<IComboBox>(cmbStatus); }
        }

        ITextBox IScheduleEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion
    }
}
