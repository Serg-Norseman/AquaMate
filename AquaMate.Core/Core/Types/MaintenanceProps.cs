/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core.Types
{
    public sealed class MaintenanceProps : IProps
    {
        public LSID Name { get; private set; }
        public int WaterChangeFactor { get; private set; }

        public MaintenanceProps(LSID name, int waterChangeFactor)
        {
            Name = name;
            WaterChangeFactor = waterChangeFactor;
        }
    }
}
