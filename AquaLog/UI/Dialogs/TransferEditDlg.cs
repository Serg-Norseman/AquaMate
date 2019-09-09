﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Model;
using AquaLog.Core.Types;

namespace AquaLog.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TransferEditDlg : Form, IEditDialog<Transfer>
    {
        private ALModel fModel;
        private Transfer fRecord;


        public ALModel Model
        {
            get { return fModel; }
            set { fModel = value; }
        }

        public Transfer Record
        {
            get { return fRecord; }
            set {
                if (fRecord != value) {
                    fRecord = value;
                    UpdateView();
                }
            }
        }


        public TransferEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            SetLocale();
        }

        public void SetLocale()
        {
            Text = Localizer.LS(LSID.Transfer);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            cmbType.Items.Clear();
            cmbType.Sorted = true;
            for (TransferType type = TransferType.Relocation; type <= TransferType.Death; type++) {
                cmbType.AddItem<TransferType>(Localizer.LS(ALData.TransferTypes[(int)type]), type);
            }

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

        private void UpdateView()
        {
            if (fRecord != null) {
                string itName = fModel.GetRecordName(fRecord.ItemType, fRecord.ItemId);
                txtName.Text = itName;

                if (fRecord.Id == 0) {
                    IList<Transfer> lastTransfers = fModel.QueryLastTransfers(fRecord.ItemId, (int)fRecord.ItemType);
                    if (lastTransfers.Count > 0) {
                        fRecord.SourceId = lastTransfers[0].TargetId;
                    }
                }

                cmbTarget.Items.Clear();
                var aquariums = fModel.QueryAquariums();
                foreach (var aqm in aquariums) {
                    cmbSource.Items.Add(aqm);

                    if (aqm.Id != fRecord.SourceId) {
                        cmbTarget.Items.Add(aqm);
                    }
                }

                cmbSource.SelectedItem = aquariums.FirstOrDefault(aqm => aqm.Id == fRecord.SourceId);
                cmbTarget.SelectedItem = aquariums.FirstOrDefault(aqm => aqm.Id == fRecord.TargetId);

                if (!fRecord.Date.Equals(ALCore.ZeroDate)) {
                    dtpDate.Value = fRecord.Date;
                }

                cmbType.SetSelectedTag(fRecord.Type);
                txtCause.Text = fRecord.Cause;

                cmbShop.Items.Clear();
                var shops = fModel.QueryShops();
                foreach (var shp in shops) {
                    if (!string.IsNullOrEmpty(shp.element)) {
                        cmbShop.Items.Add(shp.element);
                    }
                }

                txtQty.Text = fRecord.Quantity.ToString();
                if (fRecord.Type == TransferType.Purchase || fRecord.Type == TransferType.Sale) {
                    txtUnitPrice.Text = ALCore.GetDecimalStr(fRecord.UnitPrice);
                    cmbShop.Text = fRecord.Shop;
                }
            }
        }

        private void ApplyChanges()
        {
            Aquarium aqm = cmbSource.SelectedItem as Aquarium;
            fRecord.SourceId = (aqm == null) ? 0 : aqm.Id;

            aqm = cmbTarget.SelectedItem as Aquarium;
            fRecord.TargetId = (aqm == null) ? 0 : aqm.Id;

            //fTransfer.ItemId = fInhabitant.Id;
            //fTransfer.ItemType = ALCore.GetItemType(fInhabitant.GetSpeciesType());
            fRecord.Date = dtpDate.Value;
            fRecord.Type = cmbType.GetSelectedTag<TransferType>();
            fRecord.Cause = txtCause.Text;

            fRecord.Quantity = int.Parse(txtQty.Text);
            if (fRecord.Type == TransferType.Purchase || fRecord.Type == TransferType.Sale) {
                fRecord.UnitPrice = (float)ALCore.GetDecimalVal(txtUnitPrice.Text);
                fRecord.Shop = cmbShop.Text;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try {
                ApplyChanges();
                DialogResult = DialogResult.OK;
            } catch {
                DialogResult = DialogResult.None;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var transferType = cmbType.GetSelectedTag<TransferType>();

            bool ps = transferType == TransferType.Purchase || transferType == TransferType.Sale;
            txtUnitPrice.Enabled = ps;
            cmbShop.Enabled = ps;
            if (ps) {
                txtUnitPrice.Text = ALCore.GetDecimalStr(fRecord.UnitPrice);
                cmbShop.Text = fRecord.Shop;
            } else {
                txtUnitPrice.Text = "";
                cmbShop.Text = "";
            }
        }
    }
}