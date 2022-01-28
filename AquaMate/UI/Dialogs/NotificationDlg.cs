/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core.Model;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationDlg : Form
    {
        private const int LayoutPadding = 10;

        private readonly FlowLayoutPanel fLayoutPanel;
        private readonly IBrowser fBrowser;
        private readonly Font fTextFont;


        public NotificationDlg(IBrowser browser)
        {
            SuspendLayout();

            ClientSize = new Size(200, 80);
            ShowInTaskbar = false;
            ShowIcon = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Text = "Notification";
            TopMost = true;
            Load += NotificationDlg_Load;
            FormClosing += NotificationDlg_FormClosing;

            fBrowser = browser;
            fTextFont = new Font(Font, FontStyle.Bold);

            fLayoutPanel = new FlowLayoutPanel();
            fLayoutPanel.Dock = DockStyle.Fill;
            fLayoutPanel.Padding = new Padding(LayoutPadding);
            fLayoutPanel.FlowDirection = FlowDirection.TopDown;
            fLayoutPanel.WrapContents = false;
            fLayoutPanel.AutoScroll = true;
            fLayoutPanel.SizeChanged += FlowLayoutPanel_SizeChanged;
            Controls.Add(fLayoutPanel);

            ResumeLayout();
        }

        private void NotificationDlg_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            int left = workingArea.Width - Width;
            int top = workingArea.Height - Height;
            Location = new Point(left, top);
        }

        private void NotificationDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void FlowLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            int innerWidth = fLayoutPanel.ClientSize.Width - LayoutPadding * 2;
            foreach (Control ctrl in fLayoutPanel.Controls) {
                ctrl.Width = innerWidth;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            Schedule scheduleRecord = lbl.Tag as Schedule;

            fLayoutPanel.Controls.Remove(lbl);
            if (fLayoutPanel.Controls.Count == 0) {
                Hide();
            }

            fBrowser.AddMaintenance(scheduleRecord);
        }

        public void Notify(string text, Schedule record)
        {
            Show();

            foreach (Control ctrl in fLayoutPanel.Controls) {
                if (ctrl.Tag == record) {
                    return;
                }
            }

            fLayoutPanel.SuspendLayout();
            var lbl = new Label();
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Margin = new Padding(0, 0, 0, LayoutPadding);
            lbl.Dock = DockStyle.Top;
            lbl.Anchor = AnchorStyles.Left;
            lbl.Text = text;
            lbl.Tag = record;
            lbl.Font = fTextFont;
            lbl.Width = fLayoutPanel.ClientSize.Width - LayoutPadding * 2;
            lbl.Click += Label_Click;
            fLayoutPanel.Controls.Add(lbl);
            fLayoutPanel.ResumeLayout();
        }
    }
}
