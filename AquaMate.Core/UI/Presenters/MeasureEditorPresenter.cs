/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IMeasureEditorView : IEditorView<Measure>
    {
        IComboBox AquariumCombo { get; }
        IDateTimeBox TimestampField { get; }
        ITextBox TemperatureField { get; }
        ITextBox NO3Field { get; }
        ITextBox NO2Field { get; }
        ITextBox GHField { get; }
        ITextBox KHField { get; }
        ITextBox PHField { get; }
        ITextBox Cl2Field { get; }
        ITextBox CO2Field { get; }
        ITextBox NHField { get; }
        ITextBox NH3Field { get; }
        ITextBox NH4Field { get; }
        ITextBox PO4Field { get; }
    }


    public class MeasureEditorPresenter : EditorPresenter<IModel, Measure, IMeasureEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "MeasureEditorPresenter");


        public MeasureEditorPresenter(IMeasureEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.AquariumCombo.AddRange(fModel.QueryAquariumsList(!fRecord.IsNewRecord));
                fView.AquariumCombo.SetSelectedTag(fRecord.AquariumId);

                if (!ALCore.IsZeroDate(fRecord.Timestamp)) {
                    fView.TimestampField.Value = fRecord.Timestamp;
                }

                fView.TemperatureField.SetDecimalVal(fRecord.Temperature);
                fView.NO3Field.SetDecimalVal(fRecord.NO3);
                fView.NO2Field.SetDecimalVal(fRecord.NO2);
                fView.GHField.SetDecimalVal(fRecord.GH);
                fView.KHField.SetDecimalVal(fRecord.KH);
                fView.PHField.SetDecimalVal(fRecord.pH);
                fView.Cl2Field.SetDecimalVal(fRecord.Cl2);
                fView.CO2Field.SetDecimalVal(fRecord.CO2);
                fView.NHField.SetDecimalVal(fRecord.NH);
                fView.NH3Field.SetDecimalVal(fRecord.NH3);
                fView.NH4Field.SetDecimalVal(fRecord.NH4);
                fView.PO4Field.SetDecimalVal(fRecord.PO4);
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.AquariumId = fView.AquariumCombo.GetSelectedTag<int>();
                fRecord.Timestamp = fView.TimestampField.Value;
                fRecord.Temperature = (float)fView.TemperatureField.GetDecimalVal();
                fRecord.NO3 = (float)fView.NO3Field.GetDecimalVal();
                fRecord.NO2 = (float)fView.NO2Field.GetDecimalVal();
                fRecord.GH = (float)fView.GHField.GetDecimalVal();
                fRecord.KH = (float)fView.KHField.GetDecimalVal();
                fRecord.pH = (float)fView.PHField.GetDecimalVal();
                fRecord.Cl2 = (float)fView.Cl2Field.GetDecimalVal();
                fRecord.CO2 = (float)fView.CO2Field.GetDecimalVal();
                fRecord.NH = (float)fView.NHField.GetDecimalVal();
                fRecord.NH3 = (float)fView.NH3Field.GetDecimalVal();
                fRecord.NH4 = (float)fView.NH4Field.GetDecimalVal();
                fRecord.PO4 = (float)fView.PO4Field.GetDecimalVal();

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        public void CalcCO2()
        {
            double degKH = fView.KHField.GetDecimalVal();
            double PH = fView.PHField.GetDecimalVal();
            double CO2 = ALData.CalcCO2(degKH, PH);
            fView.CO2Field.SetDecimalVal(CO2);
        }

        public void CalcNH3()
        {
            double temp = fView.TemperatureField.GetDecimalVal();
            double totalNH = fView.NHField.GetDecimalVal();
            double pH = fView.PHField.GetDecimalVal();
            double NH3 = ALData.CalcNH3(pH, temp, totalNH);
            fView.NH3Field.SetDecimalVal(NH3);
        }

        public void CalcNH4()
        {
            double totalNH = ALCore.GetDecimalVal(fView.NHField.Text);
            double NH3 = ALCore.GetDecimalVal(fView.NH3Field.Text);
            double NH4 = totalNH - NH3;
            fView.NH4Field.SetDecimalVal(NH4);
        }
    }
}
