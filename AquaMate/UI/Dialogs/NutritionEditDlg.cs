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
    public partial class NutritionEditDlg : EditDialog, INutritionEditorView
    {
        private readonly NutritionEditorPresenter fPresenter;

        public NutritionEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new NutritionEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Nutrition);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            tabCommon.Text = Localizer.LS(LSID.Common);
            tabTransfers.Text = Localizer.LS(LSID.Transfers);

            lblName.Text = Localizer.LS(LSID.Name);
            lblBrand.Text = Localizer.LS(LSID.Brand);
            lblAmount.Text = Localizer.LS(LSID.Amount);
            lblNote.Text = Localizer.LS(LSID.Note);
            lblState.Text = Localizer.LS(LSID.State);
        }

        public void SetContext(IModel model, Nutrition record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 1) {
                var lv = GetControlHandler<IListView>(lvTransfers);
                ModelPresenter.FillTransfersLVPreview(lv, fPresenter.Model, fPresenter.Record, false);
            }
        }

        #region View interface implementation

        ITextBox INutritionEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IComboBox INutritionEditorView.BrandCombo
        {
            get { return GetControlHandler<IComboBox>(cmbBrand); }
        }

        ITextBox INutritionEditorView.AmountField
        {
            get { return GetControlHandler<ITextBox>(txtAmount); }
        }

        ITextBox INutritionEditorView.NoteField
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        IComboBox INutritionEditorView.StateCombo
        {
            get { return GetControlHandler<IComboBox>(cmbState); }
        }

        #endregion
    }
}
