﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AquaLog.Components;
using AquaLog.Core.Model;

namespace AquaLog.Panels
{
    public class Trend
    {
        public string Name;
        public Color Color;
        public List<ChartPoint> Points;

        public Trend(string name, Color color)
        {
            Name = name;
            Color = color;
            Points = new List<ChartPoint>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MeasuresChartPanel : DataPanel
    {
        private ZGraphControl fGraph;
        private IEnumerable<Measure> fMeasures;
        private Dictionary<string, Trend> fTrends;


        public MeasuresChartPanel()
        {
            fGraph = new ZGraphControl();
            fGraph.Dock = DockStyle.Fill;
            Controls.Add(fGraph);
        }

        public override void SetExtData(object extData)
        {
        }

        protected override void InitActions()
        {
        }

        public override void UpdateContent()
        {
            fGraph.Clear();
            if (fModel == null) return;

            fMeasures = fModel.QueryMeasures();
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

            foreach (Measure rec in fMeasures) {
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
            }

            foreach (var trendPair in fTrends) {
                var trend = trendPair.Value;
                fGraph.PrepareArray(trend.Name, "Time", "Value", ChartStyle.Point, trend.Points, trend.Color);
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
    }
}
