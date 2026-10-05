/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Export;
using AquaMate.Core.Model;
using AquaMate.Logging;
using AquaMate.UI.Components;
using BSLib;

namespace AquaMate.UI.Panels
{
    public class ListPanel : DataPanel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ListPanel");

        private readonly ZListView fListView;

        public ZListView ListView
        {
            get { return fListView; }
        }


        public ListPanel()
        {
            Padding = new Padding(10);

            fListView = UIHelper.CreateListView("ListView");
            fListView.DoubleClick += EditHandler;
            fListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            fListView.KeyDown += ListView_KeyDown;
            Controls.Add(fListView);
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) {
                EditHandler(sender, e);
            }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<Entity> records = new List<Entity>();

            foreach (ListViewItem item in ListView.SelectedItems) {
                Entity rec = item.Tag as Entity;
                if (rec != null) {
                    records.Add(rec);
                }
            }

            SelectionChanged(records);
        }

        public override void UpdateContent()
        {
            try {
                ListView.BeginUpdate();
                ListView.Items.Clear();

                if (fModel != null) {
                    UpdateListView();
                }

                ListView.EndUpdate();
            } catch (Exception ex) {
                fLogger.WriteError("UpdateContent()", ex);
            }
        }

        protected virtual void UpdateListView()
        {
        }

        protected virtual void AddHandler(object sender, EventArgs e)
        {
        }

        protected virtual void EditHandler(object sender, EventArgs e)
        {
        }

        protected virtual void DeleteHandler(object sender, EventArgs e)
        {
        }

        protected void Export()
        {
            string fileName = UIHelper.GetSaveFile("Excel files (*.xls)|*.xls|CSV files (*.csv)|*.csv");
            if (string.IsNullOrEmpty(fileName)) return;

            string ext = FileHelper.GetFileExtension(fileName);
            switch (ext) {
                case ".xls":
                    ExcelExporter.Generate(fListView, fileName);
                    break;

                case ".csv":
                    CSVExporter.Generate(fListView, fileName);
                    break;

                default:
                    return;
            }

            AppHost.LoadExtFile(fileName);
        }
    }


    public class ListPanel<R, D> : ListPanel
        where R : Entity, new()
        where D : IEditorView<R>, new()
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ListPanel<>");

        private ContextMenuStrip fMenu;

        public override void ProcessActions()
        {
            fMenu = new ContextMenuStrip();
            foreach (var action in Actions) {
                if (action.Choices == null) {
                    fMenu.Items.Add(Localizer.LS(action.BtnText), null, action.Click);
                }
            }
            ListView.ContextMenuStrip = fMenu;
        }

        private void SelectRecord(R record)
        {
            if (record == null) return;

            var lvItems = ListView.Items;
            int num = lvItems.Count;
            for (int i = 0; i < num; i++) {
                ListViewItem item = lvItems[i];
                var rowData = item.Tag as R;
                if (rowData != null && rowData.Id == record.Id) {
                    ListView.SelectItem(item);
                    return;
                }
            }
        }

        protected override void AddHandler(object sender, EventArgs e)
        {
            try {
                R record = new R();

                using (var dlg = new D()) {
                    dlg.SetContext(fModel, record);

                    if (dlg.ShowModal()) {
                        fModel.AddRecord(record);
                        UpdateContent();
                        SelectRecord(record);
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("AddHandler()", ex);
            }
        }

        protected override void EditHandler(object sender, EventArgs e)
        {
            try {
                var record = ListView.GetSelectedTag<R>();
                if (record == null) return;

                using (var dlg = new D()) {
                    dlg.SetContext(fModel, record);

                    if (dlg.ShowModal()) {
                        fModel.UpdateRecord(record);
                        UpdateContent();
                        SelectRecord(record);
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("EditHandler()", ex);
            }
        }

        protected override void DeleteHandler(object sender, EventArgs e)
        {
            try {
                var record = ListView.GetSelectedTag<R>();
                if (record == null) return;

                if (!Browser.CheckDelete(record)) return;

                fModel.DeleteRecord(record);
                UpdateContent();
            } catch (Exception ex) {
                fLogger.WriteError("DeleteHandler()", ex);
            }
        }
    }
}
