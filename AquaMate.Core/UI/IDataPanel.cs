/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
