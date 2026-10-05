/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using BSLib;

namespace AquaMate.Core.Types
{
    public sealed class ItemProps
    {
        public LSID Name { get; private set; }
        public EnumSet<ItemState> States { get; private set; }
        public ItemState ExclusionState { get; private set; }

        public ItemProps(LSID name, EnumSet<ItemState> states, ItemState exclusionState)
        {
            Name = name;
            States = states;
            ExclusionState = exclusionState;
        }
    }
}
