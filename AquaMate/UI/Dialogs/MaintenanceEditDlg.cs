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
    public partial class MaintenanceEditDlg : EditDialog, IMaintenanceEditorView
    {
        private readonly MaintenanceEditorPresenter fPresenter;

        public MaintenanceEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new MaintenanceEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Maintenance);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblAquarium.Text = Localizer.LS(LSID.Aquarium);
            lblDateTime.Text = Localizer.LS(LSID.Date);
            lblType.Text = Localizer.LS(LSID.Type);
            lblValue.Text = Localizer.LS(LSID.Value);
            lblNote.Text = Localizer.LS(LSID.Note);
        }

        public void SetContext(IModel model, Maintenance record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        IComboBox IMaintenanceEditorView.AquariumCombo
        {
            get { return GetControlHandler<IComboBox>(cmbAquarium); }
        }

        IDateTimeBox IMaintenanceEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpDateTime); }
        }

        IComboBox IMaintenanceEditorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbType); }
        }

        ITextBox IMaintenanceEditorView.ValueField
        {
            get { return GetControlHandler<ITextBox>(txtValue); }
        }

        ITextBox IMaintenanceEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion
    }
}
