﻿/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Linq;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class InhabitantEditDlg : EditDialog<Inhabitant>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "InhabitantEditDlg");

        public InhabitantEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            SetLocale();
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Inhabitant);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            cmbSex.FillCombo<Sex>(ALData.SexNames, false);

            lblName.Text = Localizer.LS(LSID.Name);
            lblNote.Text = Localizer.LS(LSID.Note);
            lblSpecies.Text = Localizer.LS(LSID.SpeciesS);
            lblSex.Text = Localizer.LS(LSID.Sex);
            lblState.Text = Localizer.LS(LSID.State);
        }

        protected override void UpdateView()
        {
            var speciesList = fModel.QuerySpecies();
            var species = speciesList.FirstOrDefault(sp => sp.Id == fRecord.SpeciesId);
            UIHelper.FillEntitiesCombo(cmbSpecies, speciesList, fRecord.SpeciesId, true);

            txtName.Text = fRecord.Name;
            txtNote.Text = fRecord.Note;
            cmbSex.SetSelectedTag(fRecord.Sex);

            ItemType itemType;

            if (species != null) {
                itemType = ALCore.GetItemType(species.Type);
                ItemState itemState;
                DateTime exclusionDate;
                fModel.GetItemState(fRecord.Id, itemType, out itemState, out exclusionDate);
                bool noTransferState = (itemState == ItemState.Unknown);
                cmbState.Enabled = noTransferState;

                if (!noTransferState) {
                    UIHelper.FillItemStatesCombo(cmbState, itemType, fRecord.State);
                }
            } else {
                itemType = ItemType.None;
                cmbState.Items.Clear();
            }

            imgViewer.SetRecord(fModel, fRecord.Id, itemType);
        }

        protected override void ApplyChanges()
        {
            Species spc = cmbSpecies.SelectedItem as Species;
            if (spc != null) {
                fRecord.SpeciesId = spc.Id;
            }

            fRecord.Name = txtName.Text;
            fRecord.Note = txtNote.Text;
            fRecord.Sex = cmbSex.GetSelectedTag<Sex>();
            fRecord.State = cmbState.GetSelectedTag<ItemState>();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try {
                ApplyChanges();
                DialogResult = DialogResult.OK;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                DialogResult = DialogResult.None;
            }
        }

        private void cmbSpecies_SelectedIndexChanged(object sender, EventArgs e)
        {
            var species = cmbSpecies.SelectedItem as Species;
            bool itemIsNull = (species == null);

            if (!itemIsNull) {
                UIHelper.FillItemStatesCombo(cmbState, ALCore.GetItemType(species.Type), fRecord.State);
            }

            bool hasSex = (!itemIsNull && ALCore.IsAnimal(species.Type));
            lblSex.Visible = hasSex;
            cmbSex.Visible = hasSex;
        }
    }
}
