/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace AquaMate.Core.Types
{
    public class DeviceProps : IProps
    {
        public LSID Name { get; private set; }
        public bool HasMeasurements { get; private set; }
        public Type PropsType { get; private set; }

        public DeviceProps(LSID name, bool hasMeasurements, Type propsType)
        {
            Name = name;
            HasMeasurements = hasMeasurements;
            PropsType = propsType;
        }
    }
}
