/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Windows.Forms;
using BSLib.Design.MVP;

namespace AquaMate.UI.Dialogs
{
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
