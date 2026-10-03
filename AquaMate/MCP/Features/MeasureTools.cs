/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using System.Text.Json;
using AquaMate.Core;
using AquaMate.Core.Model;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP.Features;

internal class MeasureListTool : BaseTool
{
    public MeasureListTool() : base("measure_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all measurements in the database with pagination support (20 items per page)",
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

        IList<Measure> measures;
        if (aquariumId > 0) {
            measures = model.QueryMeasures(aquariumId);
        } else {
            measures = model.QueryMeasures();
        }

        if (measures.Count == 0)
            return MCPContent.CreateSimpleContent("No measurements found.");

        return MCPHelper.PageableTable("measures", args, measures.Count, (int index) => {
            if (index == -1) {
                return "| Id | Aquarium | Timestamp | Temp (°C) | pH | GH | KH | NO3 | NO2 | Cl2 | CO2 |";
            } else {
                var measure = measures[index];
                var aquarium = model.GetRecord<Aquarium>(measure.AquariumId);
                string aquariumName = aquarium?.Name ?? "Unknown";

                return $"|{measure.Id}|{aquariumName}|{ALCore.GetTimeStr(measure.Timestamp)}|" +
                       $"{ALCore.GetDecimalStr(measure.Temperature, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.pH, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.GH, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.KH, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.NO3, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.NO2, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.Cl2, 2, true)}|" +
                       $"{ALCore.GetDecimalStr(measure.CO2, 2, true)}|";
            }
        });
    }
}
