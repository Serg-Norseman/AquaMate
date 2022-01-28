/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Linq;
using AquaMate.Core;
using AquaMate.Core.Types;
using AquaMate.DataCollection;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface ISettingsDialogView : IDialogView<IModel>
    {
        ALSettings Settings { get; set; }

        ICheckBox AutorunCheck { get; }
        ICheckBox HideClosedTanksCheck { get; }
        ICheckBox ExitOnCloseCheck { get; }
        ICheckBox HideAtStartupCheck { get; }
        IComboBox LocaleCombo { get; }
        ICheckBox HideLossesCheck { get; }

        IComboBox LengthUoMCombo { get; }
        IComboBox VolumeUoMCombo { get; }
        IComboBox MassUoMCombo { get; }
        IComboBox TemperatureUoMCombo { get; }

        ICheckBox ChannelEnabledCheck { get; }
        IComboBox ChannelNameCombo { get; }
        IComboBox ChannelParametersCombo { get; }

        void SelectTab(int tabIndex);
    }


    /// <summary>
    /// 
    /// </summary>
    public class SettingsDialogPresenter : Presenter<ISettingsDialogView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "SettingsDialogPresenter");

        private ALSettings fSettings;

        public ALSettings Settings
        {
            get { return fSettings; }
            set {
                if (fSettings != value) {
                    fSettings = value;
                    UpdateView();
                }
            }
        }


        public SettingsDialogPresenter(ISettingsDialogView view) : base(view)
        {
            foreach (var locale in Localizer.Locales) {
                fView.LocaleCombo.Items.Add(locale);
            }

            for (MeasurementUnit unit = MeasurementUnit.First; unit <= MeasurementUnit.Last; unit++) {
                var uom = ALData.MeasurementUnits[(int)unit];

                if (uom.MeasurementType == MeasurementType.Length) {
                    fView.LengthUoMCombo.AddItem<MeasurementUnit>(uom.StrName, unit);
                }
                if (uom.MeasurementType == MeasurementType.Volume) {
                    if (unit == MeasurementUnit.UKGallon) continue; // TODO: UKGallon not yet supported

                    fView.VolumeUoMCombo.AddItem<MeasurementUnit>(uom.StrName, unit);
                }
                if (uom.MeasurementType == MeasurementType.Mass) {
                    fView.MassUoMCombo.AddItem<MeasurementUnit>(uom.StrName, unit);
                }
                if (uom.MeasurementType == MeasurementType.Temperature) {
                    fView.TemperatureUoMCombo.AddItem<MeasurementUnit>(uom.StrName, unit);
                }
            }

            // TODO: UOM changes not yet supported
            fView.LengthUoMCombo.Enabled = false;
            fView.VolumeUoMCombo.Enabled = false;
            fView.MassUoMCombo.Enabled = false;
            fView.TemperatureUoMCombo.Enabled = false;

            fView.ChannelNameCombo.AddRange(BaseChannel.ChannelNames);
        }

        public override void UpdateView()
        {
            if (fSettings != null) {
                fView.HideClosedTanksCheck.Checked = fSettings.HideClosedTanks;
                fView.ExitOnCloseCheck.Checked = fSettings.ExitOnClose;
                fView.HideAtStartupCheck.Checked = fSettings.HideAtStartup;
                fView.HideLossesCheck.Checked = fSettings.HideLosses;

                fView.LocaleCombo.SelectedItem = Localizer.Locales.FirstOrDefault(loc => loc.Code == fSettings.CurrentLocale);

                fView.LengthUoMCombo.SetSelectedTag<MeasurementUnit>(fSettings.LengthUoM);
                fView.VolumeUoMCombo.SetSelectedTag<MeasurementUnit>(fSettings.VolumeUoM);
                fView.MassUoMCombo.SetSelectedTag<MeasurementUnit>(fSettings.MassUoM);
                fView.TemperatureUoMCombo.SetSelectedTag<MeasurementUnit>(fSettings.TemperatureUoM);

                fView.ChannelEnabledCheck.Checked = fSettings.ChannelEnabled;
                fView.ChannelNameCombo.Text = fSettings.ChannelName;
                fView.ChannelParametersCombo.Text = fSettings.ChannelParameters;
            }

            fView.AutorunCheck.Checked = AppHost.IsStartupItem();
        }

        public bool ApplyChanges()
        {
            try {
                fSettings.HideClosedTanks = fView.HideClosedTanksCheck.Checked;
                fSettings.ExitOnClose = fView.ExitOnCloseCheck.Checked;
                fSettings.HideAtStartup = fView.HideAtStartupCheck.Checked;
                fSettings.HideLosses = fView.HideLossesCheck.Checked;

                LocaleFile currentLocale = (fView.LocaleCombo.SelectedItem as LocaleFile);
                fSettings.CurrentLocale = (currentLocale != null) ? currentLocale.Code : Localizer.LS_DEF_CODE;

                fSettings.LengthUoM = fView.LengthUoMCombo.GetSelectedTag<MeasurementUnit>();
                fSettings.VolumeUoM = fView.VolumeUoMCombo.GetSelectedTag<MeasurementUnit>();
                fSettings.MassUoM = fView.MassUoMCombo.GetSelectedTag<MeasurementUnit>();
                fSettings.TemperatureUoM = fView.TemperatureUoMCombo.GetSelectedTag<MeasurementUnit>();

                fSettings.ChannelEnabled = fView.ChannelEnabledCheck.Checked;
                fSettings.ChannelName = fView.ChannelNameCombo.Text;
                fSettings.ChannelParameters = fView.ChannelParametersCombo.Text;

                if (fView.AutorunCheck.Checked) {
                    AppHost.RegisterStartup();
                } else {
                    AppHost.UnregisterStartup();
                }

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
