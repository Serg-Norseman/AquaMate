/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    public partial class SpeciesEditDlg : EditDialog, ISpeciesEditorView
    {
        private readonly SpeciesEditorPresenter fPresenter;

        public SpeciesEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new SpeciesEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.SpeciesS);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Name);
            lblDesc.Text = Localizer.LS(LSID.Description);
            lblType.Text = Localizer.LS(LSID.Type);
            lblScientificName.Text = Localizer.LS(LSID.ScientificName);
            lblFamily.Text = Localizer.LS(LSID.BioFamily);

            /*tabFish.Text = Localizer.LS(LSID.Fish);
            tabInvertebrate.Text = Localizer.LS(LSID.Invertebrate);
            tabPlant.Text = Localizer.LS(LSID.Plant);
            tabCoral.Text = Localizer.LS(LSID.Coral);*/

            lblAdultSize.Text = Localizer.LS(LSID.AdultSize);
            lblLifeSpan.Text = Localizer.LS(LSID.LifeSpan);
            lblSwimLevel.Text = Localizer.LS(LSID.SwimLevel);
            lblDistribution.Text = Localizer.LS(LSID.Distribution);
            lblHabitat.Text = Localizer.LS(LSID.Habitat);
            lblCareLevel.Text = Localizer.LS(LSID.CareLevel);
            lblTemperament.Text = Localizer.LS(LSID.Temperament);
        }

        public void SetContext(IModel model, Species record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fPresenter.ChangeSelectedType();
        }

        #region View interface implementation

        ITextBox ISpeciesEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        ITextBox ISpeciesEditorView.DescriptionField
        {
            get { return GetControlHandler<ITextBox>(txtDesc); }
        }

        IComboBox ISpeciesEditorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbType); }
        }

        ITextBox ISpeciesEditorView.ScientificNameField
        {
            get { return GetControlHandler<ITextBox>(txtScientificName); }
        }

        IComboBox ISpeciesEditorView.BioFamilyCombo
        {
            get { return GetControlHandler<IComboBox>(cmbFamily); }
        }

        ITextBox ISpeciesEditorView.TempMinField
        {
            get { return GetControlHandler<ITextBox>(txtTempMin); }
        }

        ITextBox ISpeciesEditorView.TempMaxField
        {
            get { return GetControlHandler<ITextBox>(txtTempMax); }
        }

        ITextBox ISpeciesEditorView.PHMinField
        {
            get { return GetControlHandler<ITextBox>(txtPHMin); }
        }

        ITextBox ISpeciesEditorView.PHMaxField
        {
            get { return GetControlHandler<ITextBox>(txtPHMax); }
        }

        ITextBox ISpeciesEditorView.GHMinField
        {
            get { return GetControlHandler<ITextBox>(txtGHMin); }
        }

        ITextBox ISpeciesEditorView.GHMaxField
        {
            get { return GetControlHandler<ITextBox>(txtGHMax); }
        }

        ITextBox ISpeciesEditorView.AdultSizeField
        {
            get { return GetControlHandler<ITextBox>(txtAdultSize); }
        }

        ITextBox ISpeciesEditorView.LifeSpanField
        {
            get { return GetControlHandler<ITextBox>(txtLifeSpan); }
        }

        IComboBox ISpeciesEditorView.SwimLevelCombo
        {
            get { return GetControlHandler<IComboBox>(cmbSwimLevel); }
        }

        IComboBox ISpeciesEditorView.DistributionCombo
        {
            get { return GetControlHandler<IComboBox>(cmbDistribution); }
        }

        IComboBox ISpeciesEditorView.HabitatCombo
        {
            get { return GetControlHandler<IComboBox>(cmbHabitat); }
        }

        IComboBox ISpeciesEditorView.CareLevelCombo
        {
            get { return GetControlHandler<IComboBox>(cmbCareLevel); }
        }

        IComboBox ISpeciesEditorView.TemperamentCombo
        {
            get { return GetControlHandler<IComboBox>(cmbTemperament); }
        }

        #endregion
    }
}
