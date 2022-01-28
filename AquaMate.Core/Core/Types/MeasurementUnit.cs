/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core.Types
{
    public enum MeasurementUnit
    {
        Unknown,

        // Length
        Centimeter,
        Inch,

        // Volume
        Litre,
        UKGallon,
        USGallon,

        // Mass
        Kilogram,
        Pound,

        // Temperature
        DegreeCelsius,
        DegreeFahrenheit,
        DegreeKelvin,

        // Density
        KilogramPerLitre,
        //PoundPerUKGallon,
        //PoundPerUSGallon,

        // Flow
        LitrePerHour,

        // LuminousFlux
        Lumen,

        // PhotosyntheticallyActiveRadiation
        WattPerSquareMeter,

        First = Centimeter,
        Last = DegreeKelvin
    }
}
