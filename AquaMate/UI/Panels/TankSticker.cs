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
using BSLib;

namespace AquaMate.UI.Panels
{
    public enum TankState
    {
        Normal,
        Warning,
        Alert,
        Inactive
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class TankSticker : UserControl
    {
        private Aquarium fAquarium;
        private IModel fModel;
        private bool fSelected;
        private readonly StringFormat fStrFormat;
        private SolidBrush fTextBrush;
        private List<MeasureValue> fValues;
        private WorkTime fWorkTime;

        public Aquarium Aquarium
        {
            get { return fAquarium; }
            set {
                if (fAquarium != value) {
                    fAquarium = value;
                    UpdateView();
                }
            }
        }

        public IModel Model
        {
            get { return fModel; }
            set { fModel = value; }
        }

        public bool Selected
        {
            get { return fSelected; }
            set {
                if (fSelected != value) {
                    fSelected = value;
                    Refresh();
                }
            }
        }


        public TankSticker()
        {
            Margin = new Padding(10);

            int XS = 24;
            MinimumSize = new Size(XS * 16, XS * 9);
            MaximumSize = new Size(XS * 16, XS * 9);

            fStrFormat = new StringFormat();
            fTextBrush = null;
            fValues = new List<MeasureValue>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fStrFormat.Dispose();
                if (fTextBrush != null) fTextBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SetTankState(TankState state)
        {
            int color;
            switch (state) {
                case TankState.Normal:
                    color = ALCore.NormalState;
                    break;
                case TankState.Warning:
                    color = ALCore.WarningState;
                    break;
                case TankState.Alert:
                    color = ALCore.AlertState;
                    break;
                case TankState.Inactive:
                    color = ALCore.InactiveState;
                    break;
                default:
                    color = ALCore.NormalState;
                    break;
            }
            BackColor = UIHelper.CreateColor(color);
        }

        public void UpdateView()
        {
            fWorkTime = fModel.GetWorkTime(fAquarium);

            if (!fWorkTime.WasStarted() || fWorkTime.IsInactive()) {
                SetTankState(TankState.Inactive);
            } else {
                SetTankState(TankState.Normal);
            }

            fValues = fModel.CollectData(fAquarium);

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gfx = e.Graphics;

            ButtonBorderStyle style = (fSelected) ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset;
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Silver, style);

            if (fAquarium == null) return;

            var layoutRect = ClientRectangle;
            layoutRect.Inflate(-4, -4);

            Font font = new Font(Font, FontStyle.Bold);

            fStrFormat.Alignment = StringAlignment.Near;
            DrawText(gfx, fAquarium.Name, font, ForeColor, layoutRect);

            double normalWaterVolume = fAquarium.CalcWaterVolume();
            double waterVolume = fModel.GetWaterVolume(fAquarium.Id);
            string volumes = ALCore.GetDecimalStr(waterVolume) + " / " + ALCore.GetDecimalStr(normalWaterVolume) + " / " + ALCore.GetDecimalStr(fAquarium.TankVolume);
            fStrFormat.Alignment = StringAlignment.Far;
            Color wvColor = (DoubleHelper.Equals(normalWaterVolume, waterVolume, 0.5)) ? Color.Green : Color.Orange;
            DrawText(gfx, volumes, font, wvColor, layoutRect);

            string works = fWorkTime.GetWorkDays();
            int x = layoutRect.Left;
            int y = layoutRect.Top + (int)(Font.Height * 1.6f);
            DrawText(gfx, works, Font, ForeColor, x, y);

            Color wsColor = ForeColor;
            string waterStatus = "";
            string waterChanges = "";
            if (!fWorkTime.IsInactive()) {
                if (!fWorkTime.WasStarted()) {
                    waterChanges = "---";
                } else {
                    double avgChangeDays;
                    double lastChangeDays;
                    fModel.GetWaterChangeIntervals(fAquarium.Id, fWorkTime, out avgChangeDays, out lastChangeDays);

                    string avgChange = "avg=" + ALCore.GetDecimalStr(avgChangeDays, 1) + "d";
                    string lastChange = ", last=" + ALCore.GetDecimalStr(lastChangeDays, 1) + "d";

                    if (lastChangeDays <= avgChangeDays) {
                        waterStatus = " [normal]";
                        wsColor = Color.Green;
                    } else if (lastChangeDays >= avgChangeDays * 2) {
                        waterStatus = " [alarm]";
                        wsColor = Color.Red;
                    } else if (avgChangeDays + 1 < lastChangeDays) {
                        waterStatus = " [exceeded]";
                        wsColor = Color.Orange;
                    }

                    waterChanges = avgChange + lastChange + waterStatus;
                }
            } else {
                waterChanges = "---";
            }

            y = y + (int)(Font.Height * 1.6f);
            DrawText(gfx, Localizer.LS(LSID.WaterChanges) + ": " + waterChanges, Font, wsColor, x, y);

            int inhabCount = fModel.QueryInhabitantsCount(fAquarium.Id);
            y = y + (int)(Font.Height * 1.6f);
            DrawText(gfx, Localizer.LS(LSID.Inhabitants) + ": " + inhabCount.ToString(), Font, ForeColor, x, y);

            int xoffset = layoutRect.Width / 4;
            int col = 0;
            for (int i = 0; i < 13; i++) {
                if (i % 4 == 0) {
                    y = y + (int)(Font.Height * 1.6f);
                    col = 0;
                }

                DrawMeasure(gfx, i, font, x + (xoffset * col), y);
                col += 1;
            }
        }

        private void DrawMeasure(Graphics gfx, int index, Font font, int x, int y)
        {
            MeasureValue tVal = fValues[index];
            if (!string.IsNullOrEmpty(tVal.Text) && !double.IsNaN(tVal.Value)) {
                DrawText(gfx, tVal.Text, font, tVal.Color, x, y);
            }
        }

        private void DrawText(Graphics gfx, string text, Font font, Color color, int x, int y)
        {
            if (fTextBrush == null) {
                fTextBrush = new SolidBrush(color);
            }
            fTextBrush.Color = color;
            gfx.DrawString(text, font, fTextBrush, x, y);
        }

        private void DrawText(Graphics gfx, string text, Font font, Color color, Rectangle layoutRect)
        {
            if (fTextBrush == null) {
                fTextBrush = new SolidBrush(color);
            }
            fTextBrush.Color = color;
            gfx.DrawString(text, font, fTextBrush, layoutRect, fStrFormat);
        }
    }
}
