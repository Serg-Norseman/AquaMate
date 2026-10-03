/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Windows.Forms;
using BSLib.Design.MVP;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonForm : Form, BSLib.Design.MVP.IView
    {
        private readonly ControlsManager fControlsManager;

        public string Title
        {
            get { return this.Text; }
            set { }
        }

        public CommonForm()
        {
            fControlsManager = new ControlsManager(this);
        }

        protected T GetControlHandler<T>(object control) where T : class, IControl
        {
            return fControlsManager.GetControl<T>(control);
        }

        public virtual void SetLocale()
        {
        }

        public object GetControl(string controlName)
        {
            return null;
        }

        public void SetToolTip(object component, string toolTip)
        {
        }
    }
}
