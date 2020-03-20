﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaLog.Core.Types
{
    public enum TransferType
    {
        /* 00 */ Relocation,
        /* 01 */ Purchase,
        /* 02 */ Sale,
        /* 03 */ Birth,
        /* 04 */ Death,
        /* 05 */ Exclusion,
    }
}
