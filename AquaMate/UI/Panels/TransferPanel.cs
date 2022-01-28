/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.UI.Dialogs;
using BSLib.Design.Handlers;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TransferPanel : ListPanel<Transfer, TransferEditDlg>
    {
        public TransferPanel()
        {
        }

        protected override void InitActions()
        {
            // "Add" - only from other panels
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);

            SetActionEnabled("Edit", enabled);
            SetActionEnabled("Delete", enabled);
        }

        protected override void UpdateListView()
        {
            Font defFont = ListView.Font;
            var boldFont = new FontHandler(new Font(defFont, FontStyle.Bold));

            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillTransfersLV(lv, fModel, boldFont);
        }
    }
}
