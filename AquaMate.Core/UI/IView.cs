/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
