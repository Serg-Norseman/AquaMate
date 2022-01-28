/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core.Types;
using SQLite;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public bool IsNewRecord
        {
            get {
                return (Id == 0);
            }
        }


        public abstract EntityType EntityType
        {
            get;
        }


        protected Entity()
        {
        }
    }
}
