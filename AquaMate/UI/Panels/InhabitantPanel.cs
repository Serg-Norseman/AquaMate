/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
    public sealed class InhabitantPanel : ListPanel<Inhabitant, InhabitantEditDlg>
    {
        private readonly Label fFooter;

        public InhabitantPanel() : base()
        {
            fFooter = new Label();
            fFooter.BorderStyle = BorderStyle.Fixed3D;
            fFooter.Dock = DockStyle.Bottom;
            fFooter.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold, this.Font.Unit);
            fFooter.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(fFooter);

            Controls.SetChildIndex(ListView, 0);
            Controls.SetChildIndex(fFooter, 1);
        }

        protected override void InitActions()
        {
            AddAction("Add", LSID.Add, "btn_rec_new.gif", AddHandler);
            AddAction("Edit", LSID.Edit, "btn_rec_edit.gif", EditHandler);
            AddAction("Delete", LSID.Delete, "btn_rec_delete.gif", DeleteHandler);
            AddAction("Transfer", LSID.Transfer, null, TransferHandler);
            AddAction("Chart", LSID.Chart, "", ViewLifeLinesHandler);
            AddAction("ChartFamilies", LSID.ChartFamilies, "", ViewChartFamiliesHandler);
            AddAction("Export", LSID.Export, "btn_excel.gif", ExportHandler);
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
            ModelPresenter.FillInhabitantLV(LV, Footer, fModel);
        }

        private void TransferHandler(object sender, EventArgs e)
        {
            var record = ListView.GetSelectedTag<Inhabitant>();
            if (record == null) return;

            SpeciesType speciesType = fModel.GetSpeciesType(record.SpeciesId);
            ItemType itemType = ALCore.GetItemType(speciesType);
            Browser.TransferItem(itemType, record.Id, this);
        }

        private void ViewLifeLinesHandler(object sender, EventArgs e)
        {
            Browser.SetView(MainView.LifeLinesChart, null);
        }

        private void ViewChartFamiliesHandler(object sender, EventArgs e)
        {
            Browser.SetView(MainView.BioTreemap, null);
        }

        private void ExportHandler(object sender, EventArgs e)
        {
            Export();
        }

        #region View interface implementation

        IListView LV
        {
            get { return GetControlHandler<IListView>(ListView); }
        }

        ILabel Footer
        {
            get { return GetControlHandler<ILabel>(fFooter); }
        }

        #endregion
    }
}
