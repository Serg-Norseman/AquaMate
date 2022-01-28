/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.UI.Dialogs;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MaintenancePanel : ListPanel<Maintenance, MaintenanceEditDlg>
    {
        private string fSelectedAquarium;

        public MaintenancePanel()
        {
            fSelectedAquarium = "*";
        }

        protected override void UpdateListView()
        {
            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillMaintenancesLV(lv, fModel, fSelectedAquarium);
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
            AddAction("Export", LSID.Export, "btn_excel.gif", ExportHandler);

            var aquariums = fModel.QueryAquariums();
            string[] items = new string[aquariums.Count + 1];
            items[0] = "*";
            int i = 1;
            foreach (var aqm in aquariums) {
                items[i] = aqm.Name;
                i += 1;
            }
            AddSingleSelector("AqmSelector", items, AquariumChangeHandler);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);
        }

        private void AquariumChangeHandler(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            fSelectedAquarium = (comboBox != null) ? comboBox.Text : "*";
            UpdateContent();
        }

        private void ExportHandler(object sender, EventArgs e)
        {
            Export();
        }
    }
}
