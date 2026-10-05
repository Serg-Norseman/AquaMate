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
