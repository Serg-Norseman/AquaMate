/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
