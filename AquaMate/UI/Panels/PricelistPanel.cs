/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Panels
{
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
