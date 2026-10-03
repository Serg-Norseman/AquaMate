/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using BSLib;
using BSLib.DataViz.TreeMap;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BioTreemapPanel : DataPanel
    {
        internal sealed class SpeciesItem
        {
            public string Name;
            public string Family;
            public int Quantity;

            public SpeciesItem(string name, string family, int quantity)
            {
                Name = name;
                Family = family;
                Quantity = quantity;
            }
        }

        private static DataTable fCSVData;

        private readonly TreeMapViewer fDataMap;
        private readonly NavigationStack<MapItem> fNavman;
        private readonly MapItem fRootItem;


        static BioTreemapPanel()
        {
            string taxFile = AppHost.GetAppPath() + @"common\taxonomy.csv";
            fCSVData = CSVReader.ReadCSVFile(taxFile, Encoding.UTF8, true);
        }

        public BioTreemapPanel()
        {
            fNavman = new NavigationStack<MapItem>();
            fRootItem = new MapItem();

            fDataMap = new TreeMapViewer();
            fDataMap.RootItem = fRootItem;
            fDataMap.Dock = DockStyle.Fill;
            fDataMap.MouseoverHighlight = true;
            fDataMap.HintRequest += OnHintRequest;
            fDataMap.ShowNames = true;
            fDataMap.KeyDown += DataMap_KeyDown;
            fDataMap.MouseDoubleClick += DataMap_MouseDoubleClick;
            Controls.Add(fDataMap);

            SetRootItem(fRootItem);
        }

        private void SetRootItem(MapItem item)
        {
            if (item != null) {
                fNavman.Current = item;
                fDataMap.RootItem = item;
            }
        }

        private void DataMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SetRootItem(fDataMap.CurrentItem);
        }

        private void DataMap_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.KeyCode) {
                case Keys.Back:
                    if (fNavman.CanBackward()) {
                        fDataMap.RootItem = fNavman.Back();
                    }
                    break;
            }
        }

        private static DataRow SearchFamily(string family)
        {
            if (fCSVData != null) {
                foreach (DataRow row in fCSVData.Rows) {
                    if (string.Equals(row[4].ToString(), family, StringComparison.OrdinalIgnoreCase)) {
                        return row;
                    }
                }
            }
            return null;
        }

        private MapItem GetTaxonomyItem(DataRow taxRow)
        {
            MapItem mapItem = null;
            MapItem parent = fRootItem;
            var subItems = fRootItem.Items;
            var items = taxRow.ItemArray;
            for (int i = 0; i < items.Length; i++) {
                string itName = items[i].ToString();
                mapItem = subItems.FirstOrDefault(mit => mit.Name == itName);
                if (mapItem == null) {
                    mapItem = fDataMap.Model.CreateItem(parent, itName, 0.0d);
                }
                subItems = mapItem.Items;
                parent = mapItem;
            }
            return mapItem;
        }

        public override void UpdateContent()
        {
            fRootItem.Items.Clear();

            var unkTax = fDataMap.Model.CreateItem(fRootItem, "Unknown Taxonomy", 0.0d);

            Dictionary<string, SpeciesItem> species = new Dictionary<string, SpeciesItem>();

            IList<Inhabitant> records = fModel.QueryInhabitants();
            foreach (Inhabitant rec in records) {
                Species spc = fModel.GetRecord<Species>(rec.SpeciesId);
                if (spc == null) continue;

                SpeciesType speciesType = fModel.GetSpeciesType(rec.SpeciesId);
                ItemType itemType = ALCore.GetItemType(speciesType);
                int quantity = fModel.QueryInhabitantsCount(rec.Id, itemType);

                if (quantity != 0) {
                    string name = string.Format("{0} ({1})", spc.ScientificName, spc.Name);

                    SpeciesItem item;
                    if (!species.TryGetValue(name, out item)) {
                        item = new SpeciesItem(name, spc.BioFamily, quantity);
                        species.Add(name, item);
                    } else {
                        item.Quantity += quantity;
                    }
                }
            }

            foreach (var pair in species) {
                var item = pair.Value;
                var row = SearchFamily(item.Family);
                if (row == null) {
                    fDataMap.Model.CreateItem(unkTax, item.Name, item.Quantity);
                } else {
                    var taxItem = GetTaxonomyItem(row);
                    fDataMap.Model.CreateItem(taxItem, item.Name, item.Quantity);
                }
            }

            fDataMap.UpdateView();
        }

        private void OnHintRequest(object sender, TMHintRequestEventArgs args)
        {
            args.Hint = GetFullName(args.MapItem);
        }

        private string GetFullName(MapItem item)
        {
            string result = string.Empty;

            while (item != null) {
                if (string.IsNullOrEmpty(result)) {
                    result = item.Name;
                } else {
                    result = item.Name + "\\" + result;
                }
                item = item.Parent;
            }

            return result;
        }
    }
}
