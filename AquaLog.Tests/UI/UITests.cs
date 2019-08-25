﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaLog.Core;
using NUnit.Framework;

namespace AquaLog.UI
{
    [TestFixture]
    public class UITests : CustomFormTest
    {
        private Form fMainWin;

        public override void Setup()
        {
            base.Setup();
            ALCore.TEST_MODE = true;
        }

        public void AboutDlg_Handler()
        {
            ClickButton("btnClose", "AboutDlg");
        }

        [Test]
        public void Test_Common()
        {
            fMainWin = new MainForm();

            ExpectModal("AboutDlg", "AboutDlg_Handler");
            ClickToolStripMenuItem("miAbout", fMainWin);

            ModalFormHandler = Dialog_Cancel_Handler;
            ClickToolStripMenuItem("miSettings", fMainWin);

            ClickToolStripMenuItem("miCleanSpace", fMainWin);

            ClickToolStripButton("btnTanks", fMainWin);
            ClickToolStripButton("btnInhabitants", fMainWin);
            ClickToolStripButton("btnSpecies", fMainWin);
            ClickToolStripButton("btnNutrition", fMainWin);
            ClickToolStripButton("btnDevices", fMainWin);
            ClickToolStripButton("btnMaintenance", fMainWin);
            ClickToolStripButton("btnNotes", fMainWin);
            ClickToolStripButton("btnHistory", fMainWin);
            ClickToolStripButton("btnMeasures", fMainWin);
            ClickToolStripButton("btnSchedule", fMainWin);
            ClickToolStripButton("btnTransfers", fMainWin);
            ClickToolStripButton("btnBudget", fMainWin);

            ClickToolStripButton("btnPrev", fMainWin);
            ClickToolStripButton("btnNext", fMainWin);

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_TanksPanel()
        {
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
            EnterText("txtDepth", form, "10");
            EnterText("txtWidth", form, "10");
            EnterText("txtHeigth", form, "10");
            EnterText("txtGlassThickness", form, "0.5");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_InhabitantsPanel()
        {
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

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_SpeciesPanel()
        {
            fMainWin = new MainForm();

            ClickToolStripButton("btnSpecies", fMainWin);

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

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        public static void Device_Accept_Handler(string name, IntPtr ptr, Form form)
        {
            EnterText("txtName", form, "sample device");

            ClickButton("btnAccept", form);
        }

        [Test]
        public void Test_MaintenancePanel()
        {
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
        public void Test_HistoryPanel()
        {
            fMainWin = new MainForm();

            ClickToolStripButton("btnHistory", fMainWin);

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

            ClickToolStripMenuItem("miExit", fMainWin);
        }

        [Test]
        public void Test_SchedulePanel()
        {
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
            fMainWin = new MainForm();

            ClickToolStripButton("btnTransfers", fMainWin);

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
    }
}