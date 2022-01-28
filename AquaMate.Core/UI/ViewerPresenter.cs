/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
