/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using AquaMate.Logging;
using AquaMate.UI;
using BSLib;
using BSLib.Design.Graphics;
using BSLib.Design.IoC;
using Microsoft.Win32;

namespace AquaMate.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AppHost
    {
        public static bool TEST_MODE = false;

        private AquaMate.UI.IView fCurrentView;
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "AppHost<>");

        private static string fAppDataPath = null;
        private static AppHost fInstance;
        private static IocContainer fIocContainer;
        private static IGraphicsProvider fGfxProvider;


        public static AppHost Instance
        {
            get {
                if (fInstance == null)
                    throw new ALException("Tried to call the singleton instance of the AppHost before the AppHost started.");

                return fInstance;
            }
        }

        public static IocContainer Container
        {
            get { return fIocContainer; }
        }

        public static IGraphicsProvider GfxProvider
        {
            get {
                if (fGfxProvider == null) {
                    fGfxProvider = fIocContainer.Resolve<IGraphicsProvider>();
                }
                return fGfxProvider;
            }
        }


        static AppHost()
        {
            fIocContainer = new IocContainer();
        }

        public AppHost()
        {
        }

        protected virtual void AppInit()
        {
        }

        protected virtual void AppRun(AquaMate.UI.IView view)
        {
        }

        public static TTypeToResolve ResolveDialog<TTypeToResolve>(params object[] parameters) where TTypeToResolve : AquaMate.UI.IView
        {
            return Container.Resolve<TTypeToResolve>(parameters);
        }

        public void ShowCalculator()
        {
            try {
                using (var dlg = ResolveDialog<ICalculatorView>()) {
                    dlg.ShowModal();
                }
            } catch (Exception ex) {
                fLogger.WriteError("ShowCalculator()", ex);
            }
        }

        public void ShowSettings(IModel model, int tabIndex = 0)
        {
            try {
                using (var dlg = ResolveDialog<ISettingsDialogView>()) {
                    dlg.Settings = ALSettings.Instance;
                    dlg.SelectTab(tabIndex);

                    if (dlg.ShowModal()) {
                        var browser = fCurrentView as IBrowser;
                        if (browser != null) {
                            browser.ApplySettings();
                            browser.UpdateView();
                        }
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("ShowSettings()", ex);
            }
        }

        public virtual byte[] ImageToByte(IImage image)
        {
            return null;
        }

        public virtual IImage ByteToImage(byte[] imageBytes)
        {
            return null;
        }

        public virtual IImage LoadImage()
        {
            return null;
        }

        public virtual void SaveImage(IImage image)
        {
            // dummy
        }

        private static T CreateInstance<T>() where T : AppHost
        {
            try {
                var constructors = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                return (T)constructors.Single().Invoke(null);
            } catch {
                throw new ALException(typeof(T) + " must have a parameterless constructor and all constructors have to be NonPublic.");
            }
        }

        public static void InitInstance<TAH>()
            where TAH : AppHost
        {
            fInstance = CreateInstance<TAH>();
        }

        /// <summary>
        /// Starts the AppHost and creates a singleton for this class.
        /// </summary>
        public static void Start<TAH, TMF>(string[] args)
            where TAH : AppHost
            where TMF : class, AquaMate.UI.IView, new()
        {
            if (fInstance != null) return;

            CheckPortable(args);

            bool isFirstInstance;
            using (Mutex mtx = new Mutex(true, ALCore.AppName, out isFirstInstance)) {
                if (isFirstInstance) {
                    fInstance = CreateInstance<TAH>();

                    fInstance.AppInit();

                    TMF view = new TMF();
                    if (view != null) {
                        fInstance.fCurrentView = view;
                        fInstance.AppRun(view);
                    }
                }
            }
        }

        public static void LoadExtFile(string fileName, string args = "")
        {
            #if !CI_MODE
            if (File.Exists(fileName)) {
                Process.Start(new ProcessStartInfo("file://"+fileName) { UseShellExecute = true, Arguments = args });
            } else {
                Process.Start(fileName);
            }
            #endif
        }

        #region Application Runtime

        private static Assembly GetAssembly()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            if (asm == null) {
                asm = Assembly.GetExecutingAssembly();
            }
            return asm;
        }

        public static string GetAppPath()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            if (asm == null) {
                asm = Assembly.GetExecutingAssembly();
            }

            Module[] mods = asm.GetModules();
            string fn = mods[0].FullyQualifiedName;
            return Path.GetDirectoryName(fn) + Path.DirectorySeparatorChar;
        }

        public static string GetAppCopyright()
        {
            var attr = ReflectionHelper.GetAssemblyAttribute<AssemblyCopyrightAttribute>(GetAssembly());
            return (attr == null) ? string.Empty : attr.Copyright;
        }

        public static Version GetAppVersion()
        {
            return GetAssembly().GetName().Version;
        }

        public static string GetAppDataPath()
        {
            string path;

            if (TEST_MODE) {
                path = Environment.GetEnvironmentVariable("TEMP") + Path.DirectorySeparatorChar;
            } else {
                if (string.IsNullOrEmpty(fAppDataPath)) {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                        Path.DirectorySeparatorChar + ALCore.AppName + Path.DirectorySeparatorChar;
                } else {
                    path = fAppDataPath;
                }
            }

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }

        public static string GetLocalesPath()
        {
            string appPath = GetAppPath();
            return appPath + "locales" + Path.DirectorySeparatorChar;
        }

        public static void CheckPortable(string[] args)
        {
            const string HomeDirArg = "-homedir:";
            const string LocalAppDataFolder = "appdata\\";

            string appPath = GetAppPath();

            string homedir = "";
            if (args != null && args.Length > 0) {
                foreach (var arg in args) {
                    if (arg.StartsWith(HomeDirArg)) {
                        homedir = arg.Remove(0, HomeDirArg.Length);
                    }
                }
            }

            if (!string.IsNullOrEmpty(homedir)) {
                string path = Path.Combine(appPath, homedir);
                if (Directory.Exists(path)) {
                    fAppDataPath = Path.Combine(path, LocalAppDataFolder);
                }
            }

            if (string.IsNullOrEmpty(fAppDataPath)) {
                string path = Path.Combine(appPath, LocalAppDataFolder);
                if (Directory.Exists(path)) {
                    fAppDataPath = path;
                }
            }
        }

        #endregion

        #region Application's autorun

        public static void RegisterStartup()
        {
            if (!IsStartupItem()) {
                RegistryKey rkApp = GetRunKey();
                string trayPath = AppHost.GetAppPath() + "AquaMate.exe";
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

        public static bool SwitchAutorun()
        {
            if (IsStartupItem()) {
                UnregisterStartup();
            } else {
                RegisterStartup();
            }
            return IsStartupItem();
        }

        #endregion
    }
}
