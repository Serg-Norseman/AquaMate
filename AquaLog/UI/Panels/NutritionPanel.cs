﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Model;
using AquaLog.UI.Dialogs;

namespace AquaLog.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class NutritionPanel : ListPanel<Nutrition, NutritionEditDlg>
    {
        public NutritionPanel()
        {
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
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

            var records = fModel.QueryNutritions();
            foreach (Nutrition rec in records) {
                Aquarium aqm = fModel.GetRecord<Aquarium>(rec.AquariumId);
                string aqmName = (aqm == null) ? "" : aqm.Name;

                Inhabitant inhab = fModel.GetRecord<Inhabitant>(rec.InhabitantId);
                string inhabName = (inhab == null) ? "" : inhab.Name;

                var item = new ListViewItem(aqmName);
                item.Tag = rec;
                item.SubItems.Add(rec.Name);
                item.SubItems.Add(rec.Brand);
                item.SubItems.Add(ALCore.GetDecimalStr(rec.Amount));
                item.SubItems.Add(rec.Note);
                item.SubItems.Add(inhabName);
                ListView.Items.Add(item);
            }
        }
    }
}
