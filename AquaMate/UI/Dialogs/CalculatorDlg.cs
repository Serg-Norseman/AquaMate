/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CalculatorDlg : EditDialog, ICalculatorView
    {
        private readonly CalculatorPresenter fPresenter;

        public CalculatorDlg()
        {
            InitializeComponent();

            fPresenter = new CalculatorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Calculator);
            btnCalc.Text = Localizer.LS(LSID.Calculate);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fPresenter != null) {
                fPresenter.ChangeSelectedType();
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            fPresenter.Calculate();
        }

        private void CalculatorDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        #region View interface implementation

        IComboBox ICalculatorView.TypeCombo
        {
            get { return GetControlHandler<IComboBox>(cmbType); }
        }

        IPropertyGrid ICalculatorView.ArgsGrid
        {
            get { return GetControlHandler<IPropertyGrid>(pgArgs); }
        }

        ITextBox ICalculatorView.DescriptionField
        {
            get { return GetControlHandler<ITextBox>(txtDescription); }
        }

        #endregion
    }
}
