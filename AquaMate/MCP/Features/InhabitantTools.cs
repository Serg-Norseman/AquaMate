/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using System.Text.Json;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP.Features;

internal class InhabitantListTool : BaseTool
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

        IList<Inhabitant> inhabitants;
        if (aquariumId > 0) {
            inhabitants = model.QueryInhabitants(aquariumId);
        } else {
            inhabitants = model.QueryInhabitants();
        }

        if (inhabitants.Count == 0)
            return MCPContent.CreateSimpleContent("No inhabitants found.");

        return MCPHelper.PageableTable("inhabitants", args, inhabitants.Count, (int index) => {
            if (index == -1) {
                return "| Id | Name | Species | Aquarium | Sex | Quantity | State |";
            } else {
                var inhabitant = inhabitants[index];
                var species = model.GetRecord<Species>(inhabitant.SpeciesId);
                var aquarium = model.GetRecord<Aquarium>(inhabitant.AquariumId);
                string speciesName = species?.Name ?? "Unknown";
                string aquariumName = aquarium?.Name ?? "Unknown";
                string sex = inhabitant.Sex.ToString();
                string state = inhabitant.State.ToString();
                SpeciesType speciesType = model.GetSpeciesType(inhabitant.SpeciesId);
                ItemType itemType = ALCore.GetItemType(speciesType);
                int quantity = model.QueryInhabitantsCount(inhabitant.Id, itemType);

                return $"|{inhabitant.Id}|{inhabitant.Name}|{speciesName}|{aquariumName}|{sex}|{quantity}|{state}|";
            }
        });
    }
}
