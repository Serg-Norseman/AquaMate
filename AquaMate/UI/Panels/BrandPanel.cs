/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using AquaMate.UI.Dialogs;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BrandPanel : ListPanel<Brand, BrandEditDlg>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "BrandPanel");

        public BrandPanel()
        {
        }

        protected override void UpdateListView()
        {
            var lv = GetControlHandler<IListView>(ListView);
            ModelPresenter.FillBrandsLV(lv, fModel);
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);

            AddAction("ViewSite", LSID.ViewSite, "", ViewSiteHandler);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            bool enabled = (records.Count == 1);
            SetActionEnabled("ViewSite", enabled);
        }

        private void ViewSiteHandler(object sender, EventArgs e)
        {
            try {
                var record = ListView.GetSelectedTag<Brand>();
                if (record == null) return;

                AppHost.LoadExtFile(record.WebSite);
            } catch (Exception ex) {
                fLogger.WriteError("ViewSiteHandler()", ex);
            }
        }
    }
}
