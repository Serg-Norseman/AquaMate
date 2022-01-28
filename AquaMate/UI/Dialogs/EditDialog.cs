/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Windows.Forms;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class EditDialog : CommonForm
    {
        public EditDialog() : base()
        {
        }

        public virtual void SetLocale()
        {
            // dummy
        }

        public bool ShowModal()
        {
            return (base.ShowDialog() == DialogResult.OK);
        }
    }
}
