/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataPanel : ILocalizable
    {
        void UpdateContent();
    }
}
