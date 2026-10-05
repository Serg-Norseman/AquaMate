/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBrowser: IFormView<IModel>
    {
        IModel Model { get; }

        void AddMaintenance(Schedule scheduleRecord);
        void ApplySettings();
        bool CheckDelete(Entity entity);
        bool EditTank(ITank tank);
        void Notify(string text, Schedule record);
        void SetView(MainView mainView, object extData);
        void ShowAbout();
        void ShowSettings(int tabIndex = 0);
        void SwitchVisible();
        void TransferItem(ItemType itemType, int itemId, IDataPanel view);
        void UpdateView();
    }
}
