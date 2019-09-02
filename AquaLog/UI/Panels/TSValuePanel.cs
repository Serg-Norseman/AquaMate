﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.TSDB;
using AquaLog.UI;

namespace AquaLog.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class TSValuePanel : ListPanel
    {
        private int fPointId;

        public int PointId
        {
            get { return fPointId; }
            set { fPointId = value; }
        }


        public TSValuePanel()
        {
        }

        public override void SetExtData(object extData)
        {
            int pointId = (int)extData;
            fPointId = pointId;
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
        }

        public override void UpdateContent()
        {
            ListView.Clear();
            ListView.Columns.Add(Localizer.LS(LSID.Timestamp), 140, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Value), 120, HorizontalAlignment.Right);

            TSDatabase tsdb = fModel.TSDB;
            var records = tsdb.QueryValues(fPointId, DateTime.Now.AddDays(-60), DateTime.Now);
            foreach (TSValue rec in records) {
                var item = new ListViewItem(rec.Timestamp.ToString());
                item.Tag = rec;
                item.SubItems.Add(ALCore.GetDecimalStr(rec.Value));
                ListView.Items.Add(item);
            }
        }

        protected override void AddHandler(object sender, EventArgs e)
        {
            TSValue record = new TSValue();

            using (var dlg = new TSValueEditDlg()) {
                dlg.Value = record;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.TSDB.InsertValue(fPointId, record.Timestamp, record.Value);
                    UpdateContent();
                }
            }
        }

        protected override void EditHandler(object sender, EventArgs e)
        {
            var record = ALCore.GetSelectedTag<TSValue>(ListView);
            if (record == null) return;

            using (var dlg = new TSValueEditDlg()) {
                dlg.Value = record;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    fModel.TSDB.UpdateValue(fPointId, record.Timestamp, record.Value);
                    UpdateContent();
                }
            }
        }

        protected override void DeleteHandler(object sender, EventArgs e)
        {
            var record = ALCore.GetSelectedTag<TSValue>(ListView);
            if (record == null) return;

            fModel.TSDB.DeleteValue(fPointId, record.Timestamp);
            UpdateContent();
        }
    }
}
