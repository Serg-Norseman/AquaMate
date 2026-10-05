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
using AquaMate.Core.Model;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP;

public class AquariumDetailsTool : BaseTool
{
    public AquariumDetailsTool() : base("aquarium_details") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "Provides detailed information about an aquarium by its ID.",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
                    ["aquarium_id"] = new MCPToolProperty { Type = "integer", Description = "ID of the aquarium" }
                },
                Required = new List<string> { "aquarium_id" }
            }
        };
    }

    public override List<MCPContent> ExecuteTool(IRuntimeContext context, JsonElement args)
    {
        var model = context.Get<IModel>();
        if (model == null)
            return MCPContent.CreateSimpleContent("❌ Model is not available.");

        int aquariumId = MCPHelper.GetRequiredInt(args, "aquarium_id");

        var aquarium = model.GetRecord<Aquarium>(aquariumId);
        if (aquarium == null)
            return MCPContent.CreateSimpleContent($"❌ Aquarium not found with ID: {aquariumId}");

        var workTime = model.GetWorkTime(aquarium);
        double waterVolume = model.GetWaterVolume(aquariumId);
        int inhabitantsCount = model.QueryInhabitantsCount(aquariumId);

        double avgChangeDays, lastChangeDays;
        model.GetWaterChangeIntervals(aquariumId, workTime, out avgChangeDays, out lastChangeDays);

        var details = new List<string> {
            $"# Aquarium Details: {aquarium.Name}",
            "",
            "## Basic Information",
            $"- **ID**: {aquarium.Id}",
            $"- **Name**: {aquarium.Name}",
            $"- **Description**: {aquarium.Description ?? "-"}",
            $"- **Brand**: {aquarium.Brand ?? "-"}",
            $"- **Water Type**: {aquarium.WaterType}",
            $"- **Tank Shape**: {aquarium.TankShape}",
            $"- **Tank Volume**: {aquarium.TankVolume} L",
            $"- **Water Volume**: {waterVolume} L",
            $"- **Underfill Height**: {aquarium.UnderfillHeight} cm",
            $"- **Soil Height**: {aquarium.SoilHeight} cm",
            "",
            "## Status Information",
            $"- **Work Time**: {workTime.GetWorkDays()}",
            $"- **Tank State**: {model.GetTankState(workTime)}",
            $"- **Inhabitants Count**: {inhabitantsCount}",
            $"- **Water Changes**: avg={ALCore.GetDecimalStr(avgChangeDays, 1)}d, last={ALCore.GetDecimalStr(lastChangeDays, 1)}d",
            ""
        };

        var lastMeasure = model.QueryLastMeasure(aquarium, "Temperature");
        if (!double.IsNaN(lastMeasure.value)) {
            details.Add("## Last Measurements");
            details.Add($"- **Temperature**: {lastMeasure.value} °C");
            details.Add("");
        }

        return MCPContent.CreateSimpleContent(string.Join("\n", details));
    }
}
