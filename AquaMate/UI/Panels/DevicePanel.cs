/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Dialogs;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    public sealed class DevicePanel : ListPanel<Device, DeviceEditDlg>
    {
        private readonly Label fFooter;

        public DevicePanel()
        {
            fFooter = new Label();
            fFooter.BorderStyle = BorderStyle.Fixed3D;
            fFooter.Dock = DockStyle.Bottom;
            fFooter.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold, this.Font.Unit);
            fFooter.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(fFooter);

            Controls.SetChildIndex(ListView, 0);
            Controls.SetChildIndex(fFooter, 1);
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
            AddAction("Transfer", LSID.Transfer, null, TransferHandler);

            AddAction("Data", LSID.Data, "", ViewDataHandler);
            AddAction("Trend", LSID.Trend, "", ViewTrendHandler);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);
            SetActionEnabled("Transfer", enabled);

            SetActionEnabled("Data", enabled);
            SetActionEnabled("Trend", enabled);

            if (enabled) {
                var device = records[0] as Device;
                if (device.PointId != 0) {
                    SetActionEnabled("Transfer", false);
                }
            }
        }

        protected override void UpdateListView()
        {
            var lv = GetControlHandler<IListView>(ListView);
            var footer = GetControlHandler<ILabel>(fFooter);
            ModelPresenter.FillDevicesLV(lv, footer, fModel);
        }

        public override void TickTimer()
        {
            int num = ListView.Items.Count;
            for (int i = 0; i < num; i++) {
                ListViewItem item = ListView.Items[i];
                Device device = item.Tag as Device;
                if (device != null) {
                    double curValue = fModel.GetCurrentValue(device.PointId);
                    string strVal = ALCore.GetDecimalStr(curValue);
                    item.SubItems[9].Text = strVal;
                }
            }
        }

        private void ViewDataHandler(object sender, EventArgs e)
        {
            var device = ListView.GetSelectedTag<Device>();
            if (device == null || device.PointId == 0) return;

            Browser.SetView(MainView.TSValues, device.PointId);
        }

        private void ViewTrendHandler(object sender, EventArgs e)
        {
            var device = ListView.GetSelectedTag<Device>();
            if (device == null || device.PointId == 0) return;

            Browser.SetView(MainView.TSTrend, device.PointId);
        }

        private void TransferHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<Device>();
            if (record == null) return;

            ItemType itemType = ItemType.Device;
            Browser.TransferItem(itemType, record.Id, this);
        }
    }
}
