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
