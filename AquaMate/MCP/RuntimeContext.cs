/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Services;

namespace AquaMate.MCP;

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
}
