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

public class InventoryListTool : BaseTool
{
    public InventoryListTool() : base("inventory_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all inventory items in the database with pagination support (20 items per page)",
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

        var inventoryItems = model.QueryInventory();

        if (inventoryItems.Count == 0)
            return MCPContent.CreateSimpleContent("No inventory items found.");

        return MCPHelper.PageableTable("inventory items", args, inventoryItems.Count, (int index) => {
            if (index == -1) {
                return "| Id | Name | Brand | Type | Note | State |";
            } else {
                var item = inventoryItems[index];
                string strType = item.Type.ToString();

                ItemType itemType = ALCore.GetItemType(item.Type);
                ItemState itemState;
                string strState = model.GetItemStateStr(item.Id, itemType, out itemState);

                bool fin = (itemState == ItemState.Finished || itemState == ItemState.Broken);

                return $"|{item.Id}|{item.Name}|{item.Brand}|{strType}|{item.Note}|{strState}|";
            }
        });
    }
}
