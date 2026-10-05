/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
