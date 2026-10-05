/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using AquaMate.Core;
using BSLib;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Services;

namespace AquaMate.MCP;

public class MCPSettings
{
    public bool AutoStart = false;
    public string ServerHost = "localhost";
    public int ServerPort = 8080;
    public bool EnableCors = false;
    public string AllowedHosts = "http://localhost:3000";
    public bool VerboseLogging = false;

    public void LoadOptions(IniFile ini)
    {
        AutoStart = ini.ReadBool("MCPServer", "AutoStart", false);
    }

    public void SaveOptions(IniFile ini)
    {
        ini.WriteBool("MCPServer", "AutoStart", AutoStart);
    }
}


public class RuntimeContext : IRuntimeContext
{
    private IModel fModel;
    private readonly FileSystemService fFileSystem;
    private readonly IMCPServer fMCPServer;

    public IModel Model
    {
        get { return fModel; }
        set { fModel = value; }
    }

    public string DefaultTimeFormat { get { return "yyyy-MM-dd HH:mm:ss"; } }

    public bool MemoryEnabled { get; private set; }

    public bool ProfileEnabled { get; private set; }

    public bool TasksEnabled { get; private set; }

    public bool FTSEnabled { get; private set; }

    public IMCPServer MCPServer
    {
        get { return fMCPServer; }
    }

    public RuntimeContext(IMCPServer mcpServer)
    {
        /*bool dbEnabled = true;
        var dbPath = Path.Combine(AppHost.GetAppDataPathStatic(), "gkrag.db");
        if (dbEnabled)
            LLMDatabase.SetDBPath(dbPath);

        var allowedDirectories = "D:\\TEMP\\mem".Split(';');*/

        MemoryEnabled = false;
        ProfileEnabled = false;
        TasksEnabled = false;
        FTSEnabled = false;

        fMCPServer = mcpServer;
        //fFileSystem = new FileSystemService(allowedDirectories);
    }

    public T Get<T>() where T : class
    {
        var typeToResolve = typeof(T);

        if (typeToResolve == typeof(IMCPServer)) {
            return fMCPServer as T;
        } else
        if (typeToResolve == typeof(IFileSystem)) {
            return fFileSystem as T;
        } else
        /*if (typeToResolve == typeof(ILogger)) {
            return Logger.GetLogger() as T;
        } else*/
        if (typeToResolve == typeof(IModel)) {
            return fModel as T;
        }

        return null;
    }

    public static void Initialize()
    {
        //Logger.Init(Path.Combine(AppHost.GetAppDataPathStatic(), "GKMCPPlugin.log"));
        //ZLMKit.MCP.MCPServer.SetLogger(Logger.GetLogger());
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

        // Species tools
        mcpServer.RegisterTool(new SpeciesListTool());

        // Note tools
        mcpServer.RegisterTool(new NoteListTool());

        // Inventory tools
        mcpServer.RegisterTool(new InventoryListTool());

        // Device tools
        mcpServer.RegisterTool(new DeviceListTool());

        // Budget tools
        mcpServer.RegisterTool(new BudgetListTool());

        // Brand tools
        mcpServer.RegisterTool(new BrandListTool());

        // Schedule tools
        mcpServer.RegisterTool(new ScheduleListTool());

        // Shop tools
        mcpServer.RegisterTool(new ShopListTool());

        // Transfer tools
        mcpServer.RegisterTool(new TransferListTool());
    }
}
