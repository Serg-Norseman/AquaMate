/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
