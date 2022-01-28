/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core;
using AquaMate.Core.Model;

namespace AquaMate.UI
{
    public abstract class EditorPresenter<TModel, TEntity, TView> : Presenter<TView>
        where TModel : IModel
        where TEntity : IEntity
        where TView : IView
    {
        protected TModel fModel;
        protected TEntity fRecord;


        public TModel Model
        {
            get {
                return fModel;
            }
            set {
                fModel = value;
            }
        }

        public TEntity Record
        {
            get {
                return fRecord;
            }
            set {
                fRecord = value;
            }
        }


        protected EditorPresenter(TView view) : base(view)
        {
        }

        public void SetContext(TModel model, TEntity record)
        {
            fModel = model;
            fRecord = record;
            UpdateView();
        }

        public abstract bool ApplyChanges();
    }
}
