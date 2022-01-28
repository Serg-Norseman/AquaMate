/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Dialogs;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
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
            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillNutritionsLV(lv, fModel);
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
