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
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Charts;
using AquaMate.UI.Components;
using BSLib.Controls;

namespace AquaMate.UI.Panels
{
    public class Trend
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public List<ChartPoint> Points { get; private set; }

        public Trend(string name, Color color)
        {
            Name = name;
            Color = color;
            Points = new List<ChartPoint>();
        }
    }

    public sealed class MeasuresChartPanel : DataPanel
    {
        private readonly ZChart fChart;
        private string fSelectedAquarium;
        private Dictionary<string, Trend> fTrends;


        public MeasuresChartPanel()
        {
            fChart = new ZChart();
            fChart.Dock = DockStyle.Fill;
            Controls.Add(fChart);

            fSelectedAquarium = "*";
        }

        public override void UpdateContent()
        {
            fChart.Clear();
            if (fModel == null) return;

            var measures = fModel.QueryMeasures();
            fTrends = new Dictionary<string, Trend>();
            fTrends.Add("Temp", new Trend("Temp (°C)", Color.Red));
            fTrends.Add("NO3", new Trend("NO3 (mg/l)", Color.BlueViolet));
            fTrends.Add("NO2", new Trend("NO2 (mg/l)", Color.CornflowerBlue));
            fTrends.Add("GH", new Trend("GH (°d)", Color.DarkGray));
            fTrends.Add("KH", new Trend("KH (°d)", Color.Gray));
            fTrends.Add("pH", new Trend("pH", Color.Fuchsia));
            fTrends.Add("Cl2", new Trend("Cl2 (mg/l)", Color.GreenYellow));
            fTrends.Add("CO2", new Trend("CO2", Color.Maroon));
            fTrends.Add("NH", new Trend("NHtot", Color.Violet));
            fTrends.Add("NH3", new Trend("NH3", Color.PaleVioletRed));
            fTrends.Add("NH4", new Trend("NH4", Color.MediumVioletRed));
            fTrends.Add("PO4", new Trend("PO4", Color.BlueViolet));

            foreach (Measure rec in measures) {
                Aquarium aqm = fModel.Cache.Get<Aquarium>(ItemType.Aquarium, rec.AquariumId);
                string aqmName = (aqm == null) ? "" : aqm.Name;
                if (fSelectedAquarium != "*" && fSelectedAquarium != aqmName) continue;

                AddTrendValue("Temp", rec.Timestamp, rec.Temperature);
                AddTrendValue("NO3", rec.Timestamp, rec.NO3);
                AddTrendValue("NO2", rec.Timestamp, rec.NO2);
                AddTrendValue("GH", rec.Timestamp, rec.GH);
                AddTrendValue("KH", rec.Timestamp, rec.KH);
                AddTrendValue("pH", rec.Timestamp, rec.pH);
                AddTrendValue("Cl2", rec.Timestamp, rec.Cl2);
                AddTrendValue("CO2", rec.Timestamp, rec.CO2);
                AddTrendValue("NH", rec.Timestamp, rec.NH);
                AddTrendValue("NH3", rec.Timestamp, rec.NH3);
                AddTrendValue("NH4", rec.Timestamp, rec.NH4);
                AddTrendValue("PO4", rec.Timestamp, rec.PO4);
            }

            foreach (var trendPair in fTrends) {
                var trend = trendPair.Value;
                fChart.ShowData("", "Time", "Value", new ChartSeries(trend.Name, ChartStyle.Point, trend.Points, trend.Color));
            }
        }

        private void AddTrendValue(string key, DateTime timestamp, double value)
        {
            if (value == 0.0d) return;

            Trend trend;
            if (fTrends.TryGetValue(key, out trend)) {
                trend.Points.Add(new ChartPoint(timestamp, value));
            }
        }

        protected override void InitActions()
        {
            ClearActions();

            var aquariums = fModel.QueryAquariums();
            string[] items = new string[aquariums.Count + 1];
            items[0] = "*";
            int i = 1;
            foreach (var aqm in aquariums) {
                items[i] = aqm.Name;
                i += 1;
            }
            AddSingleSelector("AqmSelector", items, AquariumChangeHandler);

            if (fTrends != null) {
                string[] trendItems = fTrends.Keys.ToArray();
                AddMultiSelector("TrendSelector", trendItems, ItemChangeHandler);
            }
        }

        private void AquariumChangeHandler(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            fSelectedAquarium = (comboBox != null) ? comboBox.Text : "*";
            UpdateContent();
        }

        private void ItemChangeHandler(object sender, EventArgs e)
        {
            var picker = sender as OptionsPicker;
            string[] selected = picker.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var series = new Dictionary<string, ChartSeries>();
            foreach (string key in selected) {
                Trend trend;
                if (fTrends.TryGetValue(key, out trend)) {
                    series.Add(key, new ChartSeries(key, ChartStyle.Point, trend.Points, trend.Color));
                }
            }
            fChart.ShowData("", "Time", "", series);
        }
    }
}
