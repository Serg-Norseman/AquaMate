/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core.Types;
using SQLite;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Transfer : Entity, IEventEntity
    {
        [Indexed]
        public int ItemId { get; set; }

        [Indexed]
        public ItemType ItemType { get; set; }

        [Indexed]
        public DateTime Timestamp { get; set; }

        public TransferType Type { get; set; }
        public string Cause { get; set; }

        // Aquariums
        public int SourceId { get; set; }
        public int TargetId { get; set; }

        public float Quantity { get; set; }

        #region Expenses and Incomes

        public float UnitPrice { get; set; }
        public string Shop { get; set; }

        #endregion


        public override EntityType EntityType
        {
            get {
                return EntityType.Transfer;
            }
        }


        public Transfer()
        {
        }
    }
}
