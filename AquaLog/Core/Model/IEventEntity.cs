﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaLog.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventEntity
    {
        DateTime Timestamp { get; set; }
    }
}
