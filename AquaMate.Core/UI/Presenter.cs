/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Presenter<TView>
        where TView : IView
    {
        protected readonly TView fView;


        protected Presenter(TView view)
        {
            fView = view;
            fView.SetLocale();
        }

        public abstract void UpdateView();
    }
}
