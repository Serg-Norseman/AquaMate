/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

#if !__MonoCS__

using System;
using System.Windows.Forms;
using NUnit.Extensions.Forms;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CustomFormTest : NUnitFormTest
    {
        public override bool UseHidden
        {
            get { return true; }
        }

        public static Form GetActiveForm(string formName)
        {
            var tester = new FormTester(formName);
            return (tester == null) ? null : (Form)tester.TheObject;
        }

        #region Control Actions

        public static void ClickButton(string name, Form form)
        {
            var tsBtn = new ButtonTester(name, form);
            if (tsBtn.Count > 1) {
                tsBtn[0].FireEvent("Click");
            } else {
                tsBtn.FireEvent("Click");
            }
        }

        public static void ClickButton(string name, string form)
        {
            var tsBtn = new ButtonTester(name, form);
            if (tsBtn.Count > 1) {
                tsBtn[0].FireEvent("Click");
            } else {
                tsBtn.FireEvent("Click");
            }
        }

        public static void ClickToolStripButton(string name, Form form)
        {
            var tsBtn = new ToolStripButtonTester(name, form);
            if (tsBtn.Count > 1) {
                tsBtn[0].FireEvent("Click");
            } else {
                tsBtn.FireEvent("Click");
            }
        }

        public static void ClickToolStripMenuItem(string name, Form form)
        {
            var tsMenuItem = new ToolStripMenuItemTester(name, form);
            tsMenuItem.Click();
        }

        public static void ClickRadioButton(string name, Form form)
        {
            var radBtn = new RadioButtonTester(name, form);
            radBtn.Click();
        }

        public static void SelectTab(string name, Form form, int value)
        {
            var tabCtl = new TabControlTester(name, form);
            tabCtl.SelectTab(value);
        }

        public static void SelectCombo(string name, Form form, int value)
        {
            var combo = new ComboBoxTester(name, form);
            combo.Select(value);
        }

        public static void EnterCombo(string name, Form form, string value)
        {
            var combo = new ComboBoxTester(name, form);
            combo.Enter(value);
        }

        public static void EnterText(string name, Form form, string value)
        {
            var textBox = new TextBoxTester(name, form);
            textBox.Enter(value);
        }

        public static void SelectListView(string name, Form form, int value)
        {
            var lv = new ListViewTester(name, form);
            //lv.Select(value);
            lv.Properties.SetSelectedItem(value);
        }

        /*public static void EnterNumeric(string name, Form form, int value)
        {
            var nud = new NumericUpDownTester(name, form);
            nud.EnterValue(value);
        }*/

        public static void CheckBox(string name, Form form, bool value)
        {
            var chk = new CheckBoxTester(name, form);
            chk.Properties.Checked = value;
        }

        public static void KeyDownForm(string formName, Keys keyData)
        {
            var formTester = new FormTester(formName);
            formTester.FireEvent("KeyDown", new KeyEventArgs(keyData));
        }

        #endregion

        #region Dialogs Handlers

        public static void MessageBox_YesHandler(string name, IntPtr ptr, Form form)
        {
            MessageBoxTester messageBox = new MessageBoxTester(ptr);
            messageBox.SendCommand(MessageBoxTester.Command.Yes);
        }

        public static void MessageBox_NoHandler(string name, IntPtr ptr, Form form)
        {
            MessageBoxTester messageBox = new MessageBoxTester(ptr);
            messageBox.SendCommand(MessageBoxTester.Command.No);
        }

        public static void MessageBox_OkHandler(string name, IntPtr ptr, Form form)
        {
            MessageBoxTester messageBox = new MessageBoxTester(ptr);
            messageBox.SendCommand(MessageBoxTester.Command.OK);
        }

        public static void MessageBox_CancelHandler(string name, IntPtr ptr, Form form)
        {
            MessageBoxTester messageBox = new MessageBoxTester(ptr);
            messageBox.SendCommand(MessageBoxTester.Command.Cancel);
        }

        public static void PrintDialog_Handler(string name, IntPtr ptr, Form form)
        {
            form.Close();
        }

        public static void PrintPreviewDialog_Handler(string name, IntPtr ptr, Form form)
        {
            form.Refresh();
            form.Close();
        }

        public static void OpenFile_Cancel_Handler(string name, IntPtr hWnd, Form form)
        {
            var openDlg = new OpenFileDialogTester(hWnd);
            openDlg.ClickCancel();
        }

        public static void SaveFile_Cancel_Handler(string name, IntPtr hWnd, Form form)
        {
            var saveDlg = new SaveFileDialogTester(hWnd);
            saveDlg.ClickCancel();
        }

        public static void Dialog_Cancel_Handler(string name, IntPtr ptr, Form form)
        {
            ClickButton("btnCancel", form);
        }

        public static void Dialog_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            ClickButton("btnAccept", form);
        }

        #endregion

        #region InputBox Handlers

        public static void InputBox_Add_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtValue", form, "sample add");
            ClickButton("btnAccept", form);
        }

        public static void InputBox_Edit_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtValue", form, "sample edit");
            ClickButton("btnAccept", form);
        }

        #endregion

        protected static NUnitFormTest fFormTest;

        public static void SetModalFormHandler(NUnitFormTest formTest, ModalFormHandler modalFormHandler)
        {
            fFormTest = formTest;
            fFormTest.ModalFormHandler = modalFormHandler;
        }
    }
}

#endif
