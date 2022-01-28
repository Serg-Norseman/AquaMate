/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;

namespace AquaMate.UI.Panels
{
    /// <summary>
    /// 
    /// </summary>
    public class AquaAnalysisPanel : ListPanel
    {
        private Aquarium fAquarium;

        public AquaAnalysisPanel() : base()
        {
        }

        public override void SetExtData(object extData)
        {
            fAquarium = (Aquarium)extData;
            UpdateContent();
        }

        protected override void InitActions()
        {
            AddAction("Export", LSID.Export, "btn_excel.gif", ExportHandler);
        }

        protected override void UpdateListView()
        {
            ListView.Clear();

            ListView.Columns.Add(Localizer.LS(LSID.Date), 120, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Type), 100, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.Value), 100, HorizontalAlignment.Right);
            ListView.Columns.Add(Localizer.LS(LSID.Note), 250, HorizontalAlignment.Left);
            ListView.Columns.Add(Localizer.LS(LSID.WaterVolume), 100, HorizontalAlignment.Right);
            ListView.Columns.Add("% change", 100, HorizontalAlignment.Right);
            ListView.Columns.Add("period change", 100, HorizontalAlignment.Right);

            ListView.Columns.Add("Temp (°C)", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("NO3 (mg/l)", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("GH (°d)", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("KH (°d)", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("pH", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("CO2", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("NHtot", 60, HorizontalAlignment.Right);
            ListView.Columns.Add("PO4", 60, HorizontalAlignment.Right);

            if (fAquarium != null) {
                var events = new List<IEventEntity>();
                events.AddRange(fModel.QueryNotes(fAquarium.Id));
                events.AddRange(fModel.QueryMaintenances(fAquarium.Id));
                events.AddRange(fModel.QueryMeasures(fAquarium.Id));
                events.AddRange(fModel.QueryTransfers(fAquarium.Id));
                events.Sort((x, y) => { return x.Timestamp.CompareTo(y.Timestamp); });

                DateTime dtPrev = ALCore.ZeroDate;
                double prevVolume = 0.0d, curVolume = 0.0d;
                string prevTime = string.Empty, curTime;
                foreach (IEventEntity evnt in events) {
                    curTime = ALCore.GetTimeStr(evnt.Timestamp);
                    if (!prevTime.Equals(curTime)) {
                        prevTime = curTime;
                    }

                    if (evnt is Maintenance) {
                        Maintenance mnt = (Maintenance)evnt;

                        double changeValue = mnt.Value;
                        switch (mnt.Type) {
                            case MaintenanceType.Restart:
                            case MaintenanceType.AquariumStarted:
                            case MaintenanceType.AquariumStopped:
                                prevVolume = curVolume;
                                curVolume = changeValue;
                                break;

                            default:
                                int factor = ALData.MaintenanceTypes[(int)mnt.Type].WaterChangeFactor;
                                if (factor != 0) {
                                    prevVolume = curVolume;
                                }
                                curVolume += (changeValue * factor);
                                break;
                        }

                        double chngPercent = (changeValue / curVolume) * 100.0d;

                        int days = -1;
                        if (mnt.Type >= MaintenanceType.Restart && mnt.Type <= MaintenanceType.WaterReplaced) {
                            if (!ALCore.IsZeroDate(dtPrev)) {
                                days = (mnt.Timestamp.Date - dtPrev).Days;
                            }
                            dtPrev = mnt.Timestamp.Date;
                        }

                        string strType = Localizer.LS(ALData.MaintenanceTypes[(int)mnt.Type].Name);
                        string strDays = (days >= 0) ? days.ToString() : string.Empty;

                        var item = ListView.AddItemEx(mnt,
                                       curTime,
                                       strType,
                                       ALCore.GetDecimalStr(mnt.Value),
                                       mnt.Note,
                                       ALCore.GetDecimalStr(curVolume),
                                       ALCore.GetDecimalStr(chngPercent),
                                       strDays
                                   );
                    }

                    if (evnt is Measure) {
                        Measure msr = (Measure)evnt;

                        var item = ListView.AddItemEx(msr,
                                       curTime,
                                       Localizer.LS(LSID.Measure),
                                       string.Empty,
                                       string.Empty,
                                       string.Empty,
                                       string.Empty,
                                       string.Empty,

                                       ALCore.GetDecimalStr(msr.Temperature, 2, true),
                                       ALCore.GetDecimalStr(msr.NO3, 2, true),
                                       ALCore.GetDecimalStr(msr.GH, 2, true),
                                       ALCore.GetDecimalStr(msr.KH, 2, true),
                                       ALCore.GetDecimalStr(msr.pH, 2, true),
                                       ALCore.GetDecimalStr(msr.CO2, 2, true),
                                       ALCore.GetDecimalStr(msr.NH, 2, true),
                                       ALCore.GetDecimalStr(msr.PO4, 2, true)
                                   );
                    }

                    if (evnt is Note) {
                        Note note = (Note)evnt;

                        var item = ListView.AddItemEx(note,
                                       curTime,
                                       Localizer.LS(LSID.Event),
                                       string.Empty,
                                       note.Event,
                                       string.Empty,
                                       string.Empty,
                                       string.Empty
                                   );
                    }

                    if (evnt is Transfer) {
                        Transfer transfer = (Transfer)evnt;
                        if (ALCore.IsInhabitant(transfer.ItemType)) {
                            string strType = Localizer.LS(ALData.TransferTypes[(int)transfer.Type]);
                            var itemRec = fModel.GetRecord(transfer.ItemType, transfer.ItemId);
                            string itName = (itemRec == null) ? string.Empty : itemRec.ToString();

                            var item = ListView.AddItemEx(transfer,
                                           curTime, strType, transfer.Quantity.ToString(), itName,
                                           string.Empty, string.Empty, string.Empty
                                       );
                        }
                    }
                }
            }
        }

        private void ExportHandler(object sender, EventArgs e)
        {
            Export();
        }
    }
}
