/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.TSDB;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TSPointEditDlg : EditDialog, ITSPointEditorView
    {
        private readonly TSPointEditorPresenter fPresenter;

        public TSPointEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new TSPointEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.TSDBPoint);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Name);
            lblUoM.Text = Localizer.LS(LSID.Unit);
            lblMin.Text = Localizer.LS(LSID.Min);
            lblMax.Text = Localizer.LS(LSID.Max);
            lblDeviation.Text = Localizer.LS(LSID.Deviation);
        }

        public void SetContext(IModel model, TSPoint record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        ITextBox ITSPointEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        ITextBox ITSPointEditorView.MeasureUnitField
        {
            get { return GetControlHandler<ITextBox>(txtUoM); }
        }

        ITextBox ITSPointEditorView.MinField
        {
            get { return GetControlHandler<ITextBox>(txtMin); }
        }

        ITextBox ITSPointEditorView.MaxField
        {
            get { return GetControlHandler<ITextBox>(txtMax); }
        }

        ITextBox ITSPointEditorView.DeviationField
        {
            get { return GetControlHandler<ITextBox>(txtDeviation); }
        }

        ITextBox ITSPointEditorView.SIDField
        {
            get { return GetControlHandler<ITextBox>(txtSID); }
        }

        #endregion
    }
}
