/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
