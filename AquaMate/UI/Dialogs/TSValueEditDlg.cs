/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.TSDB;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    public partial class TSValueEditDlg : EditDialog, ITSValueEditorView
    {
        private readonly TSValueEditorPresenter fPresenter;

        public TSValueEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new TSValueEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Value);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblTimestamp.Text = Localizer.LS(LSID.Timestamp);
            lblValue.Text = Localizer.LS(LSID.Value);
        }

        public void SetContext(IModel model, TSValue record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        #region View interface implementation

        IDateTimeBox ITSValueEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpTimestamp); }
        }

        ITextBox ITSValueEditorView.ValueField
        {
            get { return GetControlHandler<ITextBox>(txtValue); }
        }

        #endregion
    }
}
