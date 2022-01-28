/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SnapshotEditDlg : EditDialog, ISnapshotEditorView
    {
        private readonly SnapshotEditorPresenter fPresenter;

        public SnapshotEditDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("btn_cancel.gif");

            fPresenter = new SnapshotEditorPresenter(this);
        }

        public override void SetLocale()
        {
            Text = Localizer.LS(LSID.Snapshot);
            btnAccept.Text = Localizer.LS(LSID.Accept);
            btnCancel.Text = Localizer.LS(LSID.Cancel);

            lblName.Text = Localizer.LS(LSID.Name);
            lblDate.Text = Localizer.LS(LSID.Date);

            btnLoad.Text = Localizer.LS(LSID.Load);
            btnSave.Text = Localizer.LS(LSID.Save);
        }

        public void SetContext(IModel model, Snapshot record)
        {
            fPresenter.SetContext(model, record);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fPresenter.ApplyChanges() ? DialogResult.OK : DialogResult.None;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            fPresenter.LoadImage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fPresenter.SaveImage();
        }

        #region View interface implementation

        ITextBox ISnapshotEditorView.NameField
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        IPictureBox ISnapshotEditorView.PicBox
        {
            get { return GetControlHandler<IPictureBox>(pictureBox1); }
        }

        IDateTimeBox ISnapshotEditorView.TimestampField
        {
            get { return GetControlHandler<IDateTimeBox>(dtpDateTime); }
        }

        #endregion
    }
}
