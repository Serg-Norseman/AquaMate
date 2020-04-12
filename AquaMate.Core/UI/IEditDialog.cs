﻿/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditDialog<TEntity> : IDisposable, IDialogView, IView<IModel>
        where TEntity : IEntity
    {
        void SetContext(IModel model, TEntity record);
    }
}
