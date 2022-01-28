/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core;

namespace AquaMate.UI.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserAction
    {
        public readonly string Name;
        public readonly LSID BtnText;
        public readonly Image Image;
        public readonly EventHandler Click;

        public readonly string[] Choices;
        public readonly bool MultiChoice;

        public Control Control;

        public UserAction(string actionName, LSID btnText, string imageName, EventHandler clickHandler)
        {
            Name = actionName;
            BtnText = btnText;
            Image = string.IsNullOrEmpty(imageName) ? null : UIHelper.LoadResourceImage(imageName);
            Click = clickHandler;
        }

        public UserAction(string actionName, string[] choices, bool multiChoice, EventHandler changeHandler)
        {
            Name = actionName;
            Choices = choices;
            MultiChoice = multiChoice;
            Click = changeHandler;
        }
    }
}
