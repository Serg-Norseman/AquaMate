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
    public partial class ShopEditDlg : EditDialog, IShopEditorView
    {
        private readonly ShopEditorPresenter fPresenter;

        public ShopEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new ShopEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Shop);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Name);
            lblAddress.Text = Localizer.LS(LSID.Address);
            lblTelephone.Text = Localizer.LS(LSID.Telephone);
            lblWebSite.Text = Localizer.LS(LSID.WebSite);
            lblEmail.Text = Localizer.LS(LSID.Email);
            lblNote.Text = Localizer.LS(LSID.Note);
        }

        public void SetContext(IModel model, Shop record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        ITextBox IShopEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        ITextBox IShopEditorView.AddressField
        {
            get { return GetControlHandler<ITextBox>(txtAddress); }
        }

        ITextBox IShopEditorView.TelephoneField
        {
            get { return GetControlHandler<ITextBox>(txtTelephone); }
        }

        ITextBox IShopEditorView.WebSiteField
        {
            get { return GetControlHandler<ITextBox>(txtWebSite); }
        }

        ITextBox IShopEditorView.EmailField
        {
            get { return GetControlHandler<ITextBox>(txtEmail); }
        }

        ITextBox IShopEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion
    }
}
