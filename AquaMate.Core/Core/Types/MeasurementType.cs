/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core.Types
{
    public enum MeasurementType
    {
        // public
        Ca, // -> GH
        CO2,
        Cu,
        Density,
        Fe,
        GH, // General water Hardness, magnesium (Mg+) and calcium (Ca+) dissolved in water (uoms: dH, ppm (parts per million))
        KH, // Carbonate Hardness, carbonates (CO3-) and bicarbonates (HCO3-) dissolved in water
        Mg, // -> GH
        NH, // [Ammonia + Ammonium]
        NH3, // Ammonia
        NH4, // Ammonium (NH4 +)
        NO2, // Nitrite (NO2 -)
        NO3, // Nitrate (NO3 -)
        O2, // Oxygen
        PH,
        PO4, // Phosphate (PO4 3-)
        Temperature,

        // hidden, application's specific
        Length,
        Volume,
        Mass,
        Flow,
        LightTemperature,
        LuminousFlux,
        PhotosyntheticallyActiveRadiation,

        // KH & GH (ppm, degree), Density (_units_), Salinity (ppt), Conductivity, I, K, Mn, B, Mo, S, Zn
    }
}
