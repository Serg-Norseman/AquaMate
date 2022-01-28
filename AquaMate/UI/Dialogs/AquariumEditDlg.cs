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
    public partial class AquariumEditDlg : EditDialog, IAquariumEditorView
    {
        private readonly AquariumEditorPresenter fPresenter;

        public AquariumEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new AquariumEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Aquarium);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            tabCommon.Text = Localizer.LS(LSID.Common);
            tabTank.Text = Localizer.LS(LSID.Tank);

            lblName.Text = Localizer.LS(LSID.Name);
            lblDesc.Text = Localizer.LS(LSID.Description);

            // deprecated
            //lblStartDate.Text = Localizer.LS(LSID.StartDate);
            //lblStopDate.Text = Localizer.LS(LSID.StopDate);

            lblShape.Text = Localizer.LS(LSID.Shape);
            lblWaterType.Text = Localizer.LS(LSID.WaterType);

            lblVolume.Text = ALData.GetLSuom(LSID.TankVolume, MeasurementType.Volume);
            lblWaterVolume.Text = ALData.GetLSuom(LSID.WaterVolume, MeasurementType.Volume);

            btnTank.Text = Localizer.LS(LSID.Tank) + "...";

            lblUnderfillHeight.Text = ALData.GetLSuom(LSID.UnderfillHeight, MeasurementType.Length);
            lblSoilHeight.Text = ALData.GetLSuom(LSID.SoilHeight, MeasurementType.Length);
            lblSoilVolume.Text = ALData.GetLSuom(LSID.SoilVolume, MeasurementType.Volume);
            lblSoilMass.Text = ALData.GetLSuom(LSID.SoilMass, MeasurementType.Mass);
            lblBrand.Text = Localizer.LS(LSID.Brand);
        }

        public void SetContext(IModel model, Aquarium record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void cmbShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            fPresenter.RefreshProps();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            fPresenter.RecalcValues();
        }

        private void btnTank_Click(object sender, EventArgs e)
        {
            fPresenter.EditTank();
        }

        #region View interface implementation

        ITextBox IAquariumEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IComboBox IAquariumEditorView.BrandCombo
        {
            get { return GetControlHandler<IComboBox>(cmbBrand); }
        }

        ITextBox IAquariumEditorView.DescriptionField
        {
            get { return GetControlHandler<ITextBox>(txtDesc); }
        }

        IComboBox IAquariumEditorView.ShapeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbShape); }
        }

        IComboBox IAquariumEditorView.WaterTypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbWaterType); }
        }

        /*IDateTimeBox IAquariumEditorView.StartDateField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpStartDate); } // deprecated
        }

        IDateTimeBox IAquariumEditorView.StopDateField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpStopDate); } // deprecated
        }*/

        ITextBox IAquariumEditorView.TankVolumeField
        {
            get { return GetControlHandler<ITextBox>(txtTankVolume); }
        }

        ITextBox IAquariumEditorView.UnderfillHeightField
        {
            get { return GetControlHandler<ITextBox>(txtUnderfillHeight); }
        }

        ITextBox IAquariumEditorView.SoilHeightField
        {
            get { return GetControlHandler<ITextBox>(txtSoilHeight); }
        }

        IPropertyGrid IAquariumEditorView.PropsGrid
        {
            get { return GetControlHandler<IPropertyGrid>(pgProps); }
        }

        ITextBox IAquariumEditorView.WaterVolumeField
        {
            get { return GetControlHandler<ITextBox>(txtWaterVolume); }
        }

        ITextBox IAquariumEditorView.SoilVolumeField
        {
            get { return GetControlHandler<ITextBox>(txtSoilVolume); }
        }

        ITextBox IAquariumEditorView.SoilMassField
        {
            get { return GetControlHandler<ITextBox>(txtSoilMass); }
        }

        #endregion
    }
}
