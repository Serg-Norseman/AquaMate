/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.UI.Dialogs;

namespace AquaMate.UI.Components
{
    /// <summary>
    /// ImageViewer with the pop-up panel.
    /// </summary>
    public class ImageViewer : UserControl
    {
        private readonly List<Button> fBtnsList;
        private int fCurrentIndex;
        private int fItemId;
        private ItemType fItemType;
        private IModel fModel;
        private readonly int fPixelSpeed;
        private IList<Snapshot> fSnapshots;


        public override Cursor Cursor
        {
            get {
                return base.Cursor;
            }
            set {
                base.Cursor = value;
                fPictureBox.Cursor = value;
                fButtonsPanel.Cursor = value;
            }
        }


        public ImageViewer()
        {
            InitializeComponent();

            fButtonsPanel.Height = 36;
            fButtonsPanel.Top = Height;

            fBtnsList = new List<Button>();
            fBtnsList.Add(fAddButton);
            fBtnsList.Add(fDeleteButton);
            fBtnsList.Add(fPrevButton);
            fBtnsList.Add(fNextButton);

            fPixelSpeed = 5;
            fSnapshots = new List<Snapshot>();
            fCurrentIndex = -1;

            fTimer.Stop();

            RedrawButtons();
        }

        private void RedrawButtons()
        {
            fButtonsPanel.Controls.Clear();
            if (fBtnsList == null || fBtnsList.Count <= 0) return;

            int buttonsWidth = 0;
            for (int i = 0, c = fBtnsList.Count; i < c; i++) {
                buttonsWidth += (i > 0) ? (8 + fBtnsList[i].Width) : fBtnsList[i].Width;
            }

            int startX = (fButtonsPanel.Width - buttonsWidth) / 2;
            int startY = (fButtonsPanel.Height - fBtnsList[0].Height) / 2;

            for (int i = 0, c = fBtnsList.Count; i < c; i++) {
                fBtnsList[i].Location = new Point(startX, startY);
                fButtonsPanel.Controls.Add(fBtnsList[i]);
                startX += fBtnsList[i].Width + 8;
            }
        }

        private void MoveSlidePanel(object sender, EventArgs e)
        {
            if (fButtonsPanel.Top <= Height - fButtonsPanel.Height)
                fTimer.Stop();
            else
                fButtonsPanel.Top -= (fButtonsPanel.Top - 5 > Height - fButtonsPanel.Height) ? fPixelSpeed : fButtonsPanel.Top - (Height - fButtonsPanel.Height);
        }

        private void PictureBox_MouseHover(object sender, EventArgs e)
        {
            CheckCursorPosition(sender, e);
        }

        private void ButtonsPanel_MouseHover(object sender, EventArgs e)
        {
            CheckCursorPosition(sender, e);
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            CheckCursorPosition(sender, e);
        }

        private void ButtonsPanel_MouseLeave(object sender, EventArgs e)
        {
            CheckCursorPosition(sender, e);
        }

        private void CheckCursorPosition(object sender, EventArgs e)
        {
            Point p = PointToClient(Cursor.Position);
            bool buf = (p.X <= 1 || p.Y <= 1 || p.X >= fPictureBox.Width || p.Y >= fPictureBox.Height - 1);
            if (!buf) {
                fTimer.Start();
                fTimer.Interval = 1;
            } else {
                fButtonsPanel.Top = Height;
                fTimer.Stop();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            fButtonsPanel.Width = Width;
            CheckCursorPosition(this, e);
            RedrawButtons();
        }

        public void SetRecord(IModel model, int itemId, ItemType itemType)
        {
            fModel = model;
            fItemId = itemId;
            fItemType = itemType;

            Enabled = (fItemId != 0);

            UpdateContent();
        }

        private void UpdateContent()
        {
            if (fModel != null && fItemId != 0 && fItemType != ItemType.None) {
                fSnapshots = fModel.QuerySnapshots(fItemId, (int)fItemType);
                fCurrentIndex = 0;
            } else {
                fSnapshots = new List<Snapshot>();
                fCurrentIndex = -1;
            }

            UpdateImage();
        }

        private void UpdateImage()
        {
            var record = (fCurrentIndex >= 0 && fCurrentIndex < fSnapshots.Count) ? fSnapshots[fCurrentIndex] : null;
            fPictureBox.Image = (record == null) ? null : UIHelper.ByteToImage(record.Image);
        }

        private void btnImageAdd_Click(object sender, EventArgs e)
        {
            var record = new Snapshot();

            using (var dlg = new SnapshotEditDlg()) {
                dlg.SetContext(fModel, record);

                if (dlg.ShowModal()) {
                    record.ItemId = fItemId;
                    record.ItemType = fItemType;

                    fModel.AddRecord(record);
                    UpdateContent();
                }
            }
        }

        private void btnImageDelete_Click(object sender, EventArgs e)
        {
            var record = (fCurrentIndex >= 0 && fCurrentIndex < fSnapshots.Count) ? fSnapshots[fCurrentIndex] : null;
            if (record == null) return;

            // FIXME
            bool hasLinks = fModel.CheckConstraints(record);
            if (hasLinks) {
                UIHelper.ShowWarning(Localizer.LS(LSID.RecordHasExternalLinks));
                return;
            }

            string recordName = fModel.GetEntityName(record);
            if (!UIHelper.ShowQuestionYN(string.Format(Localizer.LS(LSID.RecordDeleteQuery), recordName))) return;

            fModel.DeleteRecord(record);
            UpdateContent();
        }

        private void btnImagePrev_Click(object sender, EventArgs e)
        {
            if (fCurrentIndex == 0) {
                fCurrentIndex = fSnapshots.Count - 1;
            } else {
                fCurrentIndex--;
            }

            UpdateImage();
        }

        private void btnImageNext_Click(object sender, EventArgs e)
        {
            if (fCurrentIndex < fSnapshots.Count - 1) {
                fCurrentIndex++;
            } else {
                fCurrentIndex = 0;
            }

            UpdateImage();
        }

        #region Design

        private IContainer fComponents;
        private PictureBox fPictureBox;
        private Panel fButtonsPanel;
        private Timer fTimer;
        private Button fAddButton;
        private Button fDeleteButton;
        private Button fPrevButton;
        private Button fNextButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (fComponents != null) {
                    fComponents.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            fPictureBox = new PictureBox();
            fPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            fPictureBox.Dock = DockStyle.Fill;
            fPictureBox.TabIndex = 0;
            fPictureBox.TabStop = false;
            fPictureBox.MouseLeave += PictureBox_MouseLeave;
            fPictureBox.MouseHover += PictureBox_MouseHover;

            fButtonsPanel = new Panel();
            fButtonsPanel.BackColor = SystemColors.ButtonShadow;
            fButtonsPanel.Location = new Point(0, 152);
            fButtonsPanel.Name = "panel1";
            fButtonsPanel.Size = new Size(178, 36);
            fButtonsPanel.TabIndex = 1;
            fButtonsPanel.MouseLeave += ButtonsPanel_MouseLeave;
            fButtonsPanel.MouseHover += ButtonsPanel_MouseHover;

            fAddButton = new Button();
            fAddButton.Location = new Point(618, 148);
            fAddButton.Margin = new Padding(2);
            fAddButton.Size = new Size(29, 29);
            fAddButton.TabStop = false;
            fAddButton.Click += btnImageAdd_Click;
            fAddButton.Image = UIHelper.LoadResourceImage("btn_rec_new.gif");

            fDeleteButton = new Button();
            fDeleteButton.Location = new Point(654, 148);
            fDeleteButton.Margin = new Padding(2);
            fDeleteButton.Size = new Size(29, 29);
            fDeleteButton.TabStop = false;
            fDeleteButton.Click += btnImageDelete_Click;
            fDeleteButton.Image = UIHelper.LoadResourceImage("btn_rec_delete.gif");

            fPrevButton = new Button();
            fPrevButton.Location = new Point(654, 148);
            fPrevButton.Margin = new Padding(2);
            fPrevButton.Size = new Size(29, 29);
            fPrevButton.TabStop = false;
            fPrevButton.Click += btnImagePrev_Click;
            fPrevButton.Image = UIHelper.LoadResourceImage("btn_left.gif");

            fNextButton = new Button();
            fNextButton.Location = new Point(654, 148);
            fNextButton.Margin = new Padding(2);
            fNextButton.Size = new Size(29, 29);
            fNextButton.TabStop = false;
            fNextButton.Click += btnImageNext_Click;
            fNextButton.Image = UIHelper.LoadResourceImage("btn_right.gif");

            fComponents = new System.ComponentModel.Container();
            fTimer = new Timer(fComponents);
            fTimer.Tick += MoveSlidePanel;

            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(fButtonsPanel);
            Controls.Add(fPictureBox);
            Size = new Size(178, 188);
            ResumeLayout(false);
        }

        #endregion
    }
}
