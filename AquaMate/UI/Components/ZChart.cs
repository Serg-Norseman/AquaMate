/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.UI.Charts;
using ZedGraph;

namespace AquaMate.UI.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ZChart : UserControl
    {
        private ZedGraphControl fGraph;

        public ZChart()
        {
            Clear();
        }

        public void Clear()
        {
            Controls.Clear();
            fGraph = new ZedGraphControl();
            fGraph.IsShowPointValues = true;
            fGraph.PointValueEvent += Graph_PointValueEvent;
            fGraph.Dock = DockStyle.Fill;
            Controls.Add(fGraph);

            /*GraphPane gPane = fGraph.GraphPane;

            gPane.Title.Text = "";
            gPane.XAxis.Title.Text = "";
            gPane.YAxis.Title.Text = "";

            gPane.CurveList.Clear();
            gPane.GraphObjList.Clear();

            fGraph.AxisChange();
            fGraph.Invalidate();*/
        }

        public void ShowData(string title, string xAxis, string yAxis, object data)
        {
            Clear();
            if (data == null) return;

            fGraph.GraphPane.Title.Text = title;

            var seriesList = data as Dictionary<string, ChartSeries>;
            if (seriesList != null) {
                foreach (var pair in seriesList) {
                    var series = pair.Value;
                    ShowSeries(xAxis, yAxis, series);
                }
            } else {
                var series = data as ChartSeries;
                if (series != null) {
                    ShowSeries(xAxis, yAxis, series);
                }
            }
        }

        private void ShowSeries(string xAxis, string yAxis, ChartSeries series)
        {
            IList<ChartPoint> vals = series.Data;

            GraphPane gPane = fGraph.GraphPane;
            try {
                gPane.XAxis.Title.Text = xAxis;
                //gPane.XAxis.Scale.FontSpec.Angle = 60;
                gPane.XAxis.Scale.FontSpec.Size = 10;
                gPane.YAxis.Title.Text = yAxis;
                gPane.YAxis.Scale.FontSpec.Size = 10;

                if (series.Style == ChartStyle.Pie) {
                    gPane.Legend.IsVisible = false;

                    int num = vals.Count;
                    for (int i = 0; i < num; i++) {
                        ChartPoint item = vals[i];
                        PieItem ps = gPane.AddPieSlice(item.Value, item.Color, 0F, item.Caption);
                        ps.LabelType = PieLabelType.Name_Value_Percent;
                    }
                } else if (series.Style == ChartStyle.Bar || series.Style == ChartStyle.Point) {
                    gPane.XAxis.Type = AxisType.Date;
                    gPane.Legend.IsVisible = true;

                    PointPairList ppList = new PointPairList();
                    int num = vals.Count;
                    for (int i = 0; i < num; i++) {
                        ChartPoint item = vals[i];
                        ppList.Add(new XDate(item.Timestamp), item.Value);
                    }

                    ppList.Sort();

                    switch (series.Style) {
                        case ChartStyle.Bar:
                            gPane.XAxis.Scale.Format = "yy-MM-dd";
                            gPane.XAxis.Scale.MajorUnit = DateUnit.Year;
                            gPane.XAxis.Scale.MinorUnit = DateUnit.Month;
                            gPane.AddBar(series.AxisName, ppList, series.Color);
                            break;
                        case ChartStyle.Point:
                            gPane.XAxis.Scale.Format = "yy-MM-dd HH:mm:ss";
                            gPane.XAxis.Scale.MajorUnit = DateUnit.Day;
                            gPane.XAxis.Scale.MinorUnit = DateUnit.Second;
                            gPane.AddCurve(series.AxisName, ppList, series.Color, SymbolType.Diamond).Symbol.Size = 3;
                            break;
                    }
                }
            } finally {
                fGraph.AxisChange();
                fGraph.Invalidate();
            }
        }

        private string Graph_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            var pieItem = curve as PieItem;
            if (pieItem != null) {
                return string.Format("{0}: {1:0.00}", pieItem.Label.Text, pieItem.Value);
            }

            var barItem = curve as BarItem;
            if (barItem != null) {
                return string.Format("{0}: {1:0.00}", DateTime.FromOADate(barItem[iPt].X), barItem[iPt].Y);
            }

            var lineItem = curve as LineItem;
            if (lineItem != null) {
                return string.Format("{0} ({1}): {2:0.00}", lineItem.Label.Text, DateTime.FromOADate(lineItem[iPt].X), lineItem[iPt].Y);
            }

            return string.Empty;
        }

        #region Beautify

        public static List<ChartPoint> Consolidation(List<ChartPoint> source, double minLimit = 0.25d)
        {
            double sum = 0.0d;
            foreach (var cp in source) {
                sum += cp.Value;
            }

            ChartPoint first = null;
            for (int i = source.Count - 1; i >= 0; i--) {
                var cp = source[i];
                var percent = (cp.Value / sum) * 100.00d;

                if (percent < minLimit) {
                    if (first == null) {
                        first = cp;
                    } else {
                        first.Caption = Localizer.LS(LSID.Other);
                        first.Value += cp.Value;
                        source.RemoveAt(i);
                    }
                }
            }

            return source;
        }

        // https://www.geeksforgeeks.org/rearrange-given-list-consists-alternating-minimum-maximum-elements/
        // https://www.geeksforgeeks.org/alternative-sorting/
        // https://www.geeksforgeeks.org/rearrange-array-maximum-minimum-form/
        public static List<ChartPoint> AlternateSort(List<ChartPoint> source)
        {
            int srcLen = source.Count;
            if (srcLen < 3) {
                return source;
            }

            source.Sort((x, y) => {
                return x.Value.CompareTo(y.Value);
            });

            ChartPoint[] target = new ChartPoint[srcLen];

            /*int t, b, lp, rp;
            t = srcLen - 1;
            b = 0;
            lp = t / 2 - 1;
            rp = t / 2 + 1;
            target[t / 2] = source[t];
            t--;
            while (b < t) {
                target[lp] = source[b];
                b++;
                target[rp] = source[b];
                b++;
                lp--;
                rp++;

                if (b >= t) break;

                target[lp] = source[t];
                t--;
                target[rp] = source[t];
                t--;
                lp--;
                rp++;
            }*/

            int low = 0, high = srcLen - 1;
            bool flag = true;
            for (int i = 0; i < srcLen; i++) {
                target[i] = flag ? source[high--] : source[low++];
                flag = !flag;
            }

            return new List<ChartPoint>(target);
        }

        #endregion
    }
}
