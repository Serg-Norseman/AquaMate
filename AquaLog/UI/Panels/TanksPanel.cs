﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Export;
using AquaLog.Core.Model;
using AquaLog.Core.Types;
using AquaLog.UI.Dialogs;

namespace AquaLog.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TanksPanel : DataPanel
    {
        private readonly ContextMenu fContextMenu;
        private readonly MenuItem fEditItem;
        private readonly MenuItem fDeleteItem;
        private readonly FlowLayoutPanel fLayoutPanel;

        private TankSticker fSelectedTank;


        public TankSticker SelectedTank
        {
            get { return fSelectedTank; }
        }


        public TanksPanel() : base()
        {
            fLayoutPanel = new FlowLayoutPanel();
            fLayoutPanel.Dock = DockStyle.Fill;
            fLayoutPanel.Padding = new Padding(10);
            Controls.Add(fLayoutPanel);

            fEditItem = new MenuItem();
            fEditItem.Click += btnEditTank_Click;

            fDeleteItem = new MenuItem();
            fDeleteItem.Click += btnDeleteTank_Click;

            fContextMenu = new ContextMenu();
            fContextMenu.MenuItems.AddRange(new MenuItem[] { fEditItem, fDeleteItem});
        }

        public override void SetLocale()
        {
            fEditItem.Text = Localizer.LS(LSID.Edit);
            fDeleteItem.Text = Localizer.LS(LSID.Delete);
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", btnAddTank_Click);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", btnEditTank_Click);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", btnDeleteTank_Click);
            AddAction("Transfer", LSID.Transfer, null, TransferHandler);
            AddAction("LogBook", LSID.LogBook, "", btnLogBook_Click);
            AddAction("M3DViewer", LSID.M3DViewer, "", btnM3DViewer_Click);
        }

        public override void UpdateContent()
        {
            fLayoutPanel.Controls.Clear();
            if (fModel == null) return;

            var aquariums = fModel.QueryAquariums();

            foreach (var aqm in aquariums) {
                if (aqm.IsInactive() && ALSettings.Instance.HideClosedTanks) {
                    continue;
                }

                var aqPanel = new TankSticker();
                aqPanel.Model = fModel;
                aqPanel.Aquarium = aqm;
                aqPanel.Click += OnTankClick;
                aqPanel.DoubleClick += OnTankDoubleClick;
                aqPanel.ContextMenu = fContextMenu;
                fLayoutPanel.Controls.Add(aqPanel);
            }
        }

        private void OnTankClick(object sender, EventArgs e)
        {
            if (fSelectedTank != null) {
                fSelectedTank.Selected = false;
            }
            fSelectedTank = sender as TankSticker;
            if (fSelectedTank != null) {
                fSelectedTank.Selected = true;
            }
        }

        private void OnTankDoubleClick(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;

            Browser.SetView(MainView.AquariumDetails, record);
        }

        private void btnAddTank_Click(object sender, EventArgs e)
        {
            var aqm = new Aquarium();
            aqm.Name = ALCore.UnknownName;

            using (var dlg = new AquariumEditDlg()) {
                dlg.Record = aqm;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.AddRecord(aqm);
                    UpdateContent();
                }
            }
        }

        private void btnEditTank_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var aqm = selectedItem.Aquarium;
            if (aqm == null) return;

            using (var dlg = new AquariumEditDlg()) {
                dlg.Record = aqm;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.UpdateRecord(aqm);
                    UpdateContent();
                }
            }
        }

        private void btnDeleteTank_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;

            if (!UIHelper.ShowQuestionYN(string.Format(Localizer.LS(LSID.RecordDeleteQuery), record.ToString()))) return;

            fModel.DeleteRecord(record);
            UpdateContent();
        }

        private void btnLogBook_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;

            string fileName = UIHelper.GetSaveFile("RTF files (*.rtf)|*.rtf");
            if (string.IsNullOrEmpty(fileName)) return;

            RTFLogBook.Generate(fModel, record, fileName);

            ALCore.LoadExtFile(fileName);
        }

        private void btnM3DViewer_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;
            Browser.SetView(MainView.M3DViewer, record.Tank);
        }

        private void TransferHandler(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;

            var transfer = new Transfer();
            transfer.ItemType = ItemType.Aquarium;
            transfer.ItemId = record.Id;

            using (var dlg = new TransferEditDlg()) {
                dlg.Model = fModel;
                dlg.Record = transfer;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.AddRecord(dlg.Record);
                    UpdateContent();
                }
            }
        }
    }
}
