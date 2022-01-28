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
    public class CommonForm : Form
    {
        private readonly ControlsManager fControlsManager;

        public CommonForm()
        {
            fControlsManager = new ControlsManager();
        }

        protected T GetControlHandler<T>(object control) where T : class, IControl
        {
            return fControlsManager.GetControlHandler<T>(control);
        }
    }
}
