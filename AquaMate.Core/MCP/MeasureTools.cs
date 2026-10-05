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

public class MeasureListTool : BaseTool
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

        List<Measure> measures;
        if (aquariumId > 0) {
            measures = model.QueryMeasures(aquariumId);
        } else {
            measures = model.QueryMeasures();
        }
        measures.Sort((x, y) => { return -x.Timestamp.CompareTo(y.Timestamp); });

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
