/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.Core
{
    public class ALException : Exception
    {
        public ALException(string message) : base(message)
        {
        }
    }
}
