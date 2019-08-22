﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Components;
using AquaLog.Core;
using AquaLog.Core.Model;
using AquaLog.UI;

namespace AquaLog.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterChangePanel : ListPanel<WaterChange, WaterChangeEditDlg>
    {
        public WaterChangePanel()
        {
            ListView.Columns.Add("Aquarium", 200, HorizontalAlignment.Left);
            ListView.Columns.Add("ChangeDate", 100, HorizontalAlignment.Left);
            ListView.Columns.Add("Type", 80, HorizontalAlignment.Left);
            ListView.Columns.Add("Volume", 50, HorizontalAlignment.Right);
            ListView.Columns.Add("Note", 250, HorizontalAlignment.Left);
        }

        protected override void InitActions()
        {
            fActions.Add(new UserAction("Add", "btn_rec_new.gif", AddHandler));
            fActions.Add(new UserAction("Edit", "btn_rec_edit.gif", EditHandler));
            fActions.Add(new UserAction("Delete", "btn_rec_delete.gif", DeleteHandler));
        }

        public override void UpdateContent()
        {
            ListView.Items.Clear();
            if (fModel == null) return;

            var records = fModel.QueryWaterChanges();

            foreach (WaterChange rec in records) {
                Aquarium aqm = fModel.GetRecord<Aquarium>(rec.AquariumId);
                string aqmName = (aqm == null) ? "" : aqm.Name;

                var item = new ListViewItem(aqmName);
                item.Tag = rec;
                item.SubItems.Add(ALCore.GetDateStr(rec.ChangeDate));
                item.SubItems.Add(rec.Type.ToString());
                item.SubItems.Add(ALCore.GetDecimalStr(rec.Volume));
                item.SubItems.Add(rec.Note);
                ListView.Items.Add(item);
            }
        }
    }
}
