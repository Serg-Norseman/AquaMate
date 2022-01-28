/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
