﻿namespace AquaLog.UI.Dialogs
{
    partial class AquariumEditDlg
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblShape;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCommon;
        private System.Windows.Forms.TabPage tabTank;
        private System.Windows.Forms.TextBox txtTankVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.DateTimePicker dtpStopDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblStopDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.ComboBox cmbWaterType;
        private System.Windows.Forms.Label lblWaterType;
        private System.Windows.Forms.TextBox txtWaterVolume;
        private System.Windows.Forms.Label lblWaterVolume;
        private System.Windows.Forms.PropertyGrid pgProps;
        private System.Windows.Forms.Button btnTank;
        private System.Windows.Forms.TextBox txtSoilHeight;
        private System.Windows.Forms.TextBox txtUnderfillHeight;
        private System.Windows.Forms.Label lblSoilHeight;
        private System.Windows.Forms.Label lblUnderfillHeight;
        private System.Windows.Forms.TextBox txtSoilVolume;
        private System.Windows.Forms.Label lblSoilVolume;
        private System.Windows.Forms.TextBox txtSoilMass;
        private System.Windows.Forms.Label lblSoilMass;
        private System.Windows.Forms.ComboBox cmbBrand;
        private System.Windows.Forms.Label lblBrand;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblShape = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCommon = new System.Windows.Forms.TabPage();
            this.cmbWaterType = new System.Windows.Forms.ComboBox();
            this.lblWaterType = new System.Windows.Forms.Label();
            this.dtpStopDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStopDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.tabTank = new System.Windows.Forms.TabPage();
            this.btnTank = new System.Windows.Forms.Button();
            this.pgProps = new System.Windows.Forms.PropertyGrid();
            this.txtSoilHeight = new System.Windows.Forms.TextBox();
            this.txtUnderfillHeight = new System.Windows.Forms.TextBox();
            this.lblSoilHeight = new System.Windows.Forms.Label();
            this.txtSoilMass = new System.Windows.Forms.TextBox();
            this.txtSoilVolume = new System.Windows.Forms.TextBox();
            this.txtWaterVolume = new System.Windows.Forms.TextBox();
            this.lblSoilMass = new System.Windows.Forms.Label();
            this.lblUnderfillHeight = new System.Windows.Forms.Label();
            this.lblSoilVolume = new System.Windows.Forms.Label();
            this.txtTankVolume = new System.Windows.Forms.TextBox();
            this.lblWaterVolume = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.cmbBrand = new System.Windows.Forms.ComboBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabCommon.SuspendLayout();
            this.tabTank.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(316, 381);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(2);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(96, 24);
            this.btnAccept.TabIndex = 7;
            this.btnAccept.Text = "Accept";
            this.btnAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(415, 381);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(10, 12);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblShape
            // 
            this.lblShape.Location = new System.Drawing.Point(317, 14);
            this.lblShape.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblShape.Name = "lblShape";
            this.lblShape.Size = new System.Drawing.Size(80, 17);
            this.lblShape.TabIndex = 2;
            this.lblShape.Text = "Shape";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(10, 74);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 17);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(83, 10);
            this.txtName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(226, 22);
            this.txtName.TabIndex = 1;
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            this.cmbShape.Location = new System.Drawing.Point(375, 10);
            this.cmbShape.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.Size = new System.Drawing.Size(136, 21);
            this.cmbShape.TabIndex = 3;
            this.cmbShape.SelectedIndexChanged += new System.EventHandler(this.cmbShape_SelectedIndexChanged);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(83, 71);
            this.txtDesc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(428, 48);
            this.txtDesc.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCommon);
            this.tabControl1.Controls.Add(this.tabTank);
            this.tabControl1.Location = new System.Drawing.Point(10, 128);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(501, 244);
            this.tabControl1.TabIndex = 6;
            // 
            // tabCommon
            // 
            this.tabCommon.BackColor = System.Drawing.SystemColors.Control;
            this.tabCommon.Controls.Add(this.cmbWaterType);
            this.tabCommon.Controls.Add(this.lblWaterType);
            this.tabCommon.Controls.Add(this.dtpStopDate);
            this.tabCommon.Controls.Add(this.dtpStartDate);
            this.tabCommon.Controls.Add(this.lblStopDate);
            this.tabCommon.Controls.Add(this.lblStartDate);
            this.tabCommon.Location = new System.Drawing.Point(4, 22);
            this.tabCommon.Margin = new System.Windows.Forms.Padding(2);
            this.tabCommon.Name = "tabCommon";
            this.tabCommon.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.tabCommon.Size = new System.Drawing.Size(493, 214);
            this.tabCommon.TabIndex = 0;
            this.tabCommon.Text = "Common";
            // 
            // cmbWaterType
            // 
            this.cmbWaterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWaterType.FormattingEnabled = true;
            this.cmbWaterType.Location = new System.Drawing.Point(93, 40);
            this.cmbWaterType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.cmbWaterType.Name = "cmbWaterType";
            this.cmbWaterType.Size = new System.Drawing.Size(114, 21);
            this.cmbWaterType.TabIndex = 5;
            // 
            // lblWaterType
            // 
            this.lblWaterType.Location = new System.Drawing.Point(10, 42);
            this.lblWaterType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWaterType.Name = "lblWaterType";
            this.lblWaterType.Size = new System.Drawing.Size(80, 17);
            this.lblWaterType.TabIndex = 4;
            this.lblWaterType.Text = "Water type";
            // 
            // dtpStopDate
            // 
            this.dtpStopDate.Checked = false;
            this.dtpStopDate.Location = new System.Drawing.Point(341, 10);
            this.dtpStopDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.dtpStopDate.Name = "dtpStopDate";
            this.dtpStopDate.ShowCheckBox = true;
            this.dtpStopDate.Size = new System.Drawing.Size(143, 22);
            this.dtpStopDate.TabIndex = 3;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Checked = false;
            this.dtpStartDate.Location = new System.Drawing.Point(93, 10);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(143, 22);
            this.dtpStartDate.TabIndex = 1;
            // 
            // lblStopDate
            // 
            this.lblStopDate.Location = new System.Drawing.Point(256, 14);
            this.lblStopDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStopDate.Name = "lblStopDate";
            this.lblStopDate.Size = new System.Drawing.Size(80, 17);
            this.lblStopDate.TabIndex = 2;
            this.lblStopDate.Text = "Stop date";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(10, 14);
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(80, 17);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start date";
            // 
            // tabTank
            // 
            this.tabTank.BackColor = System.Drawing.SystemColors.Control;
            this.tabTank.Controls.Add(this.btnTank);
            this.tabTank.Controls.Add(this.pgProps);
            this.tabTank.Controls.Add(this.txtSoilHeight);
            this.tabTank.Controls.Add(this.txtUnderfillHeight);
            this.tabTank.Controls.Add(this.lblSoilHeight);
            this.tabTank.Controls.Add(this.txtSoilMass);
            this.tabTank.Controls.Add(this.txtSoilVolume);
            this.tabTank.Controls.Add(this.txtWaterVolume);
            this.tabTank.Controls.Add(this.lblSoilMass);
            this.tabTank.Controls.Add(this.lblUnderfillHeight);
            this.tabTank.Controls.Add(this.lblSoilVolume);
            this.tabTank.Controls.Add(this.txtTankVolume);
            this.tabTank.Controls.Add(this.lblWaterVolume);
            this.tabTank.Controls.Add(this.lblVolume);
            this.tabTank.Location = new System.Drawing.Point(4, 22);
            this.tabTank.Margin = new System.Windows.Forms.Padding(2);
            this.tabTank.Name = "tabTank";
            this.tabTank.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.tabTank.Size = new System.Drawing.Size(493, 218);
            this.tabTank.TabIndex = 1;
            this.tabTank.Text = "Tank";
            // 
            // btnTank
            // 
            this.btnTank.Location = new System.Drawing.Point(217, 184);
            this.btnTank.Name = "btnTank";
            this.btnTank.Size = new System.Drawing.Size(75, 23);
            this.btnTank.TabIndex = 14;
            this.btnTank.Text = "Tank...";
            this.btnTank.UseVisualStyleBackColor = true;
            this.btnTank.Click += new System.EventHandler(this.btnTank_Click);
            // 
            // pgProps
            // 
            this.pgProps.HelpVisible = false;
            this.pgProps.Location = new System.Drawing.Point(9, 9);
            this.pgProps.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.pgProps.Name = "pgProps";
            this.pgProps.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgProps.Size = new System.Drawing.Size(203, 198);
            this.pgProps.TabIndex = 13;
            this.pgProps.ToolbarVisible = false;
            // 
            // txtSoilHeight
            // 
            this.txtSoilHeight.Location = new System.Drawing.Point(361, 39);
            this.txtSoilHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtSoilHeight.Name = "txtSoilHeight";
            this.txtSoilHeight.Size = new System.Drawing.Size(82, 22);
            this.txtSoilHeight.TabIndex = 9;
            this.txtSoilHeight.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // txtUnderfillHeight
            // 
            this.txtUnderfillHeight.Location = new System.Drawing.Point(361, 9);
            this.txtUnderfillHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtUnderfillHeight.Name = "txtUnderfillHeight";
            this.txtUnderfillHeight.Size = new System.Drawing.Size(82, 22);
            this.txtUnderfillHeight.TabIndex = 9;
            this.txtUnderfillHeight.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblSoilHeight
            // 
            this.lblSoilHeight.Location = new System.Drawing.Point(223, 42);
            this.lblSoilHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSoilHeight.Name = "lblSoilHeight";
            this.lblSoilHeight.Size = new System.Drawing.Size(134, 17);
            this.lblSoilHeight.TabIndex = 8;
            this.lblSoilHeight.Text = "Soil height";
            // 
            // txtSoilMass
            // 
            this.txtSoilMass.Location = new System.Drawing.Point(361, 157);
            this.txtSoilMass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtSoilMass.Name = "txtSoilMass";
            this.txtSoilMass.Size = new System.Drawing.Size(82, 22);
            this.txtSoilMass.TabIndex = 9;
            // 
            // txtSoilVolume
            // 
            this.txtSoilVolume.Location = new System.Drawing.Point(361, 127);
            this.txtSoilVolume.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtSoilVolume.Name = "txtSoilVolume";
            this.txtSoilVolume.Size = new System.Drawing.Size(82, 22);
            this.txtSoilVolume.TabIndex = 9;
            // 
            // txtWaterVolume
            // 
            this.txtWaterVolume.Location = new System.Drawing.Point(361, 98);
            this.txtWaterVolume.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtWaterVolume.Name = "txtWaterVolume";
            this.txtWaterVolume.Size = new System.Drawing.Size(82, 22);
            this.txtWaterVolume.TabIndex = 9;
            // 
            // lblSoilMass
            // 
            this.lblSoilMass.Location = new System.Drawing.Point(223, 159);
            this.lblSoilMass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSoilMass.Name = "lblSoilMass";
            this.lblSoilMass.Size = new System.Drawing.Size(134, 17);
            this.lblSoilMass.TabIndex = 8;
            this.lblSoilMass.Text = "Soil mass";
            // 
            // lblUnderfillHeight
            // 
            this.lblUnderfillHeight.Location = new System.Drawing.Point(223, 12);
            this.lblUnderfillHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUnderfillHeight.Name = "lblUnderfillHeight";
            this.lblUnderfillHeight.Size = new System.Drawing.Size(134, 17);
            this.lblUnderfillHeight.TabIndex = 8;
            this.lblUnderfillHeight.Text = "Underfill height";
            // 
            // lblSoilVolume
            // 
            this.lblSoilVolume.Location = new System.Drawing.Point(223, 130);
            this.lblSoilVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSoilVolume.Name = "lblSoilVolume";
            this.lblSoilVolume.Size = new System.Drawing.Size(134, 17);
            this.lblSoilVolume.TabIndex = 8;
            this.lblSoilVolume.Text = "Soil volume";
            // 
            // txtTankVolume
            // 
            this.txtTankVolume.Location = new System.Drawing.Point(361, 68);
            this.txtTankVolume.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.txtTankVolume.Name = "txtTankVolume";
            this.txtTankVolume.Size = new System.Drawing.Size(82, 22);
            this.txtTankVolume.TabIndex = 7;
            this.txtTankVolume.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblWaterVolume
            // 
            this.lblWaterVolume.Location = new System.Drawing.Point(223, 101);
            this.lblWaterVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWaterVolume.Name = "lblWaterVolume";
            this.lblWaterVolume.Size = new System.Drawing.Size(134, 17);
            this.lblWaterVolume.TabIndex = 8;
            this.lblWaterVolume.Text = "Water volume";
            // 
            // lblVolume
            // 
            this.lblVolume.Location = new System.Drawing.Point(223, 71);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(134, 17);
            this.lblVolume.TabIndex = 6;
            this.lblVolume.Text = "Tank volume";
            // 
            // cmbBrand
            // 
            this.cmbBrand.Location = new System.Drawing.Point(83, 41);
            this.cmbBrand.Margin = new System.Windows.Forms.Padding(2, 2, 2, 7);
            this.cmbBrand.Name = "cmbBrand";
            this.cmbBrand.Size = new System.Drawing.Size(196, 21);
            this.cmbBrand.TabIndex = 10;
            // 
            // lblBrand
            // 
            this.lblBrand.Location = new System.Drawing.Point(10, 44);
            this.lblBrand.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(103, 17);
            this.lblBrand.TabIndex = 9;
            this.lblBrand.Text = "Brand";
            // 
            // AquariumEditDlg
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(522, 416);
            this.Controls.Add(this.cmbBrand);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.cmbShape);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblShape);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AquariumEditDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aquarium";
            this.tabControl1.ResumeLayout(false);
            this.tabCommon.ResumeLayout(false);
            this.tabTank.ResumeLayout(false);
            this.tabTank.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
