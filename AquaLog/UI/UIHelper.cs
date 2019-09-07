﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AquaLog.Core;
using AquaLog.Core.Types;
using Microsoft.Win32;

namespace AquaLog.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class UIHelper
    {
        public static void FillAquariumsCombo(ComboBox comboBox, ALModel model, int selectedId)
        {
            comboBox.Items.Clear();
            var aquariums = model.QueryAquariums();
            foreach (var aqm in aquariums) {
                if (selectedId != 0 || !aqm.IsInactive()) {
                    comboBox.Items.Add(aqm);
                }
            }
            comboBox.SelectedItem = aquariums.FirstOrDefault(aqm => aqm.Id == selectedId);
        }

        public static void FillItemStatesCombo(ComboBox comboBox, ItemType itemType, ItemState selectedState)
        {
            ItemProps props = ALData.ItemTypes[(int)itemType];
            comboBox.Items.Clear();
            for (ItemState state = ItemState.Unknown; state <= ItemState.Broken; state++) {
                if (state == ItemState.Unknown || props.States.Contains(state)) {
                    comboBox.AddItem<ItemState>(Localizer.LS(ALData.ItemStates[(int)state]), state);
                }
            }
            comboBox.SetSelectedTag<ItemState>(selectedState);
        }

        public static bool ShowQuestionYN(string msg)
        {
            return MessageBox.Show(msg, ALCore.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static ListView CreateListView(string name)
        {
            var listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.Name = name;
            listView.HideSelection = false;
            listView.LabelEdit = false;
            listView.FullRowSelect = true;
            listView.View = View.Details;
            return listView;
        }

        public static ListViewItem GetSelectedItem(ListView listView)
        {
            ListViewItem result;

            if (listView.SelectedItems.Count <= 0) {
                result = null;
            } else {
                result = (listView.SelectedItems[0] as ListViewItem);
            }

            return result;
        }

        public static Color CreateColor(int rgb)
        {
            int red = (rgb >> 16) & 0xFF;
            int green = (rgb >> 8) & 0xFF;
            int blue = (rgb >> 0) & 0xFF;
            return Color.FromArgb(red, green, blue);
        }

        public static Stream LoadResourceStream(string resName)
        {
            return LoadResourceStream(typeof(ALCore), resName);
        }

        public static Stream LoadResourceStream(Type baseType, string resName)
        {
            Assembly assembly = baseType.Assembly;
            return assembly.GetManifestResourceStream(resName);
        }

        public static Bitmap LoadResourceImage(string resName)
        {
            return new Bitmap(LoadResourceStream("AquaLog.Resources." + resName));
        }

        #region Application's autorun

        public static void RegisterStartup()
        {
            if (!IsStartupItem()) {
                RegistryKey rkApp = GetRunKey();
                string trayPath = ALCore.GetAppPath() + "AquaLog.exe";
                rkApp.SetValue(ALCore.AppName, trayPath);
            }
        }

        public static void UnregisterStartup()
        {
            if (IsStartupItem()) {
                RegistryKey rkApp = GetRunKey();
                rkApp.DeleteValue(ALCore.AppName, false);
            }
        }

        public static bool IsStartupItem()
        {
            RegistryKey rkApp = GetRunKey();
            return (rkApp.GetValue(ALCore.AppName) != null);
        }

        private static RegistryKey GetRunKey()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            return rkApp;
        }

        #endregion
    }
}
