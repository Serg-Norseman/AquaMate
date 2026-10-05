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

public class SpeciesListTool : BaseTool
{
    public SpeciesListTool() : base("species_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all species in the database",
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

        var species = model.QuerySpecies();

        if (species.Count == 0)
            return MCPContent.CreateSimpleContent("No species found.");

        var lines = new List<string> {
            $"Species ({species.Count} items):",
            "| Id | Name | Scientific Name | Bio Family | Type | Temp | PH | GH | Adult Size | Life Span | Swim Level | Distribution | Habitat | Care Level | Temperament |",
            "|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|"
        };

        foreach (var spec in species) {
            string strType = spec.Type.ToString();
            string strLevel = spec.SwimLevel.ToString();
            string strCareLevel = spec.CareLevel.ToString();
            string strTemperament = spec.Temperament.ToString();

            lines.Add($"|{spec.Id}|{spec.Name}|{spec.ScientificName}|{spec.BioFamily}|{strType}|{spec.GetTempRange()}|{spec.GetPHRange()}|{spec.GetGHRange()}|{ALCore.GetDecimalStr(spec.AdultSize)}|{ALCore.GetDecimalStr(spec.LifeSpan)}|{strLevel}|{spec.Distribution}|{spec.Habitat}|{strCareLevel}|{strTemperament}|");
        }

        return MCPContent.CreateSimpleContent(string.Join("\n", lines));
    }
}
