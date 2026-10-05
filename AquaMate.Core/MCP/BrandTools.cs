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

public class BrandListTool : BaseTool
{
    public BrandListTool() : base("brand_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all brands in the database",
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

        var brands = model.QueryBrands();

        if (brands.Count == 0)
            return MCPContent.CreateSimpleContent("No brands found.");

        var lines = new List<string> {
            $"Brands ({brands.Count} items):",
            "| Id | Name | Country | Web Site | Email |",
            "|---|---|---|---|---|"
        };

        foreach (var brand in brands) {
            lines.Add($"|{brand.Id}|{brand.Name}|{brand.Country}|{brand.WebSite}|{brand.Email}|");
        }

        return MCPContent.CreateSimpleContent(string.Join("\n", lines));
    }
}
