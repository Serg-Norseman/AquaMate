/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MeasureEditDlg : EditDialog, IMeasureEditorView
    {
        private readonly MeasureEditorPresenter fPresenter;

        public MeasureEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new MeasureEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Measure);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblAquarium.Text = Localizer.LS(LSID.Aquarium);
            lblTimestamp.Text = Localizer.LS(LSID.Timestamp);
            lblTemperature.Text = ALData.GetLSuom(LSID.Temperature, MeasurementType.Temperature);
        }

        public void SetContext(IModel model, Measure record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void btnCalcCO2_Click(object sender, EventArgs e)
        {
            fPresenter.CalcCO2();
        }

        private void btnCalcNH3_Click(object sender, EventArgs e)
        {
            fPresenter.CalcNH3();
        }

        private void btnCalcNH4_Click(object sender, EventArgs e)
        {
            fPresenter.CalcNH4();
        }

        #region View interface implementation

        IComboBox IMeasureEditorView.AquariumCombo
        {
            get { return GetControlHandler<IComboBox>(cmbAquarium); }
        }

        IDateTimeBox IMeasureEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpTimestamp); }
        }

        ITextBox IMeasureEditorView.TemperatureField
        {
            get { return GetControlHandler<ITextBox>(txtTemperature); }
        }

        ITextBox IMeasureEditorView.NO3Field
        {
            get { return GetControlHandler<ITextBox>(txtNO3); }
        }

        ITextBox IMeasureEditorView.NO2Field
        {
            get { return GetControlHandler<ITextBox>(txtNO2); }
        }

        ITextBox IMeasureEditorView.GHField
        {
            get { return GetControlHandler<ITextBox>(txtGH); }
        }

        ITextBox IMeasureEditorView.KHField
        {
            get { return GetControlHandler<ITextBox>(txtKH); }
        }

        ITextBox IMeasureEditorView.PHField
        {
            get { return GetControlHandler<ITextBox>(txtPH); }
        }

        ITextBox IMeasureEditorView.Cl2Field
        {
            get { return GetControlHandler<ITextBox>(txtCl2); }
        }

        ITextBox IMeasureEditorView.CO2Field
        {
            get { return GetControlHandler<ITextBox>(txtCO2); }
        }

        ITextBox IMeasureEditorView.NHField
        {
            get { return GetControlHandler<ITextBox>(txtNHtot); }
        }

        ITextBox IMeasureEditorView.NH3Field
        {
            get { return GetControlHandler<ITextBox>(txtNH3); }
        }

        ITextBox IMeasureEditorView.NH4Field
        {
            get { return GetControlHandler<ITextBox>(txtNH4); }
        }

        ITextBox IMeasureEditorView.PO4Field
        {
            get { return GetControlHandler<ITextBox>(txtPO4); }
        }

        #endregion
    }
}
