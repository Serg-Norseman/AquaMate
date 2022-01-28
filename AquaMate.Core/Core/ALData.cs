/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2021 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using AquaMate.Core.Model;
using AquaMate.Core.Model.Tanks;
using AquaMate.Core.Types;
using BSLib;
using BSLib.Design;

namespace AquaMate.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ALData
    {
        // TODO: Move cost to settings
        public const double kWhCost = 2.76;


        public const double DefaultSoilDensity = 1.66d; // kg/l
        public const double WaterDensity = 0.9998d; // kg/l


        public static readonly MeasurementUnitProps[] MeasurementUnits = new MeasurementUnitProps[] {
            new MeasurementUnitProps(LSID.None, MeasurementType.Length),

            new MeasurementUnitProps(LSID.Centimeter, MeasurementType.Length),
            new MeasurementUnitProps(LSID.Inch, MeasurementType.Length),

            new MeasurementUnitProps(LSID.Litre, MeasurementType.Volume),
            new MeasurementUnitProps(LSID.UKGallon, MeasurementType.Volume),
            new MeasurementUnitProps(LSID.USGallon, MeasurementType.Volume),

            new MeasurementUnitProps(LSID.Kilogram, MeasurementType.Mass),
            new MeasurementUnitProps(LSID.Pound, MeasurementType.Mass),

            new MeasurementUnitProps(LSID.DegreeCelsius, MeasurementType.Temperature),
            new MeasurementUnitProps(LSID.DegreeFahrenheit, MeasurementType.Temperature),
            new MeasurementUnitProps(LSID.DegreeKelvin, MeasurementType.Temperature),

            new MeasurementUnitProps(LSID.KilogramPerLitre, MeasurementType.Density),

            new MeasurementUnitProps(LSID.LitrePerHour, MeasurementType.Flow),

            new MeasurementUnitProps(LSID.Lumen, MeasurementType.LuminousFlux),

            new MeasurementUnitProps(LSID.WattPerSquareMeter, MeasurementType.PhotosyntheticallyActiveRadiation),
        };


        public static readonly ItemProps[] ItemTypes = new ItemProps[] {
            new ItemProps(LSID.None,
                          EnumSet<ItemState>.Create(), ItemState.Unknown),

            new ItemProps(LSID.Aquarium,
                          EnumSet<ItemState>.Create(), ItemState.Broken),

            new ItemProps(LSID.Fish,
                          EnumSet<ItemState>.Create(ItemState.Alive, ItemState.Dead, ItemState.Sick), ItemState.Dead),
            new ItemProps(LSID.Invertebrate,
                          EnumSet<ItemState>.Create(ItemState.Alive, ItemState.Dead, ItemState.Sick), ItemState.Dead),
            new ItemProps(LSID.Plant,
                          EnumSet<ItemState>.Create(ItemState.Alive, ItemState.Dead, ItemState.Sick), ItemState.Dead),
            new ItemProps(LSID.Coral,
                          EnumSet<ItemState>.Create(ItemState.Alive, ItemState.Dead, ItemState.Sick), ItemState.Dead),

            new ItemProps(LSID.Nutrition,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Finished), ItemState.Finished),

            new ItemProps(LSID.Device,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Stopped, ItemState.Broken), ItemState.Broken),

            new ItemProps(LSID.Additive,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Finished), ItemState.Finished),
            new ItemProps(LSID.Chemistry,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Finished), ItemState.Finished),

            new ItemProps(LSID.Equipment,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Broken), ItemState.Broken),
            new ItemProps(LSID.Maintenance,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Broken), ItemState.Broken),
            new ItemProps(LSID.Furniture,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Broken), ItemState.Broken),
            new ItemProps(LSID.Decoration,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Broken), ItemState.Broken),
            new ItemProps(LSID.Soil,
                          EnumSet<ItemState>.Create(ItemState.InUse, ItemState.Broken), ItemState.Broken),
        };


        public static readonly LSID[] ItemStates = new LSID[] {
            LSID.None,
            LSID.Alive,
            LSID.Dead,
            LSID.Sick,
            LSID.InUse,
            LSID.Stopped,
            LSID.Finished,
            LSID.Broken,
            LSID.Sold,
        };


        public static readonly DeviceProps[] DeviceProps = new DeviceProps[] {
            new DeviceProps(LSID.Light, false, typeof(Light)),
            new DeviceProps(LSID.AirPump, false, null),
            new DeviceProps(LSID.Thermometer, true, null),
            new DeviceProps(LSID.Filter, false, typeof(Filter)),
            new DeviceProps(LSID.Heater, false, null),
            new DeviceProps(LSID.UVSterilizer, false, null),
            new DeviceProps(LSID.CO2Kit, false, null),
            new DeviceProps(LSID.Pump, false, typeof(Pump)),
        };


        public static readonly InventoryProps[] InventoryTypes = new InventoryProps[] {
            new InventoryProps(LSID.Additive, null),
            new InventoryProps(LSID.Chemistry, null),
            new InventoryProps(LSID.Equipment, null),
            new InventoryProps(LSID.Maintenance, null),
            new InventoryProps(LSID.Furniture, null),
            new InventoryProps(LSID.Decoration, typeof(Decoration)),
            new InventoryProps(LSID.Soil, typeof(Soil)),
        };


        public static readonly LSID[] TransferTypes = new LSID[] {
            LSID.Relocation,
            LSID.Purchase,
            LSID.Sale,
            LSID.Birth,
            LSID.Death,
            LSID.Exclusion,
        };


        public static readonly MaintenanceProps[] MaintenanceTypes = new MaintenanceProps[] {
            new MaintenanceProps(LSID.Restart, 0),
            new MaintenanceProps(LSID.WaterAdded, +1),
            new MaintenanceProps(LSID.WaterReplaced, 0),
            new MaintenanceProps(LSID.WaterRemoved, -1),
            new MaintenanceProps(LSID.Clean, 0),
            new MaintenanceProps(LSID.Other, 0),
            new MaintenanceProps(LSID.Fertilize, 0),
            new MaintenanceProps(LSID.Cure, 0),
            new MaintenanceProps(LSID.AquariumStarted, +1),
            new MaintenanceProps(LSID.AquariumStopped, -1),
        };


        public static readonly SpeciesProps[] SpeciesTypes = new SpeciesProps[] {
            new SpeciesProps(LSID.Fish, Color.Yellow),
            new SpeciesProps(LSID.Invertebrate, Color.Maroon),
            new SpeciesProps(LSID.Plant, Color.Green),
            new SpeciesProps(LSID.Coral, Color.HotPink),
        };


        public static readonly LSID[] SwimLevels = new LSID[] {
            LSID.SL_Any,
            LSID.SL_Top,
            LSID.SL_Top_and_Mid,
            LSID.SL_Mid,
            LSID.SL_Mid_and_Bottom,
            LSID.SL_Bottom,
        };


        public static readonly LSID[] CareLevels = new LSID[] {
            LSID.Unknown,
            LSID.Easy,
            LSID.Moderate,
            LSID.Hard,
        };


        public static readonly LSID[] Temperaments = new LSID[] {
            LSID.Unknown,
            LSID.Peaceful,
            LSID.SemiAggressive,
            LSID.Aggressive,
        };


        public static readonly LSID[] SexNames = new LSID[] {
            LSID.None,
            LSID.Female,
            LSID.Male,
            LSID.Hermaphrodite,
        };


        public static readonly LSID[] WaterTypes = new LSID[] {
            LSID.FreshWater,
            LSID.BrackishWater,
            LSID.SeaWater,
        };


        public static readonly LSID[] TankShapes = new LSID[] {
            LSID.None,
            LSID.Bowl,
            LSID.Cube,
            LSID.Rectangular,
            LSID.BowFront,
            LSID.PlateFrontCorner,
            LSID.BowFrontCorner,
            LSID.Cylinder,
        };


        public static readonly Type[] TankTypes = new Type[] {
            typeof(BaseTank), // Unknown
            typeof(BowlTank), // Bowl
            typeof(CubeTank), // Cube
            typeof(RectangularTank), // Rectangular
            typeof(BowFrontTank), // BowFront
            typeof(BaseTank), // PlateFrontCorner
            typeof(BaseTank), // BowFrontCorner
            typeof(CylinderTank), // Cylinder
        };


        public static readonly LSID[] ScheduleTypes = new LSID[] {
            LSID.Single,
            LSID.Daily,
            LSID.Weekly,
            LSID.Monthly,
            LSID.Yearly,
        };


        public static readonly LSID[] TaskStatuses = new LSID[] {
            LSID.ToDo,
            LSID.Canceled,
            LSID.Snoozed,
            LSID.Late,
            LSID.Closed,
        };


        /// <summary>
        /// NH3 levels (ppm)
        /// </summary>
        public static readonly ValueRange[] NH3Ranges = new ValueRange[] {
            new ValueRange(0.00,  0.02, Color.Green, ""),
            new ValueRange(0.02,  0.05, Color.YellowGreen, "Alert"),
            new ValueRange(0.05,  0.20, Color.Orange, "Alarm"),
            new ValueRange(0.20,  0.50, Color.Red, "Toxic"),
            new ValueRange(0.50,  1.00, Color.Maroon, "Deadly"),
        };


        /// <summary>
        /// CO2 alert levels
        ///  0 -> 12 mg/l    red [very low for plants - adjust]
        /// 13 -> 20 mg/l    yellow [low for plants - adjust]
        /// 21 -> 30 mg/l    green [optimum for plants & fish]
        /// 31 -> 40 mg/l    yellow [high for fish - adjust]
        ///     > 40 mg/l    red [toxic for fish - adjust immediately] 
        /// </summary>
        public static readonly ValueRange[] CO2Ranges = new ValueRange[] {
            new ValueRange(0, 12, Color.DeepPink, "very_low_adjust"),
            new ValueRange(12, 20, Color.YellowGreen, "low_adjust"),
            new ValueRange(20, 30, Color.Green, ""),
            new ValueRange(30, 40, Color.Orange, "alert_high"),
            new ValueRange(40, 50, Color.Red, "Toxic"),
        };


        public static readonly ValueRange[] NO3Ranges = new ValueRange[] {
            new ValueRange(0, 25, Color.Green, "Safe"),
            new ValueRange(25, 100, Color.Orange, "Warn"),
            new ValueRange(100, 250, Color.Red, "Alarm"),
        };


        public static readonly ValueRange[] NO2Ranges = new ValueRange[] {
            new ValueRange(0, 1, Color.Green, "Safe"),
            new ValueRange(1, 5, Color.Orange, "Warn"),
            new ValueRange(5, 10, Color.Red, "Alarm"),
        };


        public static readonly ValueRange[] GHRanges = new ValueRange[] {
            new ValueRange(0, 4, Color.Orange, "Warn"),
            new ValueRange(4, 16, Color.Green, "Safe"),
        };


        public static readonly ValueRange[] KHRanges = new ValueRange[] {
            new ValueRange(0, 3, Color.Red, "Alarm"),
            new ValueRange(3, 10, Color.Green, "Safe"),
            new ValueRange(10, 20, Color.Orange, "Warn"),
        };


        public static readonly ValueRange[] pHRanges = new ValueRange[] {
            new ValueRange(0, 6.4, Color.Orange, "Warn"),
            new ValueRange(6.4, 8.4, Color.Green, "Safe"),
            new ValueRange(8.4, 10, Color.Red, "Alarm"),
        };


        public static readonly ValueRange[] Cl2Ranges = new ValueRange[] {
            new ValueRange(0, 0.8, Color.Green, "Safe"),
            new ValueRange(0.8, 3.0, Color.Red, "Alarm"),
        };


        public static readonly ValueRange[] PO4Ranges = new ValueRange[] {
            new ValueRange(0.00,  0.50, Color.YellowGreen, "not enough for plants"),
            new ValueRange(0.50,  1.00, Color.Green, ""),
            new ValueRange(1.00,  2.00, Color.Orange, "algae growth"),
            new ValueRange(2.00,  5.00, Color.Red, "algae overgrowth"),
        };


        public static readonly ValueRange[] RedfieldRanges = new ValueRange[] {
            new ValueRange(0, 5, Color.Red, "high (Cianobacter)"),
            new ValueRange(5, 10, Color.Orange, "media (Cianobacter)"),
            new ValueRange(10, 20, Color.Green, "low"),
            new ValueRange(20, 25, Color.Orange, "media (green algae)"),
            new ValueRange(25, 30, Color.Red, "high (green algae)"),
        };


        public static string GetLSuom(LSID lsid, MeasurementType measurementType)
        {
            string result = Localizer.LS(lsid);

            MeasurementUnit mUnit = MeasurementUnit.Unknown;
            switch (measurementType) {
                case MeasurementType.Volume:
                    mUnit = ALSettings.Instance.VolumeUoM;
                    break;
                case MeasurementType.Length:
                    mUnit = ALSettings.Instance.LengthUoM;
                    break;
                case MeasurementType.Mass:
                    mUnit = ALSettings.Instance.MassUoM;
                    break;
                case MeasurementType.Temperature:
                    mUnit = ALSettings.Instance.TemperatureUoM;
                    break;
                case MeasurementType.Density:
                    mUnit = MeasurementUnit.KilogramPerLitre;
                    break;
                case MeasurementType.Flow:
                    mUnit = MeasurementUnit.LitrePerHour;
                    break;
                case MeasurementType.LightTemperature:
                    mUnit = MeasurementUnit.DegreeKelvin; // FIXME
                    break;
                case MeasurementType.LuminousFlux:
                    mUnit = MeasurementUnit.Lumen;
                    break;
                case MeasurementType.PhotosyntheticallyActiveRadiation:
                    mUnit = MeasurementUnit.WattPerSquareMeter;
                    break;
            }

            if (mUnit != MeasurementUnit.Unknown) {
                var props = ALData.MeasurementUnits[(int)mUnit];
                var uom = props.StrAbbreviation;

                if (!string.IsNullOrEmpty(uom)) {
                    result += ", " + uom;
                }
            }

            return result;
        }

        public static string CastStr(double value, MeasurementType measurementType, int decimalDigits = 2, bool hideZero = false)
        {
            // sourceUoM = always only SI
            MeasurementUnit targetUnit = MeasurementUnit.Unknown;

            switch (measurementType) {
                case MeasurementType.Length:
                    targetUnit = ALSettings.Instance.LengthUoM;
                    break;
                case MeasurementType.Volume:
                    targetUnit = ALSettings.Instance.VolumeUoM;
                    break;
                case MeasurementType.Mass:
                    targetUnit = ALSettings.Instance.MassUoM;
                    break;
                case MeasurementType.Temperature:
                    targetUnit = ALSettings.Instance.TemperatureUoM;
                    break;
            }

            switch (targetUnit) {
                case MeasurementUnit.Centimeter:
                    break;
                case MeasurementUnit.Inch:
                    value = UnitConverter.cm2inch(value);
                    break;
                case MeasurementUnit.Litre:
                    break;
                case MeasurementUnit.UKGallon:
                    break;
                case MeasurementUnit.USGallon:
                    value = UnitConverter.l2gal(value);
                    break;
                case MeasurementUnit.Kilogram:
                    break;
                case MeasurementUnit.Pound:
                    value = UnitConverter.kg2lb(value);
                    break;
                case MeasurementUnit.DegreeCelsius:
                    break;
                case MeasurementUnit.DegreeFahrenheit:
                    value = UnitConverter.C2F(value);
                    break;
                case MeasurementUnit.DegreeKelvin:
                    value = UnitConverter.C2K(value);
                    break;

                case MeasurementUnit.Unknown:
                default:
                    break;
            }

            return ALCore.GetDecimalStr(value, decimalDigits, hideZero);
        }

        public static void CalcSegmentParams(float chordWidth, float chordLength, out float radius, out float wedgeAngle)
        {
            radius = (chordWidth / 2.0f) + (chordLength * chordLength) / (8.0f * chordWidth);
            wedgeAngle = (2.0f * (float)Math.Asin(chordLength / (2.0f * radius)));
        }

        public static double CalcNH3(double pH, double temperature, double totalNH)
        {
            // https://rdrr.io/cran/AmmoniaConcentration/man/ammonia.html
            // f = 1 / (1 + 10^(0.09018 - pH + (2727.92 / T)))

            return totalNH / (1 + Math.Pow(10, (0.0901821 - pH + (2729.92 / (273.15 + temperature)))));
        }

        /// <summary>
        /// Calculate CO2
        /// </summary>
        public static double CalcCO2(double degKH, double PH)
        {
            // common variant: CO2 = 3 * degKH * 10^(7 - pH) [1 source]
            // CO2 = 12.839 * dKH * 10^(6.37 - pH) [2 source]

            // corrected variants:
            // CO2 = 15.65 * dKH * 10^(6.37 - pH) [3 source]
            return 15.65 * degKH * Math.Pow(10, 6.37 - PH);
        }

        /// <summary>
        /// Calculate KH = CO2 / 3 / 10(7-PH)
        /// </summary>
        public static double CalcKH(double PH, double CO2)
        {
            return CO2 / 3 / Math.Pow(10, 7 - PH);
        }

        /// <summary>
        /// Calculate pH = 7,5 + Log(KH) - Log(ppmCO2)
        /// </summary>
        public static double CalcPH(double degKH, double CO)
        {
            return 7.5 + Math.Log10(degKH) - Math.Log10(CO);
        }

        /// <summary>
        /// (all ppm)
        /// </summary>
        public static double CalcTDS(double GH, double KH, double nitrate, double nitrite, double chlorine, double other)
        {
            return (GH + KH + nitrate + nitrite + chlorine + other);
        }

        /// <summary>
        /// Treating nitrite calculator
        /// </summary>
        /// <param name="volume">Tank volume in litres</param>
        /// <param name="nitrite">Nitrite level (NO2, mg/l)</param>
        /// <returns>grams of salt to aquarium</returns>
        public static double CalcSalt(double volume, double nitrite)
        {
            return (volume * nitrite * 0.0075);
        }

        public static double CalcRedfield(double NO3, double PO4)
        {
            return (NO3 / PO4) / 1.45;
        }

        public static IEnumerable<ComboItem<T>> GetNamesList<T>(IProps[] names)
        {
            T[] enumVals = (T[])Enum.GetValues(typeof(T));
            int valsLen = enumVals.Length;

            int namesLen = names.Length;
            if (valsLen != namesLen)
                throw new ALException("Enumeration and names do not match");

            var result = new ComboItem<T>[valsLen];
            for (int i = 0; i < valsLen; i++) {
                var enm = enumVals[i];
                int eIdx = Convert.ToInt32(enm);
                result[i] = new ComboItem<T>(Localizer.LS(names[eIdx].Name), enm);
            }
            return result;
        }

        public static IEnumerable<ComboItem<T>> GetNamesList<T>(LSID[] names)
        {
            T[] enumVals = (T[])Enum.GetValues(typeof(T));
            int valsLen = enumVals.Length;

            int namesLen = names.Length;
            if (valsLen != namesLen)
                throw new ALException("Enumeration and names do not match");

            var result = new ComboItem<T>[valsLen];
            for (int i = 0; i < valsLen; i++) {
                var enm = enumVals[i];
                int eIdx = Convert.ToInt32(enm);
                result[i] = new ComboItem<T>(Localizer.LS(names[eIdx]), enm);
            }
            return result;
        }

        public static IEnumerable<ComboItem<ItemState>> GetItemStateNamesList(ItemType itemType)
        {
            ItemProps props = ALData.ItemTypes[(int)itemType];

            var result = new List<ComboItem<ItemState>>();
            for (ItemState state = ItemState.Unknown; state <= ItemState.Broken; state++) {
                if (state == ItemState.Unknown || props.States.Contains(state)) {
                    result.Add(new ComboItem<ItemState>(Localizer.LS(ALData.ItemStates[(int)state]), state));
                }
            }
            return result;
        }

        public static IEnumerable<ComboItem<int>> GetEntityNamesList(IEnumerable<Entity> entities)
        {
            var result = new List<ComboItem<int>>();
            foreach (var ent in entities) {
                result.Add(new ComboItem<int>(ent.ToString(), ent.Id));
            }
            return result;
        }

        public static IList<string> GetStringList(IEnumerable<QString> queryStrings)
        {
            var result = new List<string>();
            foreach (var qs in queryStrings) {
                if (!string.IsNullOrEmpty(qs.element))
                    result.Add(qs.element);
            }
            return result;
        }
    }
}
