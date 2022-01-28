/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core
{
    public static class UnitConverter
    {
        /// <summary>
        /// Convert cm to inch:  1 cm = 0.3937 inch
        /// </summary>
        public static double cm2inch(double cm)
        {
            return cm * 0.393701;
        }

        /// <summary>
        /// Convert inch to cm:  1 inch = 2.54 cm
        /// </summary>
        public static double inch2cm(double inch)
        {
            return inch * 2.54;
        }

        /// <summary>
        /// Convert feet to cm:  1 feet = 30.48 cm
        /// </summary>
        public static double feet2cm(double feet)
        {
            return feet * 30.48;
        }

        /// <summary>
        /// Convert cm to feet:  1 cm = 0.0328 feet
        /// </summary>
        public static double cm2feet(double cm)
        {
            return cm * 0.0328;
        }

        /// <summary>
        /// Convert US gal to liters:  1 US gallon = 3.78541178 liter
        /// </summary>
        public static double gal2l(double gal)
        {
            return gal * 3.78541178;
        }

        /// <summary>
        /// Convert liters to US gal:  1 liter = 0.264172 US gallon
        /// </summary>
        public static double l2gal(double l)
        {
            return l * 0.264172;
        }

        /// <summary>
        /// Convert cubic cm (cc) to liters:  1 cc = 0.001 liter
        /// </summary>
        public static double cc2l(double cc)
        {
            return cc * 0.001;
        }

        /// <summary>
        /// Convert liters to cubic cm (cc):  1 liter = 1000 cc
        /// </summary>
        public static double l2cc(double l)
        {
            return l * 1000;
        }

        /// <summary>
        /// Convert table spoons (tsp) to cubic cm (cc):  1 tsp = 14.786765 cc
        /// </summary>
        public static double tsp2cc(double tsp)
        {
            return tsp * 14.786765;
        }

        /// <summary>
        /// Convert cubic cm (cc) to table spoons (tsp):  1 cc = 0.067628 tsp
        /// </summary>
        public static double cc2tsp(double cc)
        {
            return cc * 0.067628;
        }

        /// <summary>
        /// Convert milligrams to grams:  1 mg = 0.001 g
        /// </summary>
        public static double mg2g(double mg)
        {
            return mg / 1000;
        }

        /// <summary>
        /// Convert grams to milligrams:  1 g = 1000 mg
        /// </summary>
        public static double g2mg(double g)
        {
            return g * 1000;
        }

        /// <summary>
        /// Convert table spoons (tsp) to grams:  1 tsp = 5 g
        /// </summary>
        public static double tsp2g(double tsp)
        {
            return tsp * 5;
        }

        /// <summary>
        /// Convert grams to table spoons (tsp):  1 g = 0.2 tsp
        /// </summary>
        public static double g2tsp(double g)
        {
            return g * 0.2;
        }

        /// <summary>
        /// Convert grams to ounces:  1 oz = 0.035274 g
        /// </summary>
        public static double g2oz(double g)
        {
            return g * 0.035274;
        }

        /// <summary>
        /// Convert ounces to grams:  1 g = 0.035274 oz
        /// </summary>
        public static double oz2g(double oz)
        {
            return oz * 28.3495;
        }

        /// <summary>
        /// Convert kilograms to pounds:  1 lb = 0.453592 kg
        /// </summary>
        public static double kg2lb(double kg)
        {
            return kg * 2.204623;
        }

        /// <summary>
        /// Convert pounds to kilograms:  1 kg = 2.204623 lb
        /// </summary>
        public static double lb2kg(double lb)
        {
            return lb * 0.453592;
        }

        /// <summary>
        /// Convert Celsius to Fahrenheit:  1 °F = (1.8 × °C) + 32
        /// </summary>
        public static double C2F(double celsius)
        {
            return (celsius * 1.8) + 32;
        }

        /// <summary>
        /// Convert Fahrenheit to Celsius:  1 °C = 0.555555555556 × (°F - 32)
        /// </summary>
        public static double F2C(double far)
        {
            return (far - 32) * 0.555555555556;
        }

        /// <summary>
        /// Convert Kelvin to Fahrenheit:  1 °F = K × 1.8 − 459.67
        /// </summary>
        public static double K2F(double kelvin)
        {
            return (kelvin * 1.8) - 459.67;
        }

        /// <summary>
        /// Convert Fahrenheit to Kelvin:  1 °K = (°F + 459.67) x 0.555555555556
        /// </summary>
        public static double F2K(double far)
        {
            return (far + 459.67) * 0.555555555556;
        }

        /// <summary>
        /// Convert Celsius to Kelvin:  1 °K = °C + 273.15
        /// </summary>
        public static double C2K(double celsius)
        {
            return celsius + 273.15;
        }

        /// <summary>
        /// Convert Kelvin to Celsius:  1 °C = K − 273.15
        /// </summary>
        public static double K2C(double kelvin)
        {
            return kelvin - 273.15;
        }

        /// <summary>
        /// Convert KH(ppm, mg/L) to KH(degrees):  1 °KH = 0.056 * KH(ppm)
        /// </summary>
        public static double ConvKHppm2KHdeg(double ppmKH)
        {
            return 0.056 * ppmKH;
        }

        /// <summary>
        /// Convert KH(ppm, mg/L) to KH(meq/l):  1 meq/l = KH(ppm) / 50
        /// </summary>
        public static double ConvKHppm2KHmeql(double ppmKH)
        {
            return ppmKH / 50;
        }


        /// <summary>
        /// Convert GH(ppm, mg/L) to GH(degrees):  1 °GH = GH(ppm) / 16.7
        /// </summary>
        public static double ConvGHppm2GHdeg(double ppmGH)
        {
            return ppmGH / 16.7;
        }


        /// <summary>
        /// Convert millilitre (ml) to drops:  1 ml = 20 drops
        /// </summary>
        public static double ml2drops(double ml)
        {
            return ml * 20.0d;
        }

        /// <summary>
        /// Convert drops to millilitre (ml):  1 drop = 0.05 ml
        /// </summary>
        public static double drops2ml(double drops)
        {
            return drops * 0.05;
        }
    }
}
