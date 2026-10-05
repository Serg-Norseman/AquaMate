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

public class DeviceListTool : BaseTool
{
    public DeviceListTool() : base("device_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all devices in the database with pagination support (20 items per page)",
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

        IList<Device> devices;
        if (aquariumId > 0) {
            devices = model.QueryDevices(aquariumId);
        } else {
            devices = model.QueryDevices();
        }

        if (devices.Count == 0)
            return MCPContent.CreateSimpleContent("No devices found.");

        return MCPHelper.PageableTable("devices", args, devices.Count, (int index) => {
            if (index == -1) {
                return "| Id | Aquarium | Name | Brand | Type | Enabled | Digital | Power | Work Time | State | Value |";
            } else {
                var device = devices[index];
                var aquarium = model.GetRecord<Aquarium>(device.AquariumId);
                string aquariumName = aquarium?.Name ?? "Unknown";
                string strType = device.Type.ToString();

                ItemState itemState;
                string strState = model.GetItemStateStr(device.Id, ItemType.Device, out itemState);

                bool fin = (itemState == ItemState.Broken);

                double curValue = model.GetCurrentValue(device.PointId);
                string strVal = ALCore.GetDecimalStr(curValue);

                return $"|{device.Id}|{aquariumName}|{device.Name}|{device.Brand}|{strType}|{device.Enabled}|{device.Digital}|{ALCore.GetDecimalStr(device.Power)}|{ALCore.GetDecimalStr(device.WorkTime)}|{strState}|{strVal}|";
            }
        });
    }
}
