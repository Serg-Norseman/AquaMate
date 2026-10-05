/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Text.Json;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP;

public class MaintenanceListTool : BaseTool
{
    public MaintenanceListTool() : base("maintenance_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all maintenance records in the database with pagination support (20 items per page)",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
                    ["page"] = new MCPToolProperty { Type = "integer", Description = "Page number (1-based, default: 1)" },
                    ["aquarium_id"] = new MCPToolProperty { Type = "integer", Description = "Filter by aquarium ID (optional)" }
                },
                Required = new List<string> { }
            }
        };
    }

    public override List<MCPContent> ExecuteTool(IRuntimeContext context, JsonElement args)
    {
        var model = context.Get<IModel>();
        if (model == null)
            return MCPContent.CreateSimpleContent("❌ Model is not available.");

        int page = MCPHelper.GetOptionalInt(args, "page", 1);
        int aquariumId = MCPHelper.GetOptionalInt(args, "aquarium_id", -1);

        List<Maintenance> maintenances;
        if (aquariumId > 0) {
            maintenances = model.QueryMaintenances(aquariumId);
        } else {
            maintenances = model.QueryMaintenances();
        }
        maintenances.Sort((x, y) => { return -x.Timestamp.CompareTo(y.Timestamp); });

        if (maintenances.Count == 0)
            return MCPContent.CreateSimpleContent("No maintenance records found.");

        return MCPHelper.PageableTable("maintenances", args, maintenances.Count, (int index) => {
            if (index == -1) {
                return "| Id | Aquarium | Timestamp | Type | Value | Note |";
            } else {
                var maintenance = maintenances[index];
                var aquarium = model.GetRecord<Aquarium>(maintenance.AquariumId);
                string aquariumName = aquarium?.Name ?? "Unknown";
                string type = maintenance.Type.ToString();

                return $"|{maintenance.Id}|{aquariumName}|{ALCore.GetTimeStr(maintenance.Timestamp)}|" +
                       $"{type}|{ALCore.GetDecimalStr(maintenance.Value)}|{maintenance.Note ?? "-"}|";
            }
        });
    }
}


public class MaintenanceAddTool : BaseTool
{
    public MaintenanceAddTool() : base("maintenance_add") { }

    public override MCPTool CreateTool()
    {
        var maintenanceTypes = new List<string>();
        foreach (var mt in ALData.MaintenanceTypes) {
            maintenanceTypes.Add(mt.Name.ToString());
        }

        return new MCPTool {
            Name = Sign,
            Description = "Add a new maintenance record",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
                    ["aquarium_id"] = new MCPToolProperty { Type = "integer", Description = "ID of the aquarium" },
                    ["type"] = new MCPToolProperty { Type = "string", Description = "Type of maintenance", Enum = maintenanceTypes },
                    ["value"] = new MCPToolProperty { Type = "number", Description = "Value (e.g., amount of water changed)" },
                    ["note"] = new MCPToolProperty { Type = "string", Description = "Additional notes (optional)" },
                    ["timestamp"] = new MCPToolProperty { Type = "string", Description = "Timestamp (optional, ISO format)" }
                },
                Required = new List<string> { "aquarium_id", "type", "value" }
            }
        };
    }

    public static double GetRequiredDbl(JsonElement args, string argName)
    {
        if (!args.TryGetProperty(argName, out var value) || value.ValueKind != JsonValueKind.Number) {
            throw new ArgumentException("Missing required argument: " + argName);
        }

        return value.GetDouble();
    }

    public override List<MCPContent> ExecuteTool(IRuntimeContext context, JsonElement args)
    {
        var model = context.Get<IModel>();
        if (model == null)
            return MCPContent.CreateSimpleContent("❌ Model is not available.");

        int aquariumId = MCPHelper.GetRequiredInt(args, "aquarium_id");
        string typeStr = MCPHelper.GetRequiredStr(args, "type");
        double value = GetRequiredDbl(args, "value");
        string note = MCPHelper.GetOptionalStr(args, "note", "");
        string timestampStr = MCPHelper.GetOptionalStr(args, "timestamp", null);

        var aquarium = model.GetRecord<Aquarium>(aquariumId);
        if (aquarium == null)
            return MCPContent.CreateSimpleContent($"❌ Aquarium not found with ID: {aquariumId}");

        MaintenanceType type = MaintenanceType.WaterReplaced;
        bool found = false;
        for (int i = 0; i < ALData.MaintenanceTypes.Length; i++) {
            if (ALData.MaintenanceTypes[i].Name.ToString().Equals(typeStr, StringComparison.OrdinalIgnoreCase)) {
                type = (MaintenanceType)i;
                found = true;
                break;
            }
        }

        if (!found) {
            return MCPContent.CreateSimpleContent($"❌ Unknown maintenance type: '{typeStr}'");
        }

        var maintenance = new Maintenance();
        maintenance.AquariumId = aquariumId;
        maintenance.Type = type;
        maintenance.Value = value;
        maintenance.Note = note;

        if (!string.IsNullOrEmpty(timestampStr)) {
            if (DateTime.TryParse(timestampStr, out DateTime timestamp)) {
                maintenance.Timestamp = timestamp;
            } else {
                return MCPContent.CreateSimpleContent($"❌ Invalid timestamp format: '{timestampStr}'");
            }
        } else {
            maintenance.Timestamp = DateTime.Now;
        }

        model.AddRecord(maintenance);

        return MCPContent.CreateSimpleContent($"✅ Maintenance record added with ID: {maintenance.Id}");
    }
}
