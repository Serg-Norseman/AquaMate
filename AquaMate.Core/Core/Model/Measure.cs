/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Text;
using AquaMate.Core.Types;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Measure : AquariumDetails, IEventEntity
    {
        public DateTime Timestamp { get; set; }

        // physical
        public float Conductivity { get; set; }
        public float Density { get; set; }
        public float Temperature { get; set; } // dlg+

        // chemical elements
        public float Ca { get; set; }
        public float Cu { get; set; }
        public float Fe { get; set; }
        public float Mg { get; set; }

        // chemical compounds and ions
        public float Cl2 { get; set; } // dlg+
        public float CO2 { get; set; } // dlg+
        public float NH { get; set; } // dlg+
        public float NH3 { get; set; } // dlg+
        public float NH4 { get; set; } // dlg+
        public float NO2 { get; set; } // dlg+
        public float NO3 { get; set; } // dlg+
        public float O2 { get; set; }
        public float PO4 { get; set; } // dlg+

        // chemical complex
        public float GH { get; set; } // dlg+
        public float KH { get; set; } // dlg+
        public float pH { get; set; } // dlg+


        public override EntityType EntityType
        {
            get {
                return EntityType.Measure;
            }
        }


        public Measure()
        {
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            AddVal(str, "T", Temperature);
            AddVal(str, "GH", GH);
            AddVal(str, "KH", KH);
            AddVal(str, "pH", pH);
            AddVal(str, "NO3", NO3);
            AddVal(str, "NO2", NO2);
            AddVal(str, "Cl2", Cl2);
            AddVal(str, "CO2", CO2);
            AddVal(str, "NH", NH);
            AddVal(str, "NH3", NH3);
            AddVal(str, "NH4", NH4);
            return str.ToString();
        }

        private static void AddVal(StringBuilder str, string sign, float value)
        {
            if (value == 0.0d) return;

            if (str.Length > 0) str.Append(", ");

            string strVal = ALCore.GetDecimalStr(value, 2);
            str.Append(sign);
            str.Append("=");
            str.Append(strVal);
        }
    }
}
