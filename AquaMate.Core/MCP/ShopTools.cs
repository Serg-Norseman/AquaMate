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

public class ShopListTool : BaseTool
{
    public ShopListTool() : base("shop_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all shops in the database",
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

        var shops = model.QueryShops();

        if (shops.Count == 0)
            return MCPContent.CreateSimpleContent("No shops found.");

        var lines = new List<string> {
            $"Shops ({shops.Count} items):",
            "| Id | Name | Address | Telephone | Web Site | Email |",
            "|---|---|---|---|---|---|"
        };

        foreach (var shop in shops) {
            lines.Add($"|{shop.Id}|{shop.Name}|{shop.Address}|{shop.Telephone}|{shop.WebSite}|{shop.Email}|");
        }

        return MCPContent.CreateSimpleContent(string.Join("\n", lines));
    }
}
