﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Model;
using AquaLog.Core.Types;
using AquaLog.UI.Dialogs;

namespace AquaLog.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NutritionPanel : ListPanel<Nutrition, NutritionEditDlg>
    {
        public NutritionPanel()
        {
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
            AddAction("Transfer", LSID.Transfer, null, TransferHandler);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);
            SetActionEnabled("Transfer", enabled);
        }

        protected override void UpdateListView()
        {
            ListView.Clear();
            ListView.Columns.Add(Localizer.LS(LSID.Aquarium), 200, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Name), 100, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Brand), 50, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Amount), 100, HorizontalAlignment.Right);
            ListView.Columns.Add(Localizer.LS(LSID.Note), 80, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Inhabitant), 80, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.State), 80, HorizontalAlignment.Left);

            var records = fModel.QueryNutritions();
            foreach (Nutrition rec in records) {
                Aquarium aqm = fModel.GetRecord<Aquarium>(rec.AquariumId);
                string aqmName = (aqm == null) ? "" : aqm.Name;

                Inhabitant inhab = fModel.GetRecord<Inhabitant>(rec.InhabitantId);
                string inhabName = (inhab == null) ? "" : inhab.Name;

                ItemState itemState;
                string strState = fModel.GetItemStateStr(rec.Id, ItemType.Nutrition, out itemState);

                var item = ListView.AddItemEx(rec,
                               aqmName,
                               rec.Name,
                               rec.Brand,
                               ALCore.GetDecimalStr(rec.Amount),
                               rec.Note,
                               inhabName,
                               strState
                           );

                if (itemState == ItemState.Finished) {
                    item.ForeColor = Color.Gray;
                }
            }
        }

        private void TransferHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<Nutrition>();
            if (record == null) return;

            ItemType itemType = ItemType.Nutrition;
            Browser.TransferItem(itemType, record.Id, this);
        }
    }
}
