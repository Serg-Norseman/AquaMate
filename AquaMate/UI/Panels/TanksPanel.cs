/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Export;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TanksPanel : DataPanel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TanksPanel");

        private readonly ContextMenuStrip fContextMenu;
        private readonly ToolStripMenuItem fEditItem;
        private readonly ToolStripMenuItem fDeleteItem;
        private readonly FlowLayoutPanel fLayoutPanel;

        private TankSticker fSelectedTank;


        public TankSticker SelectedTank
        {
            get { return fSelectedTank; }
            set {
                if (fSelectedTank != null) {
                    fSelectedTank.Selected = false;
                }

                fSelectedTank = value;

                if (fSelectedTank != null) {
                    fSelectedTank.Selected = true;

                    SelectionChanged(new List<Entity>() { fSelectedTank.Aquarium });
                } else {
                    SelectionChanged(new List<Entity>() { });
                }
            }
        }


        public TanksPanel() : base()
        {
            Click += OnPanelClick;

            fLayoutPanel = new FlowLayoutPanel();
            fLayoutPanel.Dock = DockStyle.Fill;
            fLayoutPanel.Padding = new Padding(10);
            fLayoutPanel.Click += OnPanelClick;
            Controls.Add(fLayoutPanel);

            fEditItem = new ToolStripMenuItem();
            fEditItem.Click += btnEditTank_Click;

            fDeleteItem = new ToolStripMenuItem();
            fDeleteItem.Click += btnDeleteTank_Click;

            fContextMenu = new ContextMenuStrip();
            fContextMenu.Items.AddRange(new ToolStripMenuItem[] { fEditItem, fDeleteItem});
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
            AddAction("Analysis", LSID.Analysis, "", btnAnalysis_Click);
            AddAction("Quality", LSID.Quality, "", btnQuality_Click);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count > 0);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);
            SetActionEnabled("Transfer", enabled);
            SetActionEnabled("LogBook", enabled);
            SetActionEnabled("M3DViewer", enabled);
            SetActionEnabled("Analysis", enabled);
            SetActionEnabled("Quality", enabled);
            SetActionEnabled("Radar", enabled);
        }

        public override void UpdateContent()
        {
            fLayoutPanel.Controls.Clear();
            if (fModel == null) return;

            var aquariums = fModel.QueryAquariums();

            foreach (var aqm in aquariums) {
                var workTime = fModel.GetWorkTime(aqm);
                if (!workTime.IsActive() && ALSettings.Instance.HideClosedTanks) {
                    continue;
                }

                var aqPanel = new TankSticker();
                aqPanel.Model = fModel;
                aqPanel.Aquarium = aqm;
                aqPanel.Click += OnTankClick;
                aqPanel.DoubleClick += OnTankDoubleClick;
                aqPanel.ContextMenuStrip = fContextMenu;
                fLayoutPanel.Controls.Add(aqPanel);
            }
        }

        private void OnTankClick(object sender, EventArgs e)
        {
            SelectedTank = sender as TankSticker;
        }

        private void OnPanelClick(object sender, EventArgs e)
        {
            SelectedTank = null;
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

            using (var dlg = AppHost.ResolveDialog<IAquariumEditorView>()) {
                dlg.SetContext(fModel, aqm);

                if (dlg.ShowModal()) {
                    fModel.AddRecord(aqm);
                    fModel.Cache.Put(ItemType.Aquarium, aqm.Id, aqm);
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

            using (var dlg = AppHost.ResolveDialog<IAquariumEditorView>()) {
                dlg.SetContext(fModel, aqm);

                if (dlg.ShowModal()) {
                    fModel.UpdateRecord(aqm);
                    fModel.Cache.Put(ItemType.Aquarium, aqm.Id, aqm);
                    UpdateContent();
                }
            }
        }

        private void btnDeleteTank_Click(object sender, EventArgs e)
        {
            try {
                var selectedItem = SelectedTank;
                if (selectedItem == null) return;

                var record = selectedItem.Aquarium;

                if (!Browser.CheckDelete(record)) return;

                fModel.Cache.Remove(ItemType.Aquarium, record.Id);
                fModel.DeleteRecord(record);
                UpdateContent();
            } catch (Exception ex) {
                fLogger.WriteError("btnDeleteTank_Click()", ex);
            }
        }

        private void btnLogBook_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;

            string fileName = UIHelper.GetSaveFile("RTF files (*.rtf)|*.rtf");
            if (string.IsNullOrEmpty(fileName)) return;

            RTFLogBook.Generate(fModel, record, fileName);

            AppHost.LoadExtFile(fileName);
        }

        private void btnM3DViewer_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;
            Browser.SetView(MainView.M3DViewer, record);
        }

        private void TransferHandler(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;
            ItemType itemType = ItemType.Aquarium;
            Browser.TransferItem(itemType, record.Id, this);
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;
            Browser.SetView(MainView.Analysis, record);
        }

        private void btnQuality_Click(object sender, EventArgs e)
        {
            var selectedItem = SelectedTank;
            if (selectedItem == null) return;

            var record = selectedItem.Aquarium;
            Browser.SetView(MainView.Quality, record);
        }
    }
}
