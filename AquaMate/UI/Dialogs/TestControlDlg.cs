/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using AquaMate.Tests;
using AquaMate.UI.Panels;
using BSLib;

namespace AquaMate.UI.Dialogs
{
    public partial class TestControlDlg : Form
    {
        private BindingList<TestAccuracy> _accuracyMatrix;
        private readonly List<string> _parameters = new List<string> { "NO3", "NO2", "GH", "KH", "PH", "PO4", "NH3/4" };
        private readonly MeasurePanel fMeasurePanel;

        public TestControlDlg(MeasurePanel measurePanel)
        {
            fMeasurePanel = measurePanel;

            InitializeComponent();
            LoadData();
            SetupMeasurementsGrid();
            SetupAccuracyGrid();
        }

        private void LoadData()
        {
            _accuracyMatrix = TestRepository.LoadMatrix();
            _accuracyMatrix.ListChanged += AccuracyMatrix_ListChanged;
        }

        private void SetupMeasurementsGrid()
        {
            dgvMeasurements.Columns.Clear();

            var comboColumn = new DataGridViewComboBoxColumn {
                Name = "Brand",
                HeaderText = "Test",
                DataSource = _accuracyMatrix,
                DisplayMember = "TestName",
                ValueMember = "TestName",
                FlatStyle = FlatStyle.Flat,
                Width = 150
            };
            dgvMeasurements.Columns.Add(comboColumn);

            foreach (var param in _parameters) {
                var textColumn = new DataGridViewTextBoxColumn() {
                    Name = param,
                    HeaderText = param,
                };
                textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvMeasurements.Columns.Add(textColumn);
            }

            dgvMeasurements.Rows.Add("Sera (strips)");
            dgvMeasurements.Rows.Add("Õ»Àœ¿ (drops)");
            dgvMeasurements.Rows.Add("UHE (drops)");

            int totalRowIndex = dgvMeasurements.Rows.Add();
            var textCell = new DataGridViewTextBoxCell { Value = "Weighted average" };
            dgvMeasurements.Rows[totalRowIndex].Cells[0] = textCell;

            dgvMeasurements.Rows[totalRowIndex].ReadOnly = true;
            dgvMeasurements.Rows[totalRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
            dgvMeasurements.Rows[totalRowIndex].DefaultCellStyle.Font = new System.Drawing.Font(dgvMeasurements.Font, System.Drawing.FontStyle.Bold);
        }

        private void SetupAccuracyGrid()
        {
            dgvAccuracy.DataSource = _accuracyMatrix;
            dgvAccuracy.Columns["TestName"].HeaderText = "Brand (Test)";
            dgvAccuracy.Columns["NH3_NH4"].HeaderText = "NH3/4";

            foreach (DataGridViewColumn column in dgvAccuracy.Columns) {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void AccuracyMatrix_ListChanged(object sender, ListChangedEventArgs e)
        {
            TestRepository.SaveMatrix(_accuracyMatrix);

            if (dgvMeasurements.Columns.Contains("Brand")) {
                dgvMeasurements.InvalidateColumn(0);
            }
        }

        private void btnAddBrand_Click(object sender, EventArgs e)
        {
            string newBrandName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the name of the new brand/test:",
                "Add Test",
                "New Test");

            if (!string.IsNullOrWhiteSpace(newBrandName)) {
                _accuracyMatrix.Add(new TestAccuracy { TestName = newBrandName });
            }
        }

        private void btnDeleteBrand_Click(object sender, EventArgs e)
        {
            if (dgvAccuracy.CurrentRow != null && !dgvAccuracy.CurrentRow.IsNewRow) {
                var result = MessageBox.Show(
                    "Are you sure you want to delete the selected test? It will also disappear from the dropdown lists in the upper table.",
                    "Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) {
                    var itemToRemove = (TestAccuracy)dgvAccuracy.CurrentRow.DataBoundItem;

                    // Before deleting the brand, clear references to it in the upper table to prevent WinForms from throwing DataError
                    for (int row = 0; row < dgvMeasurements.Rows.Count - 1; row++) {
                        if (dgvMeasurements.Rows[row].Cells[0].Value?.ToString() == itemToRemove.TestName) {
                            dgvMeasurements.Rows[row].Cells[0].Value = null;
                        }
                    }

                    _accuracyMatrix.Remove(itemToRemove);
                }
            } else {
                MessageBox.Show("Please select a row to delete.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Dictionary<string, object> fParameters = new Dictionary<string, object>();

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            fParameters.Clear();
            int totalRowIndex = dgvMeasurements.Rows.Count - 2;

            for (int col = 1; col < dgvMeasurements.Columns.Count; col++) {
                string paramName = dgvMeasurements.Columns[col].Name;
                var measurements = new List<CurrentMeasurement>();

                for (int row = 0; row < totalRowIndex; row++) {
                    string brand = dgvMeasurements.Rows[row].Cells[0].Value?.ToString();
                    var cellValue = dgvMeasurements.Rows[row].Cells[col].Value;

                    if (cellValue != null) {
                        double val = ConvertHelper.ParseFloat(cellValue.ToString(), double.NaN, true);
                        measurements.Add(new CurrentMeasurement { TestName = brand, Value = val });
                    }
                }

                var avg = TestRepository.CalculateWeightedAverage(measurements, _accuracyMatrix, paramName);
                dgvMeasurements.Rows[totalRowIndex].Cells[col].Value = avg.HasValue ? avg.Value.ToString() : "-";

                fParameters.Add(paramName, avg);
            }
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            Close();
            fMeasurePanel.AddRecord(fParameters);
        }
    }
}
