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
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AquaMate.Tests
{
    /// <summary>
    /// Model for storing brand accuracy matrix.
    /// </summary>
    public class TestAccuracy
    {
        public string TestName { get; set; }
        public double KH { get; set; } = 1.0;
        public double GH { get; set; } = 1.0;
        public double PH { get; set; } = 1.0;
        public double NO3 { get; set; } = 1.0;
        public double PO4 { get; set; } = 1.0;
        public double NO2 { get; set; } = 1.0;
        public double NH3_NH4 { get; set; } = 1.0;
    }

    /// <summary>
    /// Row for entering current measurement results.
    /// </summary>
    public class CurrentMeasurement
    {
        public string TestName { get; set; }
        public double? Value { get; set; }
    }

    public class TestRepository
    {
        private const string JsonPath = "accuracy_matrix.json";

        public static BindingList<TestAccuracy> LoadMatrix()
        {
            if (!File.Exists(JsonPath))
                return GetDefaultMatrix();

            try {
                string json = File.ReadAllText(JsonPath);
                var list = JsonSerializer.Deserialize<List<TestAccuracy>>(json);
                return list != null ? new BindingList<TestAccuracy>(list) : GetDefaultMatrix();
            } catch {
                return GetDefaultMatrix();
            }
        }

        public static void SaveMatrix(ICollection<TestAccuracy> matrix)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(matrix, options);
            File.WriteAllText(JsonPath, json);
        }

        public static double? CalculateWeightedAverage(List<CurrentMeasurement> measurements, IEnumerable<TestAccuracy> matrix, string parameter)
        {
            double sumProduct = 0;
            double sumWeights = 0;

            foreach (var m in measurements) {
                if (m.Value == null) continue;

                var brandAccuracy = matrix.ToList().Find(x => x.TestName.Equals(m.TestName, StringComparison.OrdinalIgnoreCase));
                if (brandAccuracy == null) continue;

                double weight = parameter.ToUpper() switch {
                    "KH" => brandAccuracy.KH,
                    "GH" => brandAccuracy.GH,
                    "PH" => brandAccuracy.PH,
                    "NO3" => brandAccuracy.NO3,
                    "PO4" => brandAccuracy.PO4,
                    "NO2" => brandAccuracy.NO2,
                    "NH3/4" => brandAccuracy.NH3_NH4,
                    _ => 1.0
                };

                // Progressive weights
                weight = weight * weight;

                sumProduct += m.Value.Value * weight;
                sumWeights += weight;
            }

            return sumWeights > 0 ? Math.Round(sumProduct / sumWeights, 2) : (double?)null;
        }

        private static BindingList<TestAccuracy> GetDefaultMatrix()
        {
            var defaultList = new List<TestAccuracy>
            {
                // Drop tests with maximum accuracy (References)
                new TestAccuracy { TestName = "JBL (drops)",     KH = 1.0, GH = 1.0, PH = 1.0, NO3 = 1.0, PO4 = 1.0, NO2 = 1.0, NH3_NH4 = 1.0 },
                new TestAccuracy { TestName = "UHE (drops)",     KH = 1.0, GH = 1.0, PH = 1.0, NO3 = 1.0, PO4 = 1.0, NO2 = 1.0, NH3_NH4 = 1.0 },
                new TestAccuracy { TestName = "Sera (drops)",    KH = 1.0, GH = 1.0, PH = 1.0, NO3 = 0.8, PO4 = 0.8, NO2 = 1.0, NH3_NH4 = 1.0 },

                // Popular mid-range brands
                new TestAccuracy { TestName = "API (drops)",     KH = 0.8, GH = 0.8, PH = 1.0, NO3 = 0.8, PO4 = 0.8, NO2 = 1.0, NH3_NH4 = 0.8 },
                new TestAccuracy { TestName = "Tetra (drops)",   KH = 1.0, GH = 0.8, PH = 1.0, NO3 = 0.8, PO4 = 0.8, NO2 = 0.8, NH3_NH4 = 0.8 },
                new TestAccuracy { TestName = "Aquayer (drops)", KH = 0.8, GH = 1.0, PH = 0.8, NO3 = 0.8, PO4 = 0.8, NO2 = 0.8, NH3_NH4 = 0.8 },

                // Budget drop tests (accuracy varies by parameter)
                new TestAccuracy { TestName = "НИЛПА (drops)",   KH = 1.0, GH = 0.8, PH = 0.6, NO3 = 0.8, PO4 = 0.8, NO2 = 0.8, NH3_NH4 = 0.8 },
                new TestAccuracy { TestName = "VladOx (drops)",  KH = 1.0, GH = 0.8, PH = 1.0, NO3 = 0.6, PO4 = 0.6, NO2 = 0.8, NH3_NH4 = 0.8 },

                // Indicator test strips (express analysis)
                new TestAccuracy { TestName = "Sera (strips)",   KH = 0.6, GH = 0.6, PH = 0.6, NO3 = 0.6, PO4 = 0.6, NO2 = 0.6, NH3_NH4 = 0.6 },
                new TestAccuracy { TestName = "Tetra (strips)",  KH = 0.6, GH = 0.6, PH = 0.6, NO3 = 0.6, PO4 = 0.6, NO2 = 0.6, NH3_NH4 = 0.6 }
            };
            return new BindingList<TestAccuracy>(defaultList);
        }
    }
}
