/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using System.Text.Json;
using AquaMate.Core;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP;

public class AquariumListTool : BaseTool
{
    public AquariumListTool() : base("aquarium_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all aquariums in the database",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
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

        var aquariums = model.QueryAquariums();

        if (aquariums.Count == 0)
            return MCPContent.CreateSimpleContent("No aquariums found.");

        var lines = new List<string> {
            $"Aquariums ({aquariums.Count} items):",
            "| Id | Name | Volume (L) | Water Type | Shape | Tank State |",
            "|---|---|---|---|---|---|"
        };

        foreach (var aquarium in aquariums) {
            string waterType = aquarium.WaterType.ToString();
            string shape = aquarium.TankShape.ToString();
            var workTime = model.GetWorkTime(aquarium);
            string state = model.GetTankState(workTime).ToString();

            lines.Add($"|{aquarium.Id}|{aquarium.Name}|{aquarium.TankVolume}|{waterType}|{shape}|{state}|");
        }

        return MCPContent.CreateSimpleContent(string.Join("\n", lines));
    }
}
