/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using AquaMate.UI.Components;
using BSLib;
using BSLib.Design.MVP;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class DataPanel : Panel, IDataPanel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "DataPanel");

        private readonly List<UserAction> fActions;
        private readonly ControlsManager fControlsManager;
        protected IBrowser fBrowser;
        protected IModel fModel;


        public List<UserAction> Actions
        {
            get { return fActions; }
        }

        public IBrowser Browser
        {
            get { return fBrowser; }
            set { fBrowser = value; }
        }

        public IModel Model
        {
            get { return fModel; }
            set {
                if (fModel != value) {
                    fModel = value;
                }
            }
        }


        public DataPanel()
        {
            BackColor = SystemColors.Control;
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;

            fActions = new List<UserAction>();
            fControlsManager = new ControlsManager();
        }

        protected T GetControlHandler<T>(object control) where T : class, IControl
        {
            return fControlsManager.GetControlHandler<T>(control);
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

        public virtual void SelectionChanged(IList<Entity> records)
        {
        }

        public void UpdateView()
        {
            try {
                SetLocale();

                ClearActions();
                InitActions();
                ProcessActions();

                UpdateContent();
            } catch (Exception ex) {
                fLogger.WriteError("UpdateView()", ex);
            }
        }

        public virtual void ProcessActions()
        {
        }

        public virtual void SetExtData(object extData)
        {
        }

        public void AddAction(string actionName, LSID btnText, string imageName, EventHandler clickHandler)
        {
            fActions.Add(new UserAction(actionName, btnText, imageName, clickHandler));
        }

        public void AddMultiSelector(string actionName, string[] choices, EventHandler changeHandler)
        {
            fActions.Add(new UserAction(actionName, choices, true, changeHandler));
        }

        public void AddSingleSelector(string actionName, string[] choices, EventHandler changeHandler)
        {
            fActions.Add(new UserAction(actionName, choices, false, changeHandler));
        }

        public void SetActionEnabled(string actionName, bool enabled)
        {
            foreach (var act in fActions) {
                if (act.Name == actionName) {
                    try {
                        if (act.Control != null) {
                            act.Control.Enabled = enabled;
                        }
                    } catch {
                    }
                    return;
                }
            }
        }

        public virtual void TickTimer()
        {
            // dummy
        }
    }
}
