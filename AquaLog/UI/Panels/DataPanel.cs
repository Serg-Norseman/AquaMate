﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AquaLog.UI.Components;
using AquaLog.Core;

namespace AquaLog.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class DataPanel : Panel, ILocalizable
    {
        private readonly List<UserAction> fActions;
        protected IBrowser fBrowser;
        protected ALModel fModel;


        public List<UserAction> Actions
        {
            get { return fActions; }
        }

        public IBrowser Browser
        {
            get { return fBrowser; }
            set { fBrowser = value; }
        }

        public ALModel Model
        {
            get { return fModel; }
            set {
                if (fModel != value) {
                    fModel = value;
                    UpdateContent();
                }
            }
        }


        public DataPanel()
        {
            BackColor = SystemColors.Control;
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;

            fActions = new List<UserAction>();
            InitActions();
        }

        public virtual void SetLocale()
        {
        }

        protected void ClearActions()
        {
            fActions.Clear();
        }

        protected virtual void InitActions()
        {
        }

        public virtual void UpdateContent()
        {
        }

        public void UpdateView()
        {
            SetLocale();

            UpdateContent();

            ClearActions();
            InitActions();
        }

        public virtual void SetExtData(object extData)
        {
        }

        public void AddAction(string actionName, LSID btnText, string imageName, EventHandler clickHandler)
        {
            fActions.Add(new UserAction(actionName, btnText, imageName, clickHandler));
        }

        public void AddAction(string actionName, string[] choices, EventHandler changeHandler)
        {
            fActions.Add(new UserAction(actionName, choices, changeHandler));
        }
    }
}
