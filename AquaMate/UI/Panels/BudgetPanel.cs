/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Charts;
using AquaMate.UI.Components;
using BSLib;
using BSLib.Design.Handlers;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    public enum BudgetChartType
    {
        ItemTypes,
        Shops,
        Brands,
        Countries,
        Monthes,
    }

    public sealed class BudgetPanel : ListPanel
    {
        private readonly Label fFooter;
        private string fTotalFooter;

        public BudgetPanel()
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
            AddAction("ChartTypes", LSID.ChartItemTypes, "", ViewChartTypesHandler);
            AddAction("ChartShops", LSID.ChartShops, "", ViewChartShopsHandler);
            AddAction("ChartBrands", LSID.ChartBrands, "", ViewChartBrandsHandler);
            AddAction("ChartCountries", LSID.ChartCountries, "", ViewChartCountriesHandler);
            AddAction("ChartMonthes", LSID.ChartMonthes, "", ViewChartMonthesHandler);
            AddAction("Brands", LSID.Brands, "", ViewBrandsHandler);
            AddAction("Shops", LSID.Shops, "", ViewShopsHandler);
            AddAction("Pricelist", LSID.Pricelist, "", ViewPricelistHandler);
            AddAction("Export", LSID.Export, "btn_excel.gif", ExportHandler);
        }

        protected override void UpdateListView()
        {
            var boldFont = new FontHandler(new Font(ListView.Font, FontStyle.Bold));
            var lv = GetControlHandler<IListView>(ListView);
            var records = fModel.QueryTransferExpenses();
            ModelPresenter.FillBudgetLV(lv, fModel, records, boldFont);

            fTotalFooter = CalcFooter(records);

            CollectBrands(records);
            CollectShops(records);
        }

        public override void SelectionChanged(IList<Entity> records)
        {
            if (records.Count > 1) {
                IList<Transfer> transfers = records.Cast<Transfer>().ToList();
                fFooter.Text = CalcFooter(transfers);
            } else {
                fFooter.Text = fTotalFooter;
            }
        }

        private string CalcFooter(IList<Transfer> records)
        {
            DateTime firstDate = ALCore.ZeroDate, lastDate = DateTime.Now.Date;
            double totalSum = 0.0d, expenses = 0.0d, incomes = 0.0d;
            foreach (Transfer rec in records) {
                int factor = 0;
                switch (rec.Type) {
                    case TransferType.Purchase:
                        factor = -1;
                        break;
                    case TransferType.Sale:
                        factor = +1;
                        break;
                }

                if (factor != 0) {
                    double sum = rec.Quantity * rec.UnitPrice;
                    if (factor > 0) {
                        incomes += sum;
                    } else {
                        expenses += sum;
                    }
                    totalSum += (sum * factor);

                    if (ALCore.IsZeroDate(firstDate)) {
                        firstDate = rec.Timestamp;
                    }
                }
            }

            double days = (lastDate - firstDate).TotalDays;
            double avgExpense = expenses / days;
            return string.Format(Localizer.LS(LSID.BalanceFooter), expenses, avgExpense, incomes, totalSum);
        }

        private void ViewBrandsHandler(object sender, EventArgs e)
        {
            Browser.SetView(MainView.Brands, null);
        }

        private void ViewShopsHandler(object sender, EventArgs e)
        {
            Browser.SetView(MainView.Shops, null);
        }

        private void ViewPricelistHandler(object sender, EventArgs e)
        {
            Browser.SetView(MainView.Pricelist, null);
        }

        private void ViewChartTypesHandler(object sender, EventArgs e)
        {
            var chartData = GetChartData(BudgetChartType.ItemTypes);
            Browser.SetView(MainView.ZChart, new ChartSeries("", ChartStyle.Pie, chartData, Color.Transparent));
        }

        private void ViewChartShopsHandler(object sender, EventArgs e)
        {
            var chartData = GetChartData(BudgetChartType.Shops);
            Browser.SetView(MainView.ZChart, new ChartSeries("", ChartStyle.Pie, chartData, Color.Transparent));
        }

        private void ViewChartBrandsHandler(object sender, EventArgs e)
        {
            var chartData = GetChartData(BudgetChartType.Brands);
            Browser.SetView(MainView.ZChart, new ChartSeries("", ChartStyle.Pie, chartData, Color.Transparent));
        }

        private void ViewChartCountriesHandler(object sender, EventArgs e)
        {
            var chartData = GetChartData(BudgetChartType.Countries);
            Browser.SetView(MainView.ZChart, new ChartSeries("", ChartStyle.Pie, chartData, Color.Transparent));
        }

        private void ViewChartMonthesHandler(object sender, EventArgs e)
        {
            var purcChartData = GetChartData(BudgetChartType.Monthes, TransferType.Purchase);
            var saleChartData = GetChartData(BudgetChartType.Monthes, TransferType.Sale);

            var series = new Dictionary<string, ChartSeries>();
            series.Add(Localizer.LS(LSID.Purchase), new ChartSeries(Localizer.LS(LSID.Purchase), ChartStyle.Bar, purcChartData, Color.Red));
            series.Add(Localizer.LS(LSID.Sale), new ChartSeries(Localizer.LS(LSID.Sale), ChartStyle.Bar, saleChartData, Color.Green));

            Browser.SetView(MainView.ZChart, series);
        }

        private void CollectBrands(IList<Transfer> transfers)
        {
            var brandRecords = fModel.QueryBrands();

            foreach (Transfer rec in transfers) {
                var itemRec = fModel.GetRecord(rec.ItemType, rec.ItemId);
                var brandedItem = itemRec as IBrandedItem;
                string brandName = (brandedItem == null) ? null : brandedItem.Brand;

                if (!string.IsNullOrEmpty(brandName) && !brandRecords.Any(p => p.Name == brandName)) {
                    var brand = new Brand(brandName);
                    fModel.AddRecord(brand);
                    brandRecords.Add(brand);
                }
            }
        }

        private void CollectShops(IList<Transfer> transfers)
        {
            var shopRecords = fModel.QueryShops();

            foreach (Transfer rec in transfers) {
                string shopName = rec.Shop;

                if (!string.IsNullOrEmpty(shopName) && !shopRecords.Any(p => p.Name == shopName)) {
                    var shop = new Shop(shopName);
                    fModel.AddRecord(shop);
                    shopRecords.Add(shop);
                }
            }
        }

        private IList<ChartPoint> GetChartData(BudgetChartType chartType, TransferType transferType = TransferType.Purchase)
        {
            Dictionary<string, ChartPoint> result = new Dictionary<string, ChartPoint>();

            IList<Brand> brandRecords = null;
            if (chartType == BudgetChartType.Countries) {
                brandRecords = fModel.QueryBrands();
            }

            var records = fModel.QueryTransferExpenses();
            foreach (Transfer rec in records) {
                if (rec.Type != transferType) continue;

                int factor = +1;
                if (chartType == BudgetChartType.Monthes) {
                    switch (rec.Type) {
                        case TransferType.Purchase:
                            factor = -1;
                            break;
                        case TransferType.Sale:
                            factor = +1;
                            break;
                    }
                }

                double trnSum = (rec.Quantity * rec.UnitPrice) * factor;
                if (trnSum == 0.0d) continue;

                var itemRec = fModel.GetRecord(rec.ItemType, rec.ItemId);
                var ts = rec.Timestamp;

                string key;
                switch (chartType) {
                    case BudgetChartType.ItemTypes:
                        Inventory inventRec = itemRec as Inventory;
                        ItemType itType = (inventRec != null) ? ALCore.GetItemType(inventRec.Type) : rec.ItemType;
                        key = Localizer.LS(ALData.ItemTypes[(int)itType].Name);
                        break;
                    case BudgetChartType.Shops:
                        key = rec.Shop;
                        break;
                    case BudgetChartType.Brands:
                        var brandedItem = itemRec as IBrandedItem;
                        key = (brandedItem == null) ? "-" : brandedItem.Brand;
                        break;
                    case BudgetChartType.Countries:
                        var brandedItem2 = itemRec as IBrandedItem;
                        var brand = (brandedItem2 == null) ? "-" : brandedItem2.Brand;
                        var brandRec = brandRecords.FirstOrDefault(p => p.Name == brand);
                        key = (brandRec == null) ? "-" : brandRec.Country;
                        break;
                    case BudgetChartType.Monthes:
                        key = string.Format("{0}/{1}", ts.Month, ts.Year);
                        break;
                    default:
                        key = "";
                        break;
                }

                if (string.IsNullOrEmpty(key)) {
                    key = "-";
                }

                ChartPoint chartPoint;
                if (result.TryGetValue(key, out chartPoint)) {
                    chartPoint.Value += trnSum;
                    result[key] = chartPoint;
                } else {
                    chartPoint = new ChartPoint(key, trnSum);
                    if (chartType == BudgetChartType.Monthes) {
                        int days = DateHelper.DaysInMonth((short)ts.Year, (byte)ts.Month);
                        chartPoint.Timestamp = new DateTime(ts.Year, ts.Month, days);
                    }
                    result.Add(key, chartPoint);
                }
            }

            List<ChartPoint> vals = result.Values.ToList();

            if (chartType != BudgetChartType.Monthes) {
                vals = ZChart.Consolidation(vals, 0.5d);

                // prettification
                vals = ZChart.AlternateSort(vals);
            }

            return vals;
        }

        private void ExportHandler(object sender, EventArgs e)
        {
            Export();
        }
    }
}
