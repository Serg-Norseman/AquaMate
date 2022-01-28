/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventEntity
    {
        DateTime Timestamp { get; set; }
    }
}
