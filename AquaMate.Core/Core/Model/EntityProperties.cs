/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core.Model
{
    public interface IEntityProperties
    {
        void SetPropNames();
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityProperties : IEntityProperties
    {
        public EntityProperties Clone()
        {
            return (EntityProperties)this.MemberwiseClone();
        }

        public virtual void SetPropNames()
        {
            // dummy
        }
    }
}
