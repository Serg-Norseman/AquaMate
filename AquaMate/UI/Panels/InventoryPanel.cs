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
    public sealed class InventoryPanel : ListPanel<Inventory, InventoryEditDlg>
    {
        public InventoryPanel()
        {
        }

        protected override void UpdateListView()
        {
            ModelPresenter.FillInventoryLV(LV, fModel);
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

        private void TransferHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<Inventory>();
            if (record == null) return;

            ItemType itemType = ALCore.GetItemType(record.Type);
            Browser.TransferItem(itemType, record.Id, this);
        }

        #region View interface implementation

        IListView LV
        {
            get { return GetControlHandler<IListView>(ListView); }
        }

        #endregion
    }
}
