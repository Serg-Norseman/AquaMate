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
using AquaMate.Core.Types;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP;

public class NutritionListTool : BaseTool
{
    public NutritionListTool() : base("nutrition_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all nutrition items in the database with pagination support (20 items per page)",
            InputSchema = new MCPToolInputSchema {
                Properties = new Dictionary<string, MCPToolProperty> {
                    ["page"] = new MCPToolProperty { Type = "integer", Description = "Page number (1-based, default: 1)" },
                    ["hide_losses"] = new MCPToolProperty { Type = "boolean", Description = "Hide losses (depleted, finished and broken items)", Default = true }
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

        bool hideLosses = MCPHelper.GetOptionalBool(args, "hide_losses", true);

        return MCPHelper.PageableTable("nutritions", args, nutritions.Count, (int index) => {
            if (index == -1) {
                return "| Id | Name | Brand | Amount | State | Note |";
            } else {
                var nutrition = nutritions[index];
                string strState = model.GetItemStateStr(nutrition.Id, ItemType.Nutrition, out ItemState itemState);
                bool fin = (itemState == ItemState.Finished);
                if (fin && hideLosses) return string.Empty;

                return $"|{nutrition.Id}|{nutrition.Name}|{nutrition.Brand ?? "-"}|" +
                       $"{ALCore.GetDecimalStr(nutrition.Amount)}|{strState}|{nutrition.Note ?? "-"}|";
            }
        });
    }
}
