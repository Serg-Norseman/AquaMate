/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Components;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class AquaQualityPanel : DataPanel
    {
        private const int LayoutPadding = 4;

        private Aquarium fAquarium;
        private readonly TableLayoutPanel fLayoutPanel;


        public AquaQualityPanel() : base()
        {
            fLayoutPanel = new TableLayoutPanel();
            fLayoutPanel.Dock = DockStyle.Fill;
            fLayoutPanel.Padding = new Padding(LayoutPadding);
            fLayoutPanel.AutoScroll = true;
            fLayoutPanel.AutoSize = true;

            fLayoutPanel.ColumnCount = 2;
            fLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            fLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            Controls.Add(fLayoutPanel);
        }

        public override void SetExtData(object extData)
        {
            fAquarium = (Aquarium)extData;
            UpdateContent();
        }

        public override void UpdateContent()
        {
            fLayoutPanel.Controls.Clear();
            if (fModel == null) return;

            if (fAquarium != null) {
                fLayoutPanel.SuspendLayout();

                fLayoutPanel.RowCount = 1;
                int col = 0, row = 0;
                var values = fModel.CollectData(fAquarium);
                foreach (var mVal in values) {
                    if (!double.IsNaN(mVal.Value) && mVal.Ranges != null) {
                        string title = mVal.Name;
                        if (!string.IsNullOrEmpty(mVal.Unit)) {
                            title += ", " + mVal.Unit;
                        }

                        var qCtl = new QualityControl();
                        qCtl.Margin = new Padding(LayoutPadding);
                        qCtl.Dock = DockStyle.Top;
                        qCtl.Anchor = AnchorStyles.Left;
                        qCtl.Ranges = mVal.Ranges;
                        qCtl.Value = mVal.Value;
                        qCtl.Title = title;
                        qCtl.Width = fLayoutPanel.ClientSize.Width - LayoutPadding * 2;

                        fLayoutPanel.Controls.Add(qCtl, col, row);
                        if (col == 0) {
                            col += 1;
                        } else {
                            col = 0;
                            row += 1;
                            fLayoutPanel.RowCount += 1;
                        }
                    }
                }

                fLayoutPanel.ResumeLayout();
            }
        }
    }
}
