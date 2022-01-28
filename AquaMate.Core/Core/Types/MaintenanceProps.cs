/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
