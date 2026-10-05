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

namespace AquaMate.UI.Dialogs
{
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
            if (sender is Label lbl) {
                AppHost.LoadExtFile(lbl.Text);
            }
        }
    }
}
