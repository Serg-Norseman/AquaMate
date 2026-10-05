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

public class ScheduleListTool : BaseTool
{
    public ScheduleListTool() : base("schedule_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all schedule items in the database with pagination support (20 items per page)",
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

        IList<Schedule> schedules;
        if (aquariumId > 0) {
            schedules = model.QuerySchedule(aquariumId);
        } else {
            schedules = model.QuerySchedule();
        }

        if (schedules.Count == 0)
            return MCPContent.CreateSimpleContent("No schedule items found.");

        return MCPHelper.PageableTable("schedule items", args, schedules.Count, (int index) => {
            if (index == -1) {
                return "| Id | Aquarium | Date | Event | Reminder | Type | Status | Note |";
            } else {
                var schedule = schedules[index];
                var aquarium = model.GetRecord<Aquarium>(schedule.AquariumId);
                string aquariumName = aquarium?.Name ?? "Unknown";
                string strType = schedule.Type.ToString();
                string strStatus = schedule.Status.ToString();

                return $"|{schedule.Id}|{aquariumName}|{ALCore.GetTimeStr(schedule.Timestamp)}|{schedule.Event}|{schedule.Reminder}|{strType}|{strStatus}|{schedule.Note}|";
            }
        });
    }
}
