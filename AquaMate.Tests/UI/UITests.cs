/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

#if !__MonoCS__

using System;
using System.Windows.Forms;
using AquaMate.Core;
using NUnit.Framework;

namespace AquaMate.UI
{
    [TestFixture]
    public class UITests : CustomFormTest
    {
        private Form fMainWin;

        public override void Setup()
        {
            base.Setup();
            AppHost.TEST_MODE = true;
        }

        public void AboutDlg_Handler()
        {
            ClickButton("btnClose", "AboutDlg");
        }

        [Test]
        public void Test_Common()
        {
            AppHost.InitInstance<WFAppHost>();
            WFAppHost.RegisterControlHandlers();
            WFAppHost.RegisterViews();

            fMainWin = new MainForm();

            ExpectModal("AboutDlg", "AboutDlg_Handler");
            ClickToolStripMenuItem("miAbout", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickToolStripMenuItem("miSettings", fMainWin);

            ModalFormHandler = Dialog_Accept_Handler;
            ClickToolStripMenuItem("miSettings", fMainWin);

            ClickToolStripMenuItem("miCleanSpace", fMainWin);

            ModalFormHandler = Calculator_Handler;
            ClickToolStripMenuItem("miCalculator", fMainWin);

            ClickToolStripButton("btnTanks", fMainWin);
            ClickToolStripButton("btnInhabitants", fMainWin);
            ClickToolStripButton("btnSpecies", fMainWin);
            ClickToolStripButton("btnNutrition", fMainWin);
            ClickToolStripButton("btnDevices", fMainWin);
            ClickToolStripButton("btnInventory", fMainWin);
            ClickToolStripButton("btnMaintenance", fMainWin);
            ClickToolStripButton("btnNotes", fMainWin);
            ClickToolStripButton("btnMeasures", fMainWin);
            ClickToolStripButton("btnSchedule", fMainWin);
            ClickToolStripButton("btnTransfers", fMainWin);
            ClickToolStripButton("btnBudget", fMainWin);
            ClickToolStripButton("btnSnapshots", fMainWin);
            ClickToolStripButton("btnTSDB", fMainWin);

            ClickToolStripButton("btnPrev", fMainWin);
            ClickToolStripButton("btnNext", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Calculator_Handler(string name, IntPtr ptr, Form form)
        {
            KeyDownForm("CalculatorDlg", Keys.Escape);
        }

        [Test]
        public void Test_TanksPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnTanks", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ModalFormHandler = Tanks_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Tanks_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample aquarium");

            SelectCombo("cmbShape", form, 2);

            SelectCombo("cmbShape", form, 3);
            //EnterText("txtDepth", form, "10");
            //EnterText("txtWidth", form, "10");
            //EnterText("txtHeigth", form, "10");
            //EnterText("txtGlassThickness", form, "0.5");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_InhabitantsPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnInhabitants", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            // LifeLines Chart
            ClickButton("btnChart", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_SpeciesPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnSpecies", fMainWin);

            //ModalFormHandler = SaveFile_Cancel_Handler;
            //ClickButton("btnExport", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ModalFormHandler = Species_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnEdit", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnDelete", fMainWin);

            ModalFormHandler = null;
            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Species_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample species");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_NutritionPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnNutrition", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ModalFormHandler = Nutrition_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = Nutrition_Accept_Handler;
            ClickButton("btnEdit", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnDelete", fMainWin);

            ModalFormHandler = null;
            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Nutrition_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample nutrition");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_DevicesPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnDevices", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ModalFormHandler = Device_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = Device_Accept_Handler;
            ClickButton("btnEdit", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnDelete", fMainWin);

            ModalFormHandler = null;
            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Device_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample device");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_InventoryPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnInventory", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ModalFormHandler = Inventory_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = Inventory_Accept_Handler;
            ClickButton("btnEdit", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnDelete", fMainWin);

            ModalFormHandler = null;
            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Inventory_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample inventory");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_MaintenancePanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnMaintenance", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_NotesPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnNotes", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_MeasuresPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnMeasures", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickButton("btnChart", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_SchedulePanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnSchedule", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_TransfersPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnTransfers", fMainWin);

            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnAdd", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnEdit", fMainWin);

            // no selected item
            //ModalFormHandler = Dialog_Cancel_Handler;
            //ClickButton("btnDelete", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_BudgetPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnBudget", fMainWin);

            ClickButton("btnChartTypes", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickButton("btnChartShops", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickButton("btnChartBrands", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickButton("btnChartCountries", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickButton("btnChartMonthes", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickButton("btnBrands", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_BrandPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnBudget", fMainWin);
            ClickButton("btnBrands", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_SnapshotPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnSnapshots", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_TSDBPanel()
        {
            WFAppHost.RegisterControlHandlers();

            fMainWin = new MainForm();

            ClickToolStripButton("btnTSDB", fMainWin);

            ModalFormHandler = DataMonitor_Handler;
            ClickButton("btnDataMonitor", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickButton("btnAdd", fMainWin);

            /*ModalFormHandler = TSPoint_Accept_Handler;
            ClickButton("btnAdd", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = TSPoint_Accept_Handler;
            ClickButton("btnEdit", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ClickButton("btnData", fMainWin);
            ClickToolStripButton("btnPrev", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnDelete", fMainWin);

            SelectListView("ListView", fMainWin, 0);
            ClickButton("btnTrend", fMainWin);*/

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void TSPoint_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample point");

            ClickButton("btnAccept", form);
        }

        public static void DataMonitor_Handler(string name, IntPtr ptr, Form form)
        {
            KeyDownForm("DataMonitor", Keys.Escape);
        }
    }
}

#endif
