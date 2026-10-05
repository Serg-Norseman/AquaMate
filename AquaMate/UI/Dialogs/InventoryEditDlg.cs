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
    public partial class InventoryEditDlg : EditDialog, IInventoryEditorView
    {
        private readonly InventoryEditorPresenter fPresenter;

        public InventoryEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new InventoryEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Inventory);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            tabCommon.Text = Localizer.LS(LSID.Common);
            tabTransfers.Text = Localizer.LS(LSID.Transfers);

            lblName.Text = Localizer.LS(LSID.Name);
            lblBrand.Text = Localizer.LS(LSID.Brand);
            lblType.Text = Localizer.LS(LSID.Type);
            lblNote.Text = Localizer.LS(LSID.Note);
            lblState.Text = Localizer.LS(LSID.State);
        }

        public void SetContext(IModel model, Inventory record)
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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 1) {
                var lv = GetControlHandler<IListView>(lvTransfers);
                ModelPresenter.FillTransfersLVPreview(lv, fPresenter.Model, fPresenter.Record, false);
            }
        }

        #region View interface implementation

        ITextBox IInventoryEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IComboBox IInventoryEditorView.BrandCombo
        {
            get { return GetControlHandler<IComboBox>(cmbBrand); }
        }

        IComboBox IInventoryEditorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbType); }
        }

        ITextBox IInventoryEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        IComboBox IInventoryEditorView.StateCombo
        {
            get { return GetControlHandler<IComboBox>(cmbState); }
        }

        IPropertyGrid IInventoryEditorView.PropsGrid
        {
            get { return GetControlHandler<IPropertyGrid>(pgProps); }
        }

        #endregion
    }
}
