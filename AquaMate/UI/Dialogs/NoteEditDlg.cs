/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NoteEditDlg : EditDialog, INoteEditorView
    {
        private readonly NoteEditorPresenter fPresenter;

        public NoteEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new NoteEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Note);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblAquarium.Text = Localizer.LS(LSID.Aquarium);
            lblDate.Text = Localizer.LS(LSID.Date);
            lblEvent.Text = Localizer.LS(LSID.Event);
            lblNote.Text = Localizer.LS(LSID.Note);
        }

        public void SetContext(IModel model, Note record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        IComboBox INoteEditorView.AquariumCombo
        {
            get { return GetControlHandler<IComboBox>(cmbAquarium); }
        }

        IDateTimeBox INoteEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpDateTime); }
        }

        ITextBox INoteEditorView.EventField
        {
            get { return GetControlHandler<ITextBox>(txtEvent); }
        }

        ITextBox INoteEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion
    }
}
