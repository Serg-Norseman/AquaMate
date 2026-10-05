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
    public partial class BrandEditDlg : EditDialog, IBrandEditorView
    {
        private readonly BrandEditorPresenter fPresenter;

        public BrandEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new BrandEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Brand);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Name);
            lblCountry.Text = Localizer.LS(LSID.Country);
            lblWebSite.Text = Localizer.LS(LSID.WebSite);
            lblEmail.Text = Localizer.LS(LSID.Email);
            lblNote.Text = Localizer.LS(LSID.Note);
        }

        public void SetContext(IModel model, Brand record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        ITextBox IBrandEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IComboBox IBrandEditorView.CountryCombo
        {
            get { return GetControlHandler<IComboBox>(cmbCountry); }
        }

        ITextBox IBrandEditorView.WebSiteField
        {
            get { return GetControlHandler<ITextBox>(txtWebSite); }
        }

        ITextBox IBrandEditorView.EmailField
        {
            get { return GetControlHandler<ITextBox>(txtEmail); }
        }

        ITextBox IBrandEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion
    }
}
