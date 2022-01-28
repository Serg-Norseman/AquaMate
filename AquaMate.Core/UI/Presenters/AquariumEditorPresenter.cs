/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.ComponentModel;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IAquariumEditorView : IEditorView<Aquarium>
    {
        ITextBox NameField { get; }
        IComboBox BrandCombo { get; }
        ITextBox DescriptionField { get; }
        IComboBox ShapeCombo { get; }
        IComboBox WaterTypeCombo { get; }

        ITextBox TankVolumeField { get; }
        ITextBox UnderfillHeightField { get; }
        ITextBox SoilHeightField { get; }
        IPropertyGrid PropsGrid { get; }
        ITextBox WaterVolumeField { get; }
        ITextBox SoilVolumeField { get; }
        ITextBox SoilMassField { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AquariumEditorPresenter : EditorPresenter<IModel, Aquarium, IAquariumEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "AquariumEditorPresenter");


        public AquariumEditorPresenter(IAquariumEditorView view) : base(view)
        {
            var tankShapesList = ALData.GetNamesList<TankShape>(ALData.TankShapes);
            fView.ShapeCombo.AddRange(tankShapesList, false);

            var waterTypesList = ALData.GetNamesList<AquariumWaterType>(ALData.WaterTypes);
            fView.WaterTypeCombo.AddRange(waterTypesList, false);
        }

        public override void UpdateView()
        {
            fView.NameField.Text = fRecord.Name;

            fView.BrandCombo.AddRange(fModel.QueryAquariumBrands(), true);
            fView.BrandCombo.Text = fRecord.Brand;

            fView.DescriptionField.Text = fRecord.Description;
            fView.ShapeCombo.SetSelectedTag(fRecord.TankShape);

            fView.WaterTypeCombo.SetSelectedTag(fRecord.WaterType);

            fView.TankVolumeField.SetDecimalVal(fRecord.TankVolume);
            fView.UnderfillHeightField.SetDecimalVal(fRecord.UnderfillHeight);
            fView.SoilHeightField.SetDecimalVal(fRecord.SoilHeight);
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.Brand = fView.BrandCombo.Text;
                fRecord.Description = fView.DescriptionField.Text;
                fRecord.TankShape = fView.ShapeCombo.GetSelectedTag<TankShape>();
                fRecord.WaterType = fView.WaterTypeCombo.GetSelectedTag<AquariumWaterType>();

                fRecord.TankVolume = fView.TankVolumeField.GetDecimalVal();
                fRecord.UnderfillHeight = fView.UnderfillHeightField.GetDecimalVal();
                fRecord.SoilHeight = fView.SoilHeightField.GetDecimalVal();

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        public void RefreshProps()
        {
            var tankShape = fView.ShapeCombo.GetSelectedTag<TankShape>();
            RefreshProps(tankShape);
        }

        public void RefreshProps(TankShape tankShape)
        {
            ITank tank = fRecord.GetTank(tankShape, fRecord.TankProperties);
            TypeDescriptor.AddAttributes(tank, new Attribute[] { new ReadOnlyAttribute(true) });
            tank.SetPropNames();
            fView.PropsGrid.SelectedObject = tank;

            RecalcValues();
        }

        public void RecalcValues()
        {
            var tankShape = fView.ShapeCombo.GetSelectedTag<TankShape>();

            fView.TankVolumeField.Enabled = (tankShape == TankShape.Unknown);

            if (tankShape != TankShape.Unknown) {
                double tankVolume = fRecord.CalcTankVolume(tankShape);
                fView.TankVolumeField.SetDecimalVal(tankVolume);
            }

            double underfillHeight = ALCore.GetDecimalVal(fView.UnderfillHeightField.Text);
            double soilHeight = ALCore.GetDecimalVal(fView.SoilHeightField.Text);

            double waterVolume = fRecord.CalcWaterVolume(tankShape, underfillHeight, soilHeight);
            fView.WaterVolumeField.Text = ALData.CastStr(waterVolume, MeasurementType.Volume);

            double soilVolume = fRecord.CalcSoilVolume(tankShape, soilHeight);
            fView.SoilVolumeField.Text = ALData.CastStr(soilVolume, MeasurementType.Volume);

            double soilMass = soilVolume * ALData.DefaultSoilDensity;
            fView.SoilMassField.Text = ALData.CastStr(soilMass, MeasurementType.Mass);

            double waterMass = waterVolume * ALData.WaterDensity;
            //txtWaterMass.Text = ALData.CastStr(waterMass, MeasurementType.Mass);

            double totalMass = waterMass + soilMass;
        }

        public void EditTank()
        {
            if (fRecord == null) return;

            TankShape tankShape = fView.ShapeCombo.GetSelectedTag<TankShape>();
            var tank = fRecord.GetTank(tankShape, fRecord.TankProperties);

            IBrowser browser = fModel.Browser;
            if (browser.EditTank(tank)) {
                fRecord.Tank = tank;
                RefreshProps(tankShape);
            }
        }
    }
}
