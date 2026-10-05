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
using AquaMate.Core.Types;
using ZLMKit;
using ZLMKit.MCP;
using ZLMKit.Protocols;

namespace AquaMate.MCP;

public class TransferListTool : BaseTool
{
    public TransferListTool() : base("transfer_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all transfers (inhabitants and equipment between aquariums, or purchases and sales) in the database with pagination support (20 items per page)",
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

        var transfers = model.QueryTransfers();
        transfers.Sort((x, y) => { return -x.Timestamp.CompareTo(y.Timestamp); });

        if (transfers.Count == 0)
            return MCPContent.CreateSimpleContent("No transfers found.");

        return MCPHelper.PageableTable("transfers", args, transfers.Count, (int index) => {
            if (index == -1) {
                return "| Id | Date | Brand | Item | Type | Source Tank | Target Tank | Quantity | Unit Price | Shop | Cause |";
            } else {
                var transfer = transfers[index];

                ItemType itemType = transfer.ItemType;
                var itemRec = model.GetRecord(itemType, transfer.ItemId);
                string itName = (itemRec == null) ? string.Empty : itemRec.ToString();

                var brandedItem = itemRec as IBrandedItem;
                string brand = (brandedItem == null) ? "-" : brandedItem.Brand;

                Aquarium aqmSour = model.Cache.Get<Aquarium>(ItemType.Aquarium, transfer.SourceId);
                Aquarium aqmTarg = model.Cache.Get<Aquarium>(ItemType.Aquarium, transfer.TargetId);
                string sourName = (aqmSour == null) ? string.Empty : aqmSour.Name;
                string targName = (aqmTarg == null) ? string.Empty : aqmTarg.Name;

                string strType = transfer.Type.ToString();

                return $"|{transfer.Id}|{ALCore.GetDateStr(transfer.Timestamp)}|{brand}|{itName}|{strType}|{sourName}|{targName}|{transfer.Quantity}|{ALCore.GetDecimalStr(transfer.UnitPrice)}|{transfer.Shop}|{transfer.Cause}|";
            }
        });
    }
}
