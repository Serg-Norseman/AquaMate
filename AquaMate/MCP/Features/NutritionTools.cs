/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using System.Text.Json;
using AquaMate.Core;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP.Features;

internal class NutritionListTool : BaseTool
{
    public NutritionListTool() : base("nutrition_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all nutrition items in the database with pagination support (20 items per page)",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
                    ["page"] = new MCPToolProperty { Type = "integer", Description = "Page number (1-based, default: 1)" }
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

        var nutritions = model.QueryNutritions();
        if (nutritions.Count == 0)
            return MCPContent.CreateSimpleContent("No nutrition items found.");

        return MCPHelper.PageableTable("nutritions", args, nutritions.Count, (int index) => {
            if (index == -1) {
                return "| Id | Name | Brand | Amount | State | Note |";
            } else {
                var nutrition = nutritions[index];
                string state = nutrition.State.ToString();

                return $"|{nutrition.Id}|{nutrition.Name}|{nutrition.Brand ?? "-"}|" +
                       $"{ALCore.GetDecimalStr(nutrition.Amount)}|{state}|{nutrition.Note ?? "-"}|";
            }
        });
    }
}
