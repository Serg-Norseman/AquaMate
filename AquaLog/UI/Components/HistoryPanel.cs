﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Model;
using AquaLog.UI;

namespace AquaLog.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class HistoryPanel : ListPanel
    {
        public HistoryPanel()
        {
            ListView.Columns.Add("Aquarium", 120, HorizontalAlignment.Left);
            ListView.Columns.Add("DateTime", 120, HorizontalAlignment.Left);
            ListView.Columns.Add("Event", 100, HorizontalAlignment.Left);
            ListView.Columns.Add("Note", 250, HorizontalAlignment.Left);
        }

        public override void UpdateContent()
        {
            ListView.Items.Clear();
            if (fModel == null) return;

            var records = fModel.QueryHistory();

            foreach (History rec in records) {
                Aquarium aqm = fModel.GetRecord<Aquarium>(rec.AquariumId);
                string aqmName = (aqm == null) ? "" : aqm.Name;

                var item = new ListViewItem(aqmName);
                item.Tag = rec;
                item.SubItems.Add(rec.DateTime.ToString());
                item.SubItems.Add(rec.Event);
                item.SubItems.Add(rec.Note);
                ListView.Items.Add(item);
            }
        }

        protected override void InitActions()
        {
            fActions.Add(new Action("Add", "btn_rec_new.gif", AddHandler));
            fActions.Add(new Action("Edit", "btn_rec_edit.gif", EditHandler));
            fActions.Add(new Action("Delete", "btn_rec_delete.gif", DeleteHandler));
        }

        protected override void AddHandler(object sender, EventArgs e)
        {
            History record = new History();

            using (var dlg = new HistoryEditDlg()) {
                dlg.Model = fModel;
                dlg.History = record;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.AddRecord(record);
                    UpdateContent();
                }
            }
        }

        protected override void EditHandler(object sender, EventArgs e)
        {
            var selectedItem = ALCore.GetSelectedItem(ListView);
            if (selectedItem == null) return;

            var record = selectedItem.Tag as History;
            if (record == null) return;

            using (var dlg = new HistoryEditDlg()) {
                dlg.Model = fModel;
                dlg.History = record;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.UpdateRecord(record);
                    UpdateContent();
                }
            }
        }

        protected override void DeleteHandler(object sender, EventArgs e)
        {
            var selectedItem = ALCore.GetSelectedItem(ListView);
            if (selectedItem == null) return;

            fModel.DeleteRecord(selectedItem.Tag as History);
            UpdateContent();
        }
    }
}