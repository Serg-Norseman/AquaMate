/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.Core.Types
{
    public class InventoryProps : IProps
    {
        public LSID Name { get; private set; }
        public Type PropsType { get; private set; }

        public InventoryProps(LSID name, Type propsType)
        {
            Name = name;
            PropsType = propsType;
        }
    }
}
