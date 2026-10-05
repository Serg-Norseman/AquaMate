/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.TSDB;
using AquaMate.UI.Dialogs;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    public sealed class TSValuePanel : ListPanel
    {
        private int fPointId;


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

        protected override void UpdateListView()
        {
            if (fPointId == 0) return;

            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillTSValuesLV(lv, fModel, fPointId);
        }

        protected override void AddHandler(object sender, EventArgs e)
        {
            var record = new TSValue();

            using (var dlg = new TSValueEditDlg()) {
                dlg.SetContext(fModel, record);

                if (dlg.ShowModal()) {
                    fModel.TSDB.InsertValue(fPointId, record.Timestamp, record.Value);
                    UpdateContent();
                }
            }
        }

        protected override void EditHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<TSValue>();
            if (record == null) return;

            using (var dlg = new TSValueEditDlg()) {
                dlg.SetContext(fModel, record);

                if (dlg.ShowModal()) {
                    fModel.TSDB.UpdateValue(fPointId, record.Timestamp, record.Value);
                    UpdateContent();
                }
            }
        }

        protected override void DeleteHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<TSValue>();
            if (record == null) return;

            fModel.TSDB.DeleteValue(fPointId, record.Timestamp);
            UpdateContent();
        }
    }
}
