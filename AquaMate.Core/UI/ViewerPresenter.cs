/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using AquaMate.Core;

namespace AquaMate.UI
{
    public abstract class ViewerPresenter<TModel, TView> : Presenter<TView>
        where TModel : IModel
        where TView : IView
    {
        protected TModel fModel;


        public TModel Model
        {
            get {
                return fModel;
            }
            set {
                fModel = value;
            }
        }


        protected ViewerPresenter(TView view) : base(view)
        {
        }

        public void SetContext(TModel model)
        {
            fModel = model;
            UpdateView();
        }
    }
}
