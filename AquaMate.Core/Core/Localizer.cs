/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using AquaMate.Logging;
using BSLib;

namespace AquaMate.Core
{
    /// <summary>
    /// Localizable string id
    /// </summary>
    public enum LSID
    {
        /* 000 */ None,
        /* 001 */ First,

        /* 001 */ File = 1,
        /* 002 */ Edit = 2,
        /* 003 */ Help,
        /* 004 */ Exit,
        /* 005 */ About,
        /* 006 */ Settings,
        /* 007 */ CleanSpace,
        /* 008 */ RecordDeleteQuery,
        /* 009 */ Aquariums,
        /* 010 */ Inhabitants,
        /* 011 */ Species,
        /* 012 */ Devices,
        /* 013 */ Nutrition,
        /* 014 */ Maintenance,
        /* 015 */ Notes,
        /* 016 */ History,
        /* 017 */ Measures,
        /* 018 */ Schedule,
        /* 019 */ Transfers,
        /* 020 */ Budget,
        /* 021 */ Accept,
        /* 022 */ Cancel,
        /* 023 */ Add,
        /* 024 */ Delete,
        /* 025 */ Language,
        /* 026 */ HideClosedTanks,
        /* 027 */ ExitOnClose,
        /* 028 */ Autorun,
        /* 029 */ Name,
        /* 030 */ Description,
        /* 031 */ Shape,
        /* 032 */ StartDate, // deprecated
        /* 033 */ StopDate, // deprecated
        /* 034 */ WaterType,
        /* 035 */ Width,
        /* 036 */ Depth,
        /* 037 */ Height,
        /* 038 */ TankVolume,
        /* 039 */ WaterVolume,
        /* 040 */ GlassThickness,
        /* 041 */ Note,
        /* 042 */ Sex,
        /* 043 */ Aquarium,
        /* 044 */ Inhabitant,
        /* 045 */ SpeciesS,
        /* 046 */ Device,
        /* 047 */ Event,
        /* 048 */ Measure,
        /* 049 */ ScheduleS,
        /* 050 */ Transfer,
        /* 051 */ Date,
        /* 052 */ Type,
        /* 053 */ Value,
        /* 054 */ Reminder,
        /* 055 */ Status,
        /* 056 */ SourceTank,
        /* 057 */ TargetTank,
        /* 058 */ Cause,
        /* 059 */ Quantity,
        /* 060 */ Shop,
        /* 061 */ UnitPrice,
        /* 062 */ Item,
        /* 063 */ Text,
        /* 064 */ Brand,
        /* 065 */ Amount,
        /* 066 */ Inventory,
        /* 067 */ Additive,
        /* 068 */ Chemistry,
        /* 069 */ Equipment,
        /* 070 */ Furniture,
        /* 071 */ Decoration,
        /* 072 */ Timestamp,
        /* 073 */ Enabled,
        /* 074 */ Digital,
        /* 075 */ Power,
        /* 076 */ WorkTime,
        /* 077 */ TSDBPoint,
        /* 078 */ Data,
        /* 079 */ Trend,
        /* 080 */ DataMonitor,
        /* 081 */ Chart,
        /* 082 */ Relocation,
        /* 083 */ Purchase,
        /* 084 */ Sale,
        /* 085 */ Birth,
        /* 086 */ Death,
        /* 087 */ Light,
        /* 088 */ Pump,
        /* 089 */ Thermometer,
        /* 090 */ Filter,
        /* 091 */ Heater,
        /* 092 */ Sum,
        /* 093 */ Restart,
        /* 094 */ WaterAdded,
        /* 095 */ WaterReplaced,
        /* 096 */ WaterRemoved,
        /* 097 */ Clean,
        /* 098 */ Other,
        /* 099 */ InclusionDate,
        /* 100 */ Fish,
        /* 101 */ Invertebrate,
        /* 102 */ Plant,
        /* 103 */ Coral,
        /* 104 */ Compatibility,
        /* 105 */ Unit,
        /* 106 */ WaterChanges,
        /* 107 */ Common,
        /* 108 */ Tank,
        /* 109 */ NavPrev,
        /* 110 */ NavNext,
        /* 111 */ Min,
        /* 112 */ Max,
        /* 113 */ Deviation,
        /* 114 */ Female,
        /* 115 */ Male,
        /* 116 */ Hermaphrodite,
        /* 117 */ FreshWater,
        /* 118 */ BrackishWater,
        /* 119 */ SeaWater,
        /* 120 */ Bowl,
        /* 121 */ Cube,
        /* 122 */ Rectangular,
        /* 123 */ BowFront,
        /* 124 */ PlateFrontCorner,
        /* 125 */ BowFrontCorner,
        /* 126 */ Single,
        /* 127 */ Daily,
        /* 128 */ Weekly,
        /* 129 */ Monthly,
        /* 130 */ Yearly,
        /* 131 */ ToDo,
        /* 132 */ Canceled,
        /* 133 */ Snoozed,
        /* 134 */ Late,
        /* 135 */ Closed,
        /* 136 */ Calculator,
        /* 137 */ Calculate,
        /* 138 */ AdultSize,
        /* 139 */ LifeSpan,
        /* 140 */ BalanceFooter,
        /* 141 */ Alive,
        /* 142 */ Dead,
        /* 143 */ Sick,
        /* 144 */ InUse,
        /* 145 */ Stopped,
        /* 146 */ Finished,
        /* 147 */ Broken,
        /* 148 */ State,
        /* 149 */ HideAtStartup,
        /* 150 */ SL_Any,
        /* 151 */ SL_Top,
        /* 152 */ SL_Top_and_Mid,
        /* 153 */ SL_Mid,
        /* 154 */ SL_Mid_and_Bottom,
        /* 155 */ SL_Bottom,
        /* 156 */ SwimLevel,
        /* 157 */ PowerFooter,
        /* 158 */ AquaWorked,
        /* 159 */ AquaWorks,
        /* 160 */ Export,
        /* 161 */ LogBook,
        /* 162 */ ExclusionDate,
        /* 163 */ DaysStr,
        /* 164 */ MonthsStr,
        /* 165 */ YearsStr,
        /* 166 */ ChartItemTypes,
        /* 167 */ ChartShops,
        /* 168 */ ChartBrands,
        /* 169 */ EdgeSize,
        /* 170 */ CentreWidth,
        /* 171 */ Snapshot,
        /* 172 */ Snapshots,
        /* 173 */ ScientificName,
        /* 174 */ BioFamily,
        /* 175 */ ChartFamilies,
        /* 176 */ M3DViewer,
        /* 177 */ Length,
        /* 178 */ Volume,
        /* 179 */ Centimeter,
        /* 180 */ Inch,
        /* 181 */ Litre,
        /* 182 */ UKGallon,
        /* 183 */ USGallon,
        /* 184 */ BottomDiameter,
        /* 185 */ TopDiameter,
        /* 186 */ Cylinder,
        /* 187 */ UnderfillHeight,
        /* 188 */ SoilHeight,
        /* 189 */ SoilVolume,
        /* 190 */ SoilMass,
        /* 191 */ Mass,
        /* 192 */ Kilogram,
        /* 193 */ Pound,
        /* 194 */ Country,
        /* 195 */ Brands,
        /* 196 */ ChartCountries,
        /* 197 */ ChartMonthes,
        /* 198 */ DegreeCelsius,
        /* 199 */ DegreeFahrenheit,
        /* 200 */ DegreeKelvin,
        /* 201 */ Temperature,
        /* 202 */ Analysis,
        /* 203 */ Quality,
        /* 204 */ Exclusion,
        /* 205 */ Sold,
        /* 206 */ DataAcquisition,
        /* 207 */ Parameters,
        /* 208 */ Channel,
        /* 209 */ BioTreemap, // not used, reserved
        /* 210 */ LifeExpectancy,
        /* 211 */ Fertilize,
        /* 212 */ Cure,
        /* 213 */ RecordHasExternalLinks,
        /* 214 */ Soil,
        /* 215 */ Density,
        /* 216 */ Size,
        /* 217 */ Weight,
        /* 218 */ Load,
        /* 219 */ Save,
        /* 220 */ Radar, // not used, reserved
        /* 221 */ Shops,
        /* 222 */ Address,
        /* 223 */ Telephone,
        /* 224 */ WebSite,
        /* 225 */ Email,
        /* 226 */ ViewSite,
        /* 227 */ HideLosses,
        /* 228 */ Pricelist,
        /* 229 */ AquariumStarted,
        /* 230 */ AquariumStopped,
        /* 231 */ Distribution,
        /* 232 */ Habitat,
        /* 233 */ Easy,
        /* 234 */ Moderate,
        /* 235 */ Hard,
        /* 236 */ Peaceful,
        /* 237 */ SemiAggressive,
        /* 238 */ Aggressive,
        /* 239 */ CareLevel,
        /* 240 */ Temperament,
        /* 241 */ Unknown,
        /* 242 */ UVSterilizer,
        /* 243 */ CO2Kit,
        /* 244 */ Flow,
        /* 245 */ LitrePerHour,
        /* 246 */ KilogramPerLitre,
        /* 247 */ LightTemperature,
        /* 248 */ LuminousFlux,
        /* 249 */ PhotosyntheticallyActiveRadiation,
        /* 250 */ Lumen,
        /* 251 */ WattPerSquareMeter,
        /* 252 */ AirPump,

        /* 301 */ MCPStart = 301,
        /* 302 */ MCPStop,
        /* 303 */ MCPServerHost,
        /* 304 */ MCPServerPort,
        /* 305 */ MCPTrustedHosts,
        /* 306 */ MCPCORS,
        /* 307 */ MCPVerboseServerLogs,
        /* 308 */ MCPAutoStart,
        /* 309 */ MCPServerStopped,
        /* 310 */ MCPServerStarted,
        /* 311 */ MCPError,
        /* 312 */ MCPMCPSrvConfig,
        /* 313 */ MCPValidPortRequired,
        /* 314 */ MCPValidationError,
        /* 315 */ MCPStartingServer,
        /* 316 */ MCPErrorStartingServer,
        /* 317 */ MCPStoppingServer,
        /* 318 */ MCPMCPServerSettings,
        /* 319 */ MCPHostToolTip,
        /* 320 */ MCPPortToolTip,
        /* 321 */ MCPAllowedHostsTip,
        /* 322 */ MCPStartingError,
        /* 323 */ MCPStoppingError,

        /* 000 */ Last = MCPStoppingError
    }


    public static class Localizer
    {
        public const int LS_DEF_CODE = 1033;
        public const string LS_DEF_SIGN = "enu";
        public const string LS_DEF_NAME = "English";

        private static readonly string[] LSDefList = new string[] {
            /* 001 */ "File",
            /* 002 */ "Edit",
            /* 003 */ "Help",
            /* 004 */ "Exit",
            /* 005 */ "About",
            /* 006 */ "Settings",
            /* 007 */ "Clean Space",
            /* 008 */ "Are you sure you want to remove record \"{0}\"?",
            /* 009 */ "Aquariums",
            /* 010 */ "Inhabitants",
            /* 011 */ "Species",
            /* 012 */ "Devices",
            /* 013 */ "Nutrition",
            /* 014 */ "Maintenance",
            /* 015 */ "Notes",
            /* 016 */ "History",
            /* 017 */ "Measures",
            /* 018 */ "Schedule",
            /* 019 */ "Transfers",
            /* 020 */ "Budget",
            /* 021 */ "Accept",
            /* 022 */ "Cancel",
            /* 023 */ "Add",
            /* 024 */ "Delete",
            /* 025 */ "Language",
            /* 026 */ "Hide closed tanks",
            /* 027 */ "Exit on Close",
            /* 028 */ "Autorun",
            /* 029 */ "Name",
            /* 030 */ "Description",
            /* 031 */ "Shape",
            /* 032 */ "Start date", // deprecated
            /* 033 */ "Stop date", // deprecated
            /* 034 */ "Water type",
            /* 035 */ "Width",
            /* 036 */ "Depth",
            /* 037 */ "Heigth",
            /* 038 */ "Tank volume",
            /* 039 */ "Water volume",
            /* 040 */ "Glass thickness",
            /* 041 */ "Note",
            /* 042 */ "Sex",
            /* 043 */ "Aquarium",
            /* 044 */ "Inhabitant",
            /* 045 */ "Species",
            /* 046 */ "Device",
            /* 047 */ "Event",
            /* 048 */ "Measure",
            /* 049 */ "Schedule",
            /* 050 */ "Transfer",
            /* 051 */ "Date",
            /* 052 */ "Type",
            /* 053 */ "Value",
            /* 054 */ "Reminder",
            /* 055 */ "Status",
            /* 056 */ "From aquarium",
            /* 057 */ "To Aquarium",
            /* 058 */ "Cause",
            /* 059 */ "Quantity",
            /* 060 */ "Shop",
            /* 061 */ "Unit price",
            /* 062 */ "Item",
            /* 063 */ "Text",
            /* 064 */ "Brand",
            /* 065 */ "Amount",
            /* 066 */ "Inventory",
            /* 067 */ "Additive",
            /* 068 */ "Chemistry",
            /* 069 */ "Equipment",
            /* 070 */ "Furniture",
            /* 071 */ "Decoration",
            /* 072 */ "Timestamp",
            /* 073 */ "Enabled",
            /* 074 */ "Digital",
            /* 075 */ "Power",
            /* 076 */ "Work time",
            /* 077 */ "TSDB Point",
            /* 078 */ "Data",
            /* 079 */ "Trend",
            /* 080 */ "Data Monitor",
            /* 081 */ "Chart",
            /* 082 */ "Relocation",
            /* 083 */ "Purchase",
            /* 084 */ "Sale",
            /* 085 */ "Birth",
            /* 086 */ "Death",
            /* 087 */ "Light",
            /* 088 */ "Pump",
            /* 089 */ "Thermometer",
            /* 090 */ "Filter",
            /* 091 */ "Heater",
            /* 092 */ "Sum",
            /* 093 */ "Restart",
            /* 094 */ "WaterAdded",
            /* 095 */ "WaterReplaced",
            /* 096 */ "WaterRemoved",
            /* 097 */ "Clean",
            /* 098 */ "Other",
            /* 099 */ "Inclusion date",
            /* 100 */ "Fish",
            /* 101 */ "Invertebrate",
            /* 102 */ "Plant",
            /* 103 */ "Coral",
            /* 104 */ "Compatibility",
            /* 105 */ "Unit",
            /* 106 */ "Water changes",
            /* 107 */ "Common",
            /* 108 */ "Tank",
            /* 109 */ "Prev",
            /* 110 */ "Next",
            /* 111 */ "Min",
            /* 112 */ "Max",
            /* 113 */ "Deviation",
            /* 114 */ "Female",
            /* 115 */ "Male",
            /* 116 */ "Hermaphrodite",
            /* 117 */ "Freshwater",
            /* 118 */ "Brackish water",
            /* 119 */ "Seawater",
            /* 120 */ "Bowl",
            /* 121 */ "Cube",
            /* 122 */ "Rectangular",
            /* 123 */ "Bowfront",
            /* 124 */ "Platefront corner",
            /* 125 */ "Bowfront corner",
            /* 126 */ "Single",
            /* 127 */ "Daily",
            /* 128 */ "Weekly",
            /* 129 */ "Monthly",
            /* 130 */ "Yearly",
            /* 131 */ "ToDo",
            /* 132 */ "Canceled",
            /* 133 */ "Snoozed",
            /* 134 */ "Late",
            /* 135 */ "Closed",
            /* 136 */ "Calculator",
            /* 137 */ "Calculate",
            /* 138 */ "Adult size",
            /* 139 */ "Life span",
            /* 140 */ "Expenses: {0:C2} ({1:C2}/d), Incomes: {2:C2}, Balance: {3:C2}",
            /* 141 */ "Alive",
            /* 142 */ "Dead",
            /* 143 */ "Sick",
            /* 144 */ "In Use",
            /* 145 */ "Stopped",
            /* 146 */ "Finished",
            /* 147 */ "Broken",
            /* 148 */ "State",
            /* 149 */ "Hide at startup",
            /* 150 */ "Any",
            /* 151 */ "Top",
            /* 152 */ "Top and Mid",
            /* 153 */ "Mid",
            /* 154 */ "Mid and Bottom",
            /* 155 */ "Bottom",
            /* 156 */ "Swim Level",
            /* 157 */ "Total power (kW/d): {0:0.00}, Electric cost: {1:C2}/d",
            /* 158 */ "Worked from {0} to {1} [{2} d]",
            /* 159 */ "Works from {0} [{1} d]",
            /* 160 */ "Export",
            /* 161 */ "Log book",
            /* 162 */ "Exclusion date",
            /* 163 */ "{0} ds",
            /* 164 */ "{0} ms",
            /* 165 */ "{0} ys",
            /* 166 */ "Chart (item types)",
            /* 167 */ "Chart (shops)",
            /* 168 */ "Chart (brands)",
            /* 169 */ "Edge size",
            /* 170 */ "Centre width",
            /* 171 */ "Snapshot",
            /* 172 */ "Snapshots",
            /* 173 */ "Scientific Name",
            /* 174 */ "Family",
            /* 175 */ "Chart (families)",
            /* 176 */ "3D Viewer",
            /* 177 */ "Length",
            /* 178 */ "Volume",
            /* 179 */ "Centimeter,cm",
            /* 180 */ "Inch,in",
            /* 181 */ "Litre,l",
            /* 182 */ "UK Gallon,gal",
            /* 183 */ "US Gallon,gal",
            /* 184 */ "Bottom diameter",
            /* 185 */ "Top diameter",
            /* 186 */ "Cylinder",
            /* 187 */ "Underfill height",
            /* 188 */ "Soil height",
            /* 189 */ "Soil volume",
            /* 190 */ "Soil mass",
            /* 191 */ "Mass",
            /* 192 */ "Kilogram",
            /* 193 */ "Pound",
            /* 194 */ "Country",
            /* 195 */ "Brands",
            /* 196 */ "Chart (countries)",
            /* 197 */ "Chart (monthes)",
            /* 198 */ "Degree Celsius,°C",
            /* 199 */ "Degree Fahrenheit,°F",
            /* 200 */ "Kelvin,K",
            /* 201 */ "Temperature",
            /* 202 */ "Analysis",
            /* 203 */ "Quality",
            /* 204 */ "Exclusion",
            /* 205 */ "Sold",
            /* 206 */ "Data acquisition",
            /* 207 */ "Parameters",
            /* 208 */ "Channel",
            /* 209 */ "BioTreemap",
            /* 210 */ "Life expectancy",
            /* 211 */ "Fertilize",
            /* 212 */ "Cure",
            /* 213 */ "The record has external links.",
            /* 214 */ "Soil",
            /* 215 */ "Density",
            /* 216 */ "Size",
            /* 217 */ "Weight",
            /* 218 */ "Load",
            /* 219 */ "Save",
            /* 220 */ "Radar",
            /* 221 */ "Shops",
            /* 222 */ "Address",
            /* 223 */ "Telephone",
            /* 224 */ "WebSite",
            /* 225 */ "Email",
            /* 226 */ "View site",
            /* 227 */ "Hide losses",
            /* 228 */ "Pricelist",
            /* 229 */ "AquariumStarted",
            /* 230 */ "AquariumStopped",
            /* 231 */ "Distribution",
            /* 232 */ "Habitat",
            /* 233 */ "Easy",
            /* 234 */ "Moderate",
            /* 235 */ "Hard",
            /* 236 */ "Peaceful",
            /* 237 */ "SemiAggressive",
            /* 238 */ "Aggressive",
            /* 239 */ "CareLevel",
            /* 240 */ "Temperament",
            /* 241 */ "Unknown",
            /* 242 */ "UV Sterilizer",
            /* 243 */ "CO2 Kit",
            /* 244 */ "Flow",
            /* 245 */ "Litre Per Hour,L/h",
            /* 246 */ "Kilogram Per Litre,kg/L",
            /* 247 */ "LightTemperature",
            /* 248 */ "LuminousFlux",
            /* 249 */ "PhotosyntheticallyActiveRadiation",
            /* 250 */ "Lumen,lm",
            /* 251 */ "Watt Per Square Meter,W/m2",
            /* 252 */ "AirPump",

            /* 301 */ "Start",
            /* 302 */ "Stop",
            /* 303 */ "Server Host",
            /* 304 */ "Server Port",
            /* 305 */ "Trusted Hosts",
            /* 306 */ "Cross-Origin Resource Sharing (CORS)",
            /* 307 */ "Verbose Server Logs",
            /* 308 */ "Autostart on application launch",
            /* 309 */ "Server stopped",
            /* 310 */ "Server started: {0}",
            /* 311 */ "Error",
            /* 312 */ "MCP server configuration",
            /* 313 */ "Specify a valid port number (1-65535).",
            /* 314 */ "Validation Error",
            /* 315 */ "Starting server...",
            /* 316 */ "Error starting server",
            /* 317 */ "Stopping server",
            /* 318 */ "MCP Server Settings & Control",
            /* 319 */ "IP or host to listen on (e.g., localhost or 0.0.0.0)",
            /* 320 */ "Port for connections",
            /* 321 */ "Comma-separated list of allowed origins",
            /* 322 */ "Error starting server: {0}",
            /* 323 */ "Error stopping server: {0}",
        };


        private static readonly ILogger fLogger;
        private static readonly List<LocaleFile> fLocales;
        private static readonly Dictionary<int, string> fList;

        public static IList<LocaleFile> Locales
        {
            get { return fLocales; }
        }


        static Localizer()
        {
            fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "Localizer");
            fLocales = new List<LocaleFile>();
            fList = new Dictionary<int, string>();
        }

        public static string LS(LSID lsid)
        {
            int idx = ((IConvertible)lsid).ToInt32(null);
            string res;
            return (fList.TryGetValue(idx, out res)) ? res : "?";
        }

        public static void DefInit()
        {
            DefInit(LSDefList);
        }

        private static void DefInit(string[] source)
        {
            fList.Clear();
            for (LSID id = LSID.First; id <= LSID.Last; id++) {
                int idx = (int)id;
                fList.Add(idx, source[idx - 1]);
            }
        }

        public static bool LoadFromFile(string fileName)
        {
            bool result = false;

            if (File.Exists(fileName)) {
                fList.Clear();

                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8)) {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;

                    int lsId = -1;
                    using (XmlReader xr = XmlReader.Create(reader, settings)) {
                        while (xr.Read()) {
                            if (xr.NodeType == XmlNodeType.Element && xr.Name == "string") {
                                if (!int.TryParse(xr.GetAttribute("id"), out lsId)) {
                                    lsId = -1;
                                }
                            } else if (xr.NodeType == XmlNodeType.Text && lsId != -1) {
                                string lsText = xr.Value;
                                fList.Add(lsId, lsText);
                            }
                        }
                    }
                    result = true;
                }
            }

            return result;
        }

        private static void PrepareLocaleFile(string fileName)
        {
            try {
                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8)) {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;

                    int code = -1;
                    string name;
                    using (XmlReader xr = XmlReader.Create(reader, settings)) {
                        while (xr.Read()) {
                            if (xr.NodeType == XmlNodeType.Element && xr.Name == "resources") {
                                if (!int.TryParse(xr.GetAttribute("code"), out code)) {
                                    code = -1;
                                }
                                name = xr.GetAttribute("name");

                                fLocales.Add(new LocaleFile(code, "", name, fileName));
                                break;
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("PrepareLocaleFile(" + fileName + ")", ex);
            }
        }

        public static void FindLocales()
        {
            try {
                string path = AppHost.GetLocalesPath();
                if (Directory.Exists(path)) {
                    string[] localeFiles = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < localeFiles.Length; i++) PrepareLocaleFile(localeFiles[i]);
                }
            } catch (Exception ex) {
                fLogger.WriteError("FindLocales()", ex);
            }
        }

        public static void LoadLocale(int localeCode)
        {
            if (localeCode != Localizer.LS_DEF_CODE) {
                bool loaded = false;

                foreach (LocaleFile localeFile in fLocales) {
                    if (localeFile.Code == localeCode) {
                        loaded = Localizer.LoadFromFile(localeFile.FileName);
                        break;
                    }
                }

                if (!loaded) localeCode = Localizer.LS_DEF_CODE;
            }

            if (localeCode == Localizer.LS_DEF_CODE) {
                DefInit();
            }
        }

        public static LocaleFile GetLocaleByCode(int code)
        {
            foreach (LocaleFile localeFile in fLocales) {
                if (localeFile.Code == code) {
                    return localeFile;
                }
            }
            return null;
        }
    }
}
