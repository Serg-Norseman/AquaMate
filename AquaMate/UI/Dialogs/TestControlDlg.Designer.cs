using System.Drawing;
using System.Windows.Forms;

namespace AquaMate.UI.Dialogs
{
    partial class TestControlDlg
    {
        private SplitContainer splitContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton btnAddBrand;
        private ToolStripButton btnDeleteBrand;
        private DataGridView dgvAccuracy;
        private DataGridView dgvMeasurements;
        private ToolStrip toolStrip2;
        private ToolStripButton btnCalculate;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel toolStripLabel2;
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestControlDlg));
            splitContainer1 = new SplitContainer();
            dgvMeasurements = new DataGridView();
            toolStrip2 = new ToolStrip();
            btnCalculate = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            dgvAccuracy = new DataGridView();
            toolStrip1 = new ToolStrip();
            btnAddBrand = new ToolStripButton();
            btnDeleteBrand = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMeasurements).BeginInit();
            toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccuracy).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvMeasurements);
            splitContainer1.Panel1.Controls.Add(toolStrip2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dgvAccuracy);
            splitContainer1.Panel2.Controls.Add(toolStrip1);
            splitContainer1.Size = new Size(1000, 573);
            splitContainer1.SplitterDistance = 228;
            splitContainer1.TabIndex = 0;
            // 
            // dgvMeasurements
            // 
            dgvMeasurements.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMeasurements.Dock = DockStyle.Fill;
            dgvMeasurements.Location = new Point(0, 25);
            dgvMeasurements.Name = "dgvMeasurements";
            dgvMeasurements.Size = new Size(1000, 203);
            dgvMeasurements.TabIndex = 2;
            // 
            // toolStrip2
            // 
            toolStrip2.Items.AddRange(new ToolStripItem[] { btnCalculate, toolStripSeparator1, toolStripLabel1 });
            toolStrip2.Location = new Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(1000, 25);
            toolStrip2.TabIndex = 1;
            toolStrip2.Text = "toolStrip2";
            // 
            // btnCalculate
            // 
            btnCalculate.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnCalculate.ImageTransparentColor = Color.Magenta;
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(48, 22);
            btnCalculate.Text = "Calculate";
            btnCalculate.Click += btnCalculate_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.ForeColor = SystemColors.HotTrack;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(100, 22);
            toolStripLabel1.Text = "Test Results";
            // 
            // dgvAccuracy
            // 
            dgvAccuracy.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAccuracy.Dock = DockStyle.Fill;
            dgvAccuracy.Location = new Point(0, 25);
            dgvAccuracy.Name = "dgvAccuracy";
            dgvAccuracy.Size = new Size(1000, 316);
            dgvAccuracy.TabIndex = 2;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { btnAddBrand, btnDeleteBrand, toolStripSeparator2, toolStripLabel2 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1000, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnAddBrand
            // 
            btnAddBrand.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnAddBrand.ImageTransparentColor = Color.Magenta;
            btnAddBrand.Name = "btnAddBrand";
            btnAddBrand.Size = new Size(99, 22);
            btnAddBrand.Text = "Add Brand";
            btnAddBrand.Click += btnAddBrand_Click;
            // 
            // btnDeleteBrand
            // 
            btnDeleteBrand.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDeleteBrand.ImageTransparentColor = Color.Magenta;
            btnDeleteBrand.Name = "btnDeleteBrand";
            btnDeleteBrand.Size = new Size(91, 22);
            btnDeleteBrand.Text = "Delete Brand";
            btnDeleteBrand.Click += btnDeleteBrand_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.ForeColor = SystemColors.HotTrack;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(147, 22);
            toolStripLabel2.Text = "Accuracy Coefficients";
            // 
            // TestControlDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 573);
            Controls.Add(splitContainer1);
            Name = "TestControlDlg";
            Text = "Test Control";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMeasurements).EndInit();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccuracy).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
        }
    }
}
