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
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    public partial class SettingsDlg : CommonForm, ISettingsDialogView
    {
        private readonly SettingsDialogPresenter fPresenter;


        public ALSettings Settings
        {
            get { return fPresenter.Settings; }
            set { fPresenter.Settings = value; }
        }


        public SettingsDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new SettingsDialogPresenter(this);

            SetLocale();
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Settings);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            tabCommon.Text = Localizer.LS(LSID.Common);
            chkHideClosedTanks.Text = Localizer.LS(LSID.HideClosedTanks);
            chkExitOnClose.Text = Localizer.LS(LSID.ExitOnClose);
            lblLocale.Text = Localizer.LS(LSID.Language);
            chkAutorun.Text = Localizer.LS(LSID.Autorun);
            chkHideAtStartup.Text = Localizer.LS(LSID.HideAtStartup);

            tabData.Text = Localizer.LS(LSID.Data);
            lblLengthUoM.Text = Localizer.LS(LSID.Length);
            lblVolumeUoM.Text = Localizer.LS(LSID.Volume);
            lblMassUoM.Text = Localizer.LS(LSID.Mass);
            lblTemperatureUoM.Text = Localizer.LS(LSID.Temperature);
            chkHideLosses.Text = Localizer.LS(LSID.HideLosses);

            tabDAS.Text = Localizer.LS(LSID.DataAcquisition);
            lblChannel.Text = Localizer.LS(LSID.Channel);
            lblParameters.Text = Localizer.LS(LSID.Parameters);
            chkEnabled.Text = Localizer.LS(LSID.Enabled);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        public bool ShowModal()
        {
            return (base.ShowDialog() == DialogResult.OK);
        }

        public void SelectTab(int tabIndex)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[tabIndex];
        }

        #region View interface implementation

        ICheckBox ISettingsDialogView.AutorunCheck
        {
            get { return GetControlHandler<ICheckBox>(chkAutorun); }
        }

        ICheckBox ISettingsDialogView.HideClosedTanksCheck
        {
            get { return GetControlHandler<ICheckBox>(chkHideClosedTanks); }
        }

        ICheckBox ISettingsDialogView.ExitOnCloseCheck
        {
            get { return GetControlHandler<ICheckBox>(chkExitOnClose); }
        }

        ICheckBox ISettingsDialogView.HideAtStartupCheck
        {
            get { return GetControlHandler<ICheckBox>(chkHideAtStartup); }
        }

        IComboBox ISettingsDialogView.LocaleCombo
        {
            get { return GetControlHandler<IComboBox>(cmbLocale); }
        }

        ICheckBox ISettingsDialogView.HideLossesCheck
        {
            get { return GetControlHandler<ICheckBox>(chkHideLosses); }
        }


        IComboBox ISettingsDialogView.LengthUoMCombo
        {
            get { return GetControlHandler<IComboBox>(cmbLengthUoM); }
        }

        IComboBox ISettingsDialogView.VolumeUoMCombo
        {
            get { return GetControlHandler<IComboBox>(cmbVolumeUoM); }
        }

        IComboBox ISettingsDialogView.MassUoMCombo
        {
            get { return GetControlHandler<IComboBox>(cmbMassUoM); }
        }

        IComboBox ISettingsDialogView.TemperatureUoMCombo
        {
            get { return GetControlHandler<IComboBox>(cmbTemperatureUoM); }
        }

        ICheckBox ISettingsDialogView.ChannelEnabledCheck
        {
            get { return GetControlHandler<ICheckBox>(chkEnabled); }
        }

        IComboBox ISettingsDialogView.ChannelNameCombo
        {
            get { return GetControlHandler<IComboBox>(cmbChannel); }
        }

        IComboBox ISettingsDialogView.ChannelParametersCombo
        {
            get { return GetControlHandler<IComboBox>(cmbParameters); }
        }

        #endregion
    }
}
