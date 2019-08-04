﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Linq;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.TSDB;

namespace AquaLog.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TSPointEditDlg : Form
    {
        private TSDatabase fModel;
        private TSPoint fPoint;

        public TSDatabase Model
        {
            get { return fModel; }
            set { fModel = value; }
        }

        public TSPoint Point
        {
            get { return fPoint; }
            set {
                if (fPoint != value) {
                    fPoint = value;
                    UpdateView();
                }
            }
        }


        public TSPointEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = ALCore.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = ALCore.LoadResourceImage("btn_cancel.gif");
        }

        private void UpdateView()
        {
            if (fPoint != null) {
                txtName.Text = fPoint.Name;
                txtUoM.Text = fPoint.MeasureUnit;
                txtMin.Text = ALCore.GetDecimalStr(fPoint.Min);
                txtMax.Text = ALCore.GetDecimalStr(fPoint.Max);
                txtDeviation.Text = ALCore.GetDecimalStr(fPoint.Deviation);
            }
        }

        private void ApplyChanges()
        {
            fPoint.Name = txtName.Text;
            fPoint.MeasureUnit = txtUoM.Text;
            fPoint.Min = (float)ALCore.GetDecimalVal(txtMin);
            fPoint.Max = (float)ALCore.GetDecimalVal(txtMax);
            fPoint.Deviation = (float)ALCore.GetDecimalVal(txtDeviation);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try {
                ApplyChanges();
                DialogResult = DialogResult.OK;
            } catch {
                DialogResult = DialogResult.None;
            }
        }
    }
}