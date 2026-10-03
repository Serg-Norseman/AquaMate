/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.MCP;
using AquaMate.MCP.Features;
using AquaMate.UI.Components;
using AquaMate.UI.Dialogs;
using BSLib;
using BSLib.Design.Graphics;
using BSLib.Design.Handlers;
using BSLib.Design.IoC;
using BSLib.Design.MVP;
using ZLMKit.MCP;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WFAppHost : AppHost
    {
        public WFAppHost() : base()
        {
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            mcpShutdown();
        }

        protected override void AppInit()
        {
#if NETCOREAPP30
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterControlHandlers();
            RegisterViews();

            mcpStartup();
        }

        protected override void AppRun(AquaMate.UI.IView view)
        {
            var mainForm = view as Form;
            Application.Run(mainForm);
        }

        public override byte[] ImageToByte(IImage image)
        {
            var handler = image as ImageHandler;
            if (handler == null) {
                return null;
            } else {
                var wfImage = handler.Handle;
                return UIHelper.ImageToByte(wfImage, ImageFormat.Jpeg);
            }
        }

        public override IImage ByteToImage(byte[] imageBytes)
        {
            if (imageBytes == null) {
                return null;
            } else {
                var wfImage = UIHelper.ByteToImage(imageBytes);
                return new ImageHandler(wfImage);
            }
        }

        public override IImage LoadImage()
        {
            Image image = null;

            string fileName = UIHelper.GetOpenFile("title", "context",
                          "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.tiff)|*.jpg;*.jpeg;*.png;*.gif;*.tiff",
                          1, "");

            if (string.IsNullOrEmpty(fileName))
                return null;

            if (File.Exists(fileName)) {
                using (FileStream stream = File.Open(fileName, FileMode.Open)) {
                    BinaryReader br = new BinaryReader(stream);
                    byte[] data = br.ReadBytes((int)stream.Length);
                    image = UIHelper.ByteToImage(data);
                }
            }

            return new ImageHandler(image);
        }

        public override void SaveImage(IImage image)
        {
            string fileName = UIHelper.GetSaveFile("title", "context",
                          "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.tiff)|*.jpg;*.jpeg;*.png;*.gif;*.tiff",
                          1, "jpg", "", true);

            if (string.IsNullOrEmpty(fileName))
                return;

            var wfImage = ((ImageHandler)image).Handle;
            wfImage.Save(fileName);
        }

        public static void RegisterControlHandlers()
        {
            ControlsManager.RegisterHandlerType(typeof(Button), typeof(ButtonHandler));
            ControlsManager.RegisterHandlerType(typeof(CheckBox), typeof(CheckBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(ComboBox), typeof(ComboBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(Label), typeof(LabelHandler));
            ControlsManager.RegisterHandlerType(typeof(MaskedTextBox), typeof(MaskedTextBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(NumericUpDown), typeof(NumericBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(ProgressBar), typeof(ProgressBarHandler));
            ControlsManager.RegisterHandlerType(typeof(RadioButton), typeof(RadioButtonHandler));
            ControlsManager.RegisterHandlerType(typeof(TabControl), typeof(TabControlHandler));
            ControlsManager.RegisterHandlerType(typeof(TextBox), typeof(TextBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(TreeView), typeof(TreeViewHandler));
            ControlsManager.RegisterHandlerType(typeof(ToolStripMenuItem), typeof(MenuItemHandler));
            ControlsManager.RegisterHandlerType(typeof(ToolStripComboBox), typeof(ToolStripComboBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(PropertyGrid), typeof(PropertyGridHandler));
            ControlsManager.RegisterHandlerType(typeof(DateTimePicker), typeof(DateTimeBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(PictureBox), typeof(PictureBoxHandler));
            ControlsManager.RegisterHandlerType(typeof(ZListView), typeof(ZListViewHandler));
        }

        public static void RegisterViews()
        {
            IContainer container = Container;
            container.Reset();

            container.Register<IGraphicsProvider, WFGfxProvider>(LifeCycle.Singleton);

            container.Register<IAquariumEditorView, AquariumEditDlg>(LifeCycle.Transient);
            container.Register<ICalculatorView, CalculatorDlg>(LifeCycle.Transient);
            container.Register<ISettingsDialogView, SettingsDlg>(LifeCycle.Transient);
            container.Register<ITankEditorView, TankEditDlg>(LifeCycle.Transient);
        }

        #region MCP Support

        private MCPServer fMCPServer;
        private RuntimeContext fRuntimeContext;

        public RuntimeContext RuntimeContext { get => fRuntimeContext; set => fRuntimeContext = value; }

        public static void mcpShowDialog()
        {
            using (var dlg = new MCPServerForm()) {
                dlg.ShowDialog();
            }
        }

        public bool mcpStartup()
        {
            try {
                RuntimeContext.Initialize();

                fMCPServer = new MCPServer();
                fRuntimeContext = new RuntimeContext(fMCPServer);
                fMCPServer.Context = fRuntimeContext;

                InitFeatures(fMCPServer);
                return true;
            } catch (Exception ex) {
                //Logger.WriteError("GKMCPPlugin.Startup()", ex);
                return false;
            }
        }

        public bool mcpShutdown()
        {
            bool result = true;
            try {
                StopAsync();
            } catch (Exception ex) {
                //Logger.WriteError("GKMCPPlugin.Shutdown()", ex);
                result = false;
            }
            return result;
        }

        internal bool IsRunning()
        {
            return fMCPServer.IsRunning;
        }

        internal async Task StartAsync()
        {
            await fMCPServer.StartAsync(ServerHost, ServerPort, EnableCors, AllowedHosts, VerboseLogging);
        }

        internal async Task StopAsync()
        {
            await fMCPServer.StopAsync();
        }

        internal bool AutoStart = false;
        internal string ServerHost = "localhost";
        internal int ServerPort = 8080;
        internal bool EnableCors = false;
        internal string AllowedHosts = "http://localhost:3000";
        internal bool VerboseLogging = false;

        public void mcpLoadOptions(IniFile ini)
        {
            AutoStart = ini.ReadBool("GKMCPPlugin", "AutoStart", false);
            if (AutoStart) {
                StartAsync();
            }
        }

        public void mcpSaveOptions(IniFile ini)
        {
            ini.WriteBool("GKMCPPlugin", "AutoStart", AutoStart);
        }

        public static void InitFeatures(MCPServer mcpServer)
        {
            mcpServer.InitFeatures(false, false);

            // Aquarium tools
            mcpServer.RegisterTool(new AquariumListTool());
            mcpServer.RegisterTool(new AquariumDetailsTool());

            // Measurement tools
            mcpServer.RegisterTool(new MeasureListTool());

            // Maintenance tools
            mcpServer.RegisterTool(new MaintenanceListTool());
            mcpServer.RegisterTool(new MaintenanceAddTool());

            // Inhabitant tools
            mcpServer.RegisterTool(new InhabitantListTool());

            // Nutrition tools
            mcpServer.RegisterTool(new NutritionListTool());
        }

        #endregion
    }
}
