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
    public partial class TransferEditDlg : EditDialog, ITransferEditorView
    {
        private readonly TransferEditorPresenter fPresenter;

        public TransferEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new TransferEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Transfer);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Item);
            lblSource.Text = Localizer.LS(LSID.SourceTank);
            lblTarget.Text = Localizer.LS(LSID.TargetTank);
            lblDate.Text = Localizer.LS(LSID.Date);
            lblType.Text = Localizer.LS(LSID.Type);
            lblCause.Text = Localizer.LS(LSID.Cause);
            lblQty.Text = Localizer.LS(LSID.Quantity);
            lblUnitPrice.Text = Localizer.LS(LSID.UnitPrice);
            lblShop.Text = Localizer.LS(LSID.Shop);
        }

        public void SetContext(IModel model, Transfer record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fPresenter.ChangeSelectedType();
        }

        private void cmbAquarium_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) {
                var comboBox = sender as ComboBox;
                if (comboBox != null) {
                    comboBox.SelectedItem = null;
                }
            }
        }

        #region View interface implementation

        ITextBox ITransferEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IComboBox ITransferEditorView.SourceCombo
        {
            get { return GetControlHandler<IComboBox>(cmbSource); }
        }

        IComboBox ITransferEditorView.TargetCombo
        {
            get { return GetControlHandler<IComboBox>(cmbTarget); }
        }

        IDateTimeBox ITransferEditorView.DateField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpDate); }
        }

        IComboBox ITransferEditorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbType); }
        }

        ITextBox ITransferEditorView.CauseField
        {
            get { return GetControlHandler<ITextBox>(txtCause); }
        }

        ITextBox ITransferEditorView.QuantityField
        {
            get { return GetControlHandler<ITextBox>(txtQty); }
        }

        ITextBox ITransferEditorView.UnitPriceField
        {
            get { return GetControlHandler<ITextBox>(txtUnitPrice); }
        }

        IComboBox ITransferEditorView.ShopCombo
        {
            get { return GetControlHandler<IComboBox>(cmbShop); }
        }

        #endregion
    }
}
