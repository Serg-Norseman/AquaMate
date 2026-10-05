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

public class BudgetListTool : BaseTool
{
    public BudgetListTool() : base("budget_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all budget items (purchases and sales of everything) in the database with pagination support (20 items per page)",
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

        var transfers = model.QueryTransferExpenses();

        if (transfers.Count == 0)
            return MCPContent.CreateSimpleContent("No budget items found.");

        return MCPHelper.PageableTable("budget items", args, transfers.Count, (int index) => {
            if (index == -1) {
                return "| Id | Date | Type | Brand | Item | Quantity | Unit Price | Sum | Shop | State |";
            } else {
                var transfer = transfers[index];

                int factor = 0;
                switch (transfer.Type) {
                    case TransferType.Purchase:
                        factor = -1;
                        break;
                    case TransferType.Sale:
                        factor = +1;
                        break;
                }

                double sum = 0.0d;
                if (factor != 0) {
                    sum = (transfer.Quantity * transfer.UnitPrice) * factor;
                }

                ItemType itemType = transfer.ItemType;
                var itemRec = model.GetRecord(itemType, transfer.ItemId);
                string itName = (itemRec == null) ? string.Empty : itemRec.ToString();

                ItemState itemState;
                string strState = model.GetItemStateStr(transfer.ItemId, itemType, out itemState);

                var brandedItem = itemRec as IBrandedItem;
                string brand = (brandedItem == null) ? "-" : brandedItem.Brand;

                return $"|{transfer.Id}|{ALCore.GetDateStr(transfer.Timestamp)}|{transfer.ItemType.ToString()}|{brand}|{itName}|{transfer.Quantity}|{ALCore.GetDecimalStr(transfer.UnitPrice)}|{ALCore.GetDecimalStr(sum)}|{transfer.Shop}|{strState}|";
            }
        });
    }
}
