/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PricelistPanel : ListPanel
    {
        public PricelistPanel()
        {
        }

        protected override void UpdateListView()
        {
            var lv = GetControlHandler<IListView>(ListView);
            var records = fModel.QueryTransferExpenses();
            ModelPresenter.FillPricelistLV(lv, fModel, records);
        }
    }
}
