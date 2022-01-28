/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;

namespace AquaMate.UI
{
    public interface IView : IDisposable, ILocalizable
    {
    }


    public interface IModalDialog
    {
        /// <summary>
        /// DialogResult ShowDialog() [WinForms], bool? ShowDialog() [WPF]
        /// </summary>
        /// <returns></returns>
        bool ShowModal();
    }


    public interface IFormView<TModel> : IView
        //where TModel : IModel
    {
    }


    public interface IDialogView<TModel> : IFormView<TModel>, IModalDialog
    {
    }


    public interface IEditorView<in TEntity> : IView, IModalDialog
        where TEntity : IEntity
    {
        void SetContext(IModel model, TEntity record);
    }
}
