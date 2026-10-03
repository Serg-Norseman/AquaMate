/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Collections.Generic;
using System.Drawing;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using Elistia.DotNetRtfWriter;

namespace AquaMate.Core.Export
{
    /// <summary>
    /// 
    /// </summary>
    public static class RTFLogBook
    {
        private static RtfDocument fDocument;


        public static void Generate(IModel model, Aquarium aquarium, string fileName)
        {
            if (model == null || aquarium == null) return;

            fDocument = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait, Lcid.English);
            try {
                var titleFont = CreateFont("", 16.0f, true, false, Color.Black);
                var textFont = CreateFont("", 12.0f, false, false, Color.Black);
                var dateFont = CreateFont("", 12.0f, true, true, Color.Black);

                AddParagraph(Localizer.LS(LSID.LogBook), titleFont, Align.Center, 0.0f, 16.0f);

                var events = new List<IEventEntity>();
                events.AddRange(model.QueryTransfers(aquarium.Id));
                events.AddRange(model.QueryNotes(aquarium.Id));
                events.AddRange(model.QueryMaintenances(aquarium.Id));
                events.AddRange(model.QueryMeasures(aquarium.Id));
                events.Sort((x, y) => { return x.Timestamp.CompareTo(y.Timestamp); });

                string prevDate = string.Empty, curDate;
                foreach (IEventEntity evnt in events) {
                    curDate = ALCore.GetDateStr(evnt.Timestamp);
                    if (!prevDate.Equals(curDate)) {
                        AddParagraph(curDate, dateFont, Align.Left, 6.0f, 6.0f);

                        prevDate = curDate;
                    }

                    if (evnt is Transfer) {
                        Transfer rec = (Transfer)evnt;
                        string strType = Localizer.LS(ALData.TransferTypes[(int)rec.Type]);
                        string itName = model.GetRecordName(rec.ItemType, rec.ItemId);
                        if (rec.Type == TransferType.Purchase || rec.Type == TransferType.Sale) {
                            AddListItem(string.Format("{0}: {1} ({2} x {3:C2})", strType, itName, rec.Quantity, rec.UnitPrice), textFont);
                        } else {
                            AddListItem(string.Format("{0}: {1} ({2})", strType, itName, rec.Quantity), textFont);
                        }
                    }

                    if (evnt is Note) {
                        Note note = (Note)evnt;
                        AddListItem(string.Format("{0}: {1}", note.Event, note.Content), textFont);
                    }

                    if (evnt is Maintenance) {
                        Maintenance mnt = (Maintenance)evnt;
                        string strType = Localizer.LS(ALData.MaintenanceTypes[(int)mnt.Type].Name);
                        string notes = (string.IsNullOrEmpty(mnt.Note)) ? string.Empty : " (" + mnt.Note + ")";
                        AddListItem(string.Format("{0}: {1}{2}", strType, ALCore.GetDecimalStr(mnt.Value), notes), textFont);
                    }

                    if (evnt is Measure) {
                        Measure msr = (Measure)evnt;
                        AddListItem(string.Format("{0}: {1}", Localizer.LS(LSID.Measure), msr.ToString()), textFont);
                    }
                }
            } finally {
                fDocument.Save(fileName);
            }
        }

        #region RTF Utilities

        private static void AddParagraph(string text, FontStruct font, Align alignment,
            float spacingBefore, float spacingAfter,
            float indent = 0.0f, bool keepTogether = false)
        {
            RtfParagraph par = fDocument.AddParagraph();
            par.Alignment = alignment;

            var margins = par.Margins;
            margins[Direction.Top] = spacingBefore;
            margins[Direction.Bottom] = spacingAfter;

            AddParagraphChunk(par, text, font);
        }

        private static void AddParagraphChunk(RtfParagraph par, string text, FontStruct fntStr)
        {
            par.DefaultCharFormat.Font = fntStr.FD;

            int beg = par.Text.Length;
            par.Text.Append(text);
            int end = par.Text.Length - 1;

            RtfCharFormat fmt = par.AddCharFormat(beg, end);
            fmt.Font = fntStr.FD;
            fmt.FgColor = fntStr.Color;
            fmt.FontSize = fntStr.Size;
            if (fntStr.Bold) fmt.FontStyle.AddStyle(FontStyleFlag.Bold);
            if (fntStr.Underline) fmt.FontStyle.AddStyle(FontStyleFlag.Underline);
        }

        private static void AddListItem(string text, FontStruct fntStr)
        {
            RtfParagraph par = fDocument.AddParagraph();

            var symFont = CreateFont("Symbol", fntStr.Size, fntStr.Bold, fntStr.Underline, fntStr.OriginalColor);

            AddParagraphChunk(par, "\t· ", symFont);
            AddParagraphChunk(par, text, fntStr);
        }

        private static string Clr2Hex(Color color)
        {
            return $"{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private static FontStruct CreateFont(string name, float size, bool bold, bool underline, Color color)
        {
            if (string.IsNullOrEmpty(name)) name = "Times New Roman";

            FontStruct fntStr = new FontStruct();
            fntStr.FD = fDocument.CreateFont(name);
            fntStr.OriginalColor = color;
            fntStr.Color = fDocument.CreateColor(new RtfColor(Clr2Hex(color)));
            fntStr.Size = size;
            fntStr.Bold = bold;
            fntStr.Underline = underline;
            return fntStr;
        }

        private class FontStruct
        {
            public FontDescriptor FD;
            public float Size;
            public Color OriginalColor;
            public ColorDescriptor Color;
            public bool Bold;
            public bool Underline;
        }

        #endregion
    }
}
