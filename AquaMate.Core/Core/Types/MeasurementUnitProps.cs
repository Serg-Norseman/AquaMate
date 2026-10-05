/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core.Types
{
    public sealed class MeasurementUnitProps
    {
        public LSID Name;
        public MeasurementType MeasurementType;

        public string StrName;
        public string StrAbbreviation;

        public MeasurementUnitProps(LSID name, MeasurementType measurementType)
        {
            Name = name;
            MeasurementType = measurementType;
        }
    }
}
