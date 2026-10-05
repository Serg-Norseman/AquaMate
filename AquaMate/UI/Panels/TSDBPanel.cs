/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using AquaMate.TSDB;
using AquaMate.UI.Dialogs;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TSDBPanel : ListPanel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TSDBPanel");

        public TSDBPanel()
        {
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
            AddAction("Data", LSID.Data, "", ViewDataHandler);
            AddAction("Trend", LSID.Trend, "", ViewTrendHandler);
            AddAction("DataMonitor", LSID.DataMonitor, "", ShowMonitor);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);

            SetActionEnabled("Data", enabled);
            SetActionEnabled("Trend", enabled);
        }

        protected override void UpdateListView()
        {
            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillTSPointsLV(lv, fModel);
        }

        protected override void AddHandler(object sender, EventArgs e)
        {
            var record = new TSPoint();

            using (var dlg = new TSPointEditDlg()) {
                dlg.SetContext(fModel, record);

                if (dlg.ShowModal()) {
                    fModel.TSDB.AddPoint(record);
                    UpdateContent();
                }
            }
        }

        protected override void EditHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<TSPoint>();
            if (record == null) return;

            using (var dlg = new TSPointEditDlg()) {
                dlg.SetContext(fModel, record);

                if (dlg.ShowModal()) {
                    fModel.TSDB.UpdatePoint(record);
                    UpdateContent();
                }
            }
        }

        protected override void DeleteHandler(object sender, EventArgs e)
        {
            try {
                var record = ListView.GetSelectedTag<TSPoint>();
                if (record == null) return;

                if (!Browser.CheckDelete(record)) return;

                fModel.TSDB.DeletePoint(record);
                UpdateContent();
            } catch (Exception ex) {
                fLogger.WriteError("DeleteHandler()", ex);
            }
        }

        private void ViewDataHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<TSPoint>();
            if (record == null) return;

            Browser.SetView(MainView.TSValues, record.Id);
        }

        private void ViewTrendHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<TSPoint>();
            if (record == null) return;

            Browser.SetView(MainView.TSTrend, record.Id);
        }

        private void ShowMonitor(object sender, EventArgs e)
        {
            using (var monitor = new DataMonitor(Browser)) {
                monitor.ShowDialog();
            }
        }

        public override void TickTimer()
        {
            int num = ListView.Items.Count;
            for (int i = 0; i < num; i++) {
                ListViewItem item = ListView.Items[i];
                TSPoint point = item.Tag as TSPoint;
                if (point != null) {
                    double curValue = fModel.GetCurrentValue(point.Id);
                    string strVal = ALCore.GetDecimalStr(curValue);
                    item.SubItems[6].Text = strVal;
                }
            }
        }
    }
}
