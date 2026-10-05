/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
