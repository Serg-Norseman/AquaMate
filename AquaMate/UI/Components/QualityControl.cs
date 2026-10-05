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
using System.Windows.Forms;
using AquaMate.Core.Types;

namespace AquaMate.UI.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class QualityControl : UserControl
    {
        private class VRItem
        {
            public ValueRange Range;
            public double Length;
            public Rectangle Rect;
            public Brush Brush;
        }


        private const int LayoutPadding = 10;
        private const string ValuesFormat = "{0:0.00}";

        private readonly Brush fEmptyBrush;
        private readonly List<VRItem> fList;
        private ValueRange[] fRanges;
        private double fRangesLength;
        private Font fScaleFont;
        private double fScaleWidth;
        private string fTitle;
        private double fValue;
        private int fValuePos;


        public ValueRange[] Ranges
        {
            get { return fRanges; }
            set {
                fRanges = value;
                UpdateContent();
            }
        }

        public double Value
        {
            get { return fValue; }
            set {
                fValue = value;
                UpdateContent();
            }
        }

        public string Title
        {
            get { return fTitle; }
            set {
                fTitle = value;
                UpdateContent();
            }
        }


        public QualityControl()
        {
            BorderStyle = BorderStyle.FixedSingle;

            fEmptyBrush = new SolidBrush(Color.Gray);
            fList = new List<VRItem>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fEmptyBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateContent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (Width <= 0 || Height <= 0) return;

            Graphics gfx = e.Graphics;
            int lineHeight = Font.Height;

            var titleFont = new Font(Font, FontStyle.Bold);
            gfx.DrawString(fTitle, titleFont, Brushes.Black, 0, 0);

            fScaleFont = new Font(Font.FontFamily, Font.SizeInPoints * 0.8f);

            int count = fList.Count;
            if (count > 0) {
                string line = string.Format(ValuesFormat, fValue);
                DrawText(gfx, Font, Brushes.Black, line, fValuePos, (lineHeight + Gap) * 1, -1);

                line = "▼";
                DrawText(gfx, Font, Brushes.Black, line, fValuePos, (lineHeight + Gap) * 2, -1);

                int markersY = (lineHeight + Gap) * 3 + (lineHeight / 2 + Gap);
                for (int i = 0; i < count; i++) {
                    VRItem item = fList[i];
                    DrawRect(gfx, item.Rect, item.Brush);

                    if (i == 0) {
                        double val = item.Range.Min;
                        line = string.Format(ValuesFormat, val);
                        float x = LayoutPadding + (int)(fScaleWidth * (val / fRangesLength));
                        DrawText(gfx, fScaleFont, Brushes.Black, line, x, markersY, 2);
                    } else if (i == count - 1) {
                        double val = item.Range.Min;
                        line = string.Format(ValuesFormat, val);
                        float x = LayoutPadding + (int)(fScaleWidth * (val / fRangesLength));
                        DrawText(gfx, fScaleFont, Brushes.Black, line, x, markersY, -1);

                        val = item.Range.Max;
                        line = string.Format(ValuesFormat, val);
                        x = LayoutPadding + (int)(fScaleWidth * (val / fRangesLength));
                        DrawText(gfx, fScaleFont, Brushes.Black, line, x, markersY, 3);
                    } else {
                        double val = item.Range.Min;
                        line = string.Format(ValuesFormat, val);
                        float x = LayoutPadding + (int)(fScaleWidth * (val / fRangesLength));
                        DrawText(gfx, fScaleFont, Brushes.Black, line, x, markersY, -1);
                    }
                }
            } else {
                //DrawRect(gfx, 10, Width - 20, fEmptyBrush);
            }
        }

        private void DrawRect(Graphics gfx, Rectangle rt, Brush lb)
        {
            if (rt.Width > 0) {
                gfx.FillRectangle(lb, rt);
            }
        }

        private void DrawText(Graphics gfx, Font font, Brush brush, string text, float x, float y, int quad = 2)
        {
            // quadrant clockwise from 00 hours
            SizeF tsz = gfx.MeasureString(text, font);

            switch (quad) {
                case -1:
                    x -= (tsz.Width / 2.0f);
                    break;
                case 1:
                    y -= tsz.Height;
                    break;
                case 2:
                    break;
                case 3:
                    x -= tsz.Width;
                    break;
                case 4:
                    x -= tsz.Width;
                    y -= tsz.Height;
                    break;
            }

            gfx.DrawString(text, font, brush, x, y);
        }

        private const int Gap = 0;

        private void UpdateContent()
        {
            int lineHeight = Font.Height;
            int scaleHeight = lineHeight / 2;
            ClientSize = new Size(ClientSize.Width, (lineHeight * 4) + scaleHeight + (Gap * 6));
            fList.Clear();
            if (fRanges == null) return;

            int count = fRanges.Length;
            fScaleWidth = Width - (LayoutPadding * 2) - (count - 1);

            fRangesLength = 0.0;
            foreach (var range in fRanges) {
                VRItem item = new VRItem();
                item.Range = range;
                item.Length = (range.Max - range.Min);
                item.Brush = new SolidBrush(range.Color);
                fList.Add(item);

                fRangesLength += item.Length;
            }

            int x = LayoutPadding;
            int y = (lineHeight + Gap) * 3;
            for (int i = 0; i < count; i++) {
                var item = fList[i];
                int itemWidth = (int)(fScaleWidth * (item.Length / fRangesLength));
                item.Rect = new Rectangle(x, y, itemWidth, scaleHeight);
                x += (itemWidth + 1);
            }

            fValuePos = LayoutPadding + (int)(fScaleWidth * (fValue / fRangesLength));

            Invalidate();
        }
    }
}
