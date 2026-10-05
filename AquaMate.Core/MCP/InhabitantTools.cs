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

public class InhabitantListTool : BaseTool
{
    public InhabitantListTool() : base("inhabitant_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all inhabitants in the database with pagination support (20 items per page)",
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

        int aquariumId = MCPHelper.GetOptionalInt(args, "aquarium_id", -1);

        var inhabitants = model.PrepareInhabitants(aquariumId);
        if (inhabitants.Count == 0)
            return MCPContent.CreateSimpleContent("No inhabitants found.");

        return MCPHelper.PageableTable("inhabitants", args, inhabitants.Count, (int index) => {
            if (index == -1) {
                return "| Id | Name | Species | Aquarium | Sex | Quantity | State | Inclusion Date | Life Span | Temp | PH | GH |";
            } else {
                var inh = inhabitants[index];
                return $"|{inh.Id}|{inh.Name}|{inh.SpeciesName}|{inh.AquariumName}|{inh.Sex.ToString()}|{inh.Quantity}|{inh.State.ToString()}|{inh.InclusionDate}|{inh.LifeSpan}|{inh.Temp}|{inh.PH}|{inh.GH}|";
            }
        });
    }
}
