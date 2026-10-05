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
using BSLib.Design.MVP.Controls;

namespace AquaMate.Core.Calculations
{
    public abstract class BaseCalculation : ILocalizable
    {
        private double fResultValue;
        private readonly CalculationType fType;
        private CalculationProps fProps;


        public static readonly CalculationProps[] CalculationData = new CalculationProps[] {
            new CalculationProps("Convert cm to inch", "1 cm = 0.3937 inch",
                                 new [] { new CalcParam("SourceValue", "Source Value (cm)") },
                                 new CalcParam("ResultValue", "Result (inch)"), UnitConverter.cm2inch),
            new CalculationProps("Convert inch to cm", "1 inch = 2.54 cm",
                                 new [] { new CalcParam("SourceValue", "Source Value (inch)") },
                                 new CalcParam("ResultValue", "Result (cm)"), UnitConverter.inch2cm),
            new CalculationProps("Convert feet to cm", "1 feet = 30.48 cm",
                                 new [] { new CalcParam("SourceValue", "Source Value (feet)") },
                                 new CalcParam("ResultValue", "Result (cm)"), UnitConverter.feet2cm),
            new CalculationProps("Convert cm to feet", "1 cm = 0.0328 feet",
                                 new [] { new CalcParam("SourceValue", "Source Value (cm)") },
                                 new CalcParam("ResultValue", "Result (feet)"), UnitConverter.cm2feet),
            new CalculationProps("Convert US gal to liters", "1 US gallon = 3.78541178 liter",
                                 new [] { new CalcParam("SourceValue", "Source Value (gal)") },
                                 new CalcParam("ResultValue", "Result (l)"), UnitConverter.gal2l),
            new CalculationProps("Convert liters to US gal", "1 liter = 0.264172 US gallon",
                                 new [] { new CalcParam("SourceValue", "Source Value (l)") },
                                 new CalcParam("ResultValue", "Result (gal)"), UnitConverter.l2gal),
            new CalculationProps("Convert cubic cm (cc) to liters", "1 cc = 0.001 liter",
                                 new [] { new CalcParam("SourceValue", "Source Value (cc)") },
                                 new CalcParam("ResultValue", "Result (l)"), UnitConverter.cc2l),
            new CalculationProps("Convert liters to cubic cm (cc)", "1 liter = 1000 cc",
                                 new [] { new CalcParam("SourceValue", "Source Value (l)") },
                                 new CalcParam("ResultValue", "Result (cc)"), UnitConverter.l2cc),

            new CalculationProps("Convert millilitre (ml) to drops", "1 ml = 20 drops",
                                 new [] { new CalcParam("SourceValue", "Source Value (ml)") },
                                 new CalcParam("ResultValue", "Result (drops)"), UnitConverter.ml2drops),
            new CalculationProps("Convert drops to millilitre (ml)", "1 drop = 0.05 ml",
                                 new [] { new CalcParam("SourceValue", "Source Value (drops)") },
                                 new CalcParam("ResultValue", "Result (ml)"), UnitConverter.drops2ml),

            new CalculationProps("Convert table spoons (tsp) to cubic cm (cc)", "1 tsp = 14.786765 cc",
                                 new [] { new CalcParam("SourceValue", "Source Value (tsp)") },
                                 new CalcParam("ResultValue", "Result (cc)"), UnitConverter.tsp2cc),
            new CalculationProps("Convert cubic cm (cc) to table spoons (tsp)", "1 cc = 0.067628 tsp",
                                 new [] { new CalcParam("SourceValue", "Source Value (cc)") },
                                 new CalcParam("ResultValue", "Result (tsp)"), UnitConverter.cc2tsp),
            new CalculationProps("Convert milligrams to grams", "1 mg = 0.001 g",
                                 new [] { new CalcParam("SourceValue", "Source Value (mg)") },
                                 new CalcParam("ResultValue", "Result (g)"), UnitConverter.mg2g),
            new CalculationProps("Convert grams to milligrams", "1 g = 1000 mg",
                                 new [] { new CalcParam("SourceValue", "Source Value (g)") },
                                 new CalcParam("ResultValue", "Result (mg)"), UnitConverter.g2mg),
            new CalculationProps("Convert table spoons (tsp) to grams", "1 tsp = 5 g",
                                 new [] { new CalcParam("SourceValue", "Source Value (tsp)") },
                                 new CalcParam("ResultValue", "Result (g)"), UnitConverter.tsp2g),
            new CalculationProps("Convert grams to table spoons (tsp)", "1 g = 0.2 tsp",
                                 new [] { new CalcParam("SourceValue", "Source Value (g)") },
                                 new CalcParam("ResultValue", "Result (tsp)"), UnitConverter.g2tsp),
            new CalculationProps("Convert grams to ounces", "1 oz = 0.035274 g",
                                 new [] { new CalcParam("SourceValue", "Source Value (g)") },
                                 new CalcParam("ResultValue", "Result (oz)"), UnitConverter.g2oz),
            new CalculationProps("Convert ounces to grams", "1 g = 0.035274 oz",
                                 new [] { new CalcParam("SourceValue", "Source Value (oz)") },
                                 new CalcParam("ResultValue", "Result (g)"), UnitConverter.oz2g),
            new CalculationProps("Convert kilograms to pounds", "1 lb = 0.453592 kg",
                                 new [] { new CalcParam("SourceValue", "Source Value (kg)") },
                                 new CalcParam("ResultValue", "Result (lb)"), UnitConverter.kg2lb),
            new CalculationProps("Convert pounds to kilograms", "1 kg = 2.204623 lb",
                                 new [] { new CalcParam("SourceValue", "Source Value (lb)") },
                                 new CalcParam("ResultValue", "Result (kg)"), UnitConverter.lb2kg),
            new CalculationProps("Convert Celsius to Fahrenheit", "1 °F = (1.8 × °C) + 32",
                                 new [] { new CalcParam("SourceValue", "Source Value (C)") },
                                 new CalcParam("ResultValue", "Result (F)"), UnitConverter.C2F),
            new CalculationProps("Convert Fahrenheit to Celsius", "1 °C = 0.555555555556 × (°F - 32)",
                                 new [] { new CalcParam("SourceValue", "Source Value (F)") },
                                 new CalcParam("ResultValue", "Result (C)"), UnitConverter.F2C),
            new CalculationProps("Convert Kelvin to Fahrenheit", "1 °F = K × 1.8 − 459.67",
                                 new [] { new CalcParam("SourceValue", "Source Value (K)") },
                                 new CalcParam("ResultValue", "Result (F)"), UnitConverter.K2F),
            new CalculationProps("Convert Fahrenheit to Kelvin", "1 °K = (°F + 459.67) x 0.555555555556",
                                 new [] { new CalcParam("SourceValue", "Source Value (F)") },
                                 new CalcParam("ResultValue", "Result (K)"), UnitConverter.F2K),
            new CalculationProps("Convert Celsius to Kelvin", "1 °K = °C + 273.15",
                                 new [] { new CalcParam("SourceValue", "Source Value (C)") },
                                 new CalcParam("ResultValue", "Result (K)"), UnitConverter.C2K),
            new CalculationProps("Convert Kelvin to Celsius", "1 °C = K − 273.15",
                                 new [] { new CalcParam("SourceValue", "Source Value (K)") },
                                 new CalcParam("ResultValue", "Result (C)"), UnitConverter.K2C),

            new CalculationProps("Convert KH(ppm, mg/L) to KH(degrees)", "1 °KH = 0.056 * KH(ppm)",
                                 new [] { new CalcParam("SourceValue", "Source Value (KHppm)") },
                                 new CalcParam("ResultValue", "Result (KHdeg)"), UnitConverter.ConvKHppm2KHdeg),
            new CalculationProps("Convert KH(ppm, mg/L) to KH(meq/l)", "1 meq/l = KH(ppm) / 50",
                                 new [] { new CalcParam("SourceValue", "Source Value (KHppm)") },
                                 new CalcParam("ResultValue", "Result (meq/l)"), UnitConverter.ConvKHppm2KHmeql),


            new CalculationProps("Convert GH(ppm, mg/L) to GH(degrees)", "1 °GH = GH(ppm) / 16.7",
                                 new [] { new CalcParam("SourceValue", "Source Value (GHppm)") },
                                 new CalcParam("ResultValue", "Result (GHdeg)"), UnitConverter.ConvGHppm2GHdeg),


            new CalculationProps("Treating nitrite calculator", "grams of salt to aquarium",
                                 new [] { new CalcParam("Volume", "volume (litres)"), new CalcParam("Nitrite", "nitrite (NO2, mg/l)") },
                                 new CalcParam("ResultValue", "Result (grams of salt)"), null),
        };


        [Browsable(false)]
        public virtual string Description
        {
            get {
                var calcProps = CalculationData[(int)Type];
                return calcProps.Description;
            }
        }

        [Browsable(true), DisplayName("Result"), Category("Results"), Description("Value of result"), ReadOnly(true)]
        public double ResultValue
        {
            get { return fResultValue; }
            set { fResultValue = value; }
        }

        [Browsable(false)]
        public CalculationType Type
        {
            get { return fType; }
        }


        protected BaseCalculation(CalculationType type)
        {
            fType = type;
            SetLocale();
        }

        public void SetProps(CalculationProps props)
        {
            fProps = props;
            SetLocale();
        }

        public abstract void Calculate();

        public virtual void SetLocale()
        {
            var calcProps = CalculationData[(int)Type];

            foreach (var argParam in calcProps.Args) {
                ALCore.SetDisplayNameValue(this, argParam.PropName, argParam.DispName);
            }

            var resultParam = calcProps.Result;
            ALCore.SetDisplayNameValue(this, resultParam.PropName, resultParam.DispName);
        }

        public static IEnumerable<ComboItem<T>> GetNamesList<T>()
        {
            T[] enumVals = (T[])Enum.GetValues(typeof(T));
            int valsLen = enumVals.Length;

            var names = CalculationData;
            int namesLen = names.Length;
            if (valsLen != namesLen)
                throw new ALException("Enumeration and names do not match");

            var result = new ComboItem<T>[valsLen];
            for (int i = 0; i < valsLen; i++) {
                var enm = enumVals[i];
                int eIdx = Convert.ToInt32(enm);
                result[i] = new ComboItem<T>(names[eIdx].Name, enm);
            }
            return result;
        }
    }
}
