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

public class NoteListTool : BaseTool
{
    public NoteListTool() : base("note_list") { }

    public override MCPTool CreateTool()
    {
        return new MCPTool {
            Name = Sign,
            Description = "List all notes in the database with pagination support (20 items per page)",
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

        List<Note> notes;
        if (aquariumId > 0) {
            notes = model.QueryNotes(aquariumId);
        } else {
            notes = model.QueryNotes();
        }
        notes.Sort((x, y) => { return -x.Timestamp.CompareTo(y.Timestamp); });

        if (notes.Count == 0)
            return MCPContent.CreateSimpleContent("No notes found.");

        return MCPHelper.PageableTable("notes", args, notes.Count, (int index) => {
            if (index == -1) {
                return "| Id | Aquarium | Date | Event | Text |";
            } else {
                var note = notes[index];
                var aquarium = model.GetRecord<Aquarium>(note.AquariumId);
                string aquariumName = aquarium?.Name ?? "Unknown";

                return $"|{note.Id}|{aquariumName}|{ALCore.GetTimeStr(note.Timestamp)}|{note.Event}|{note.Content}|";
            }
        });
    }
}
