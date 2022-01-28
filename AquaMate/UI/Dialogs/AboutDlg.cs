/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.Core;

namespace AquaMate.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class AboutDlg : Form
    {
        public AboutDlg()
        {
            InitializeComponent();

            btnClose.Image = UIHelper.LoadResourceImage("btn_accept.gif");

            Text = Localizer.LS(LSID.About);
            btnClose.Text = "Close";
            lblProduct.Text = ALCore.AppName;
            lblVersion.Text = @"Version " + AppHost.GetAppVersion();
            lblCopyright.Text = AppHost.GetAppCopyright();
        }

        private void LabelMail_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null) {
                AppHost.LoadExtFile(lbl.Text);
            }
        }
    }
}
