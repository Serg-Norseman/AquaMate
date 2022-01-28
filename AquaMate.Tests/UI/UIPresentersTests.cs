/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.TSDB;
using NSubstitute;
using NUnit.Framework;

namespace AquaMate.UI
{
    [TestFixture]
    public class UIPresentersTests
    {
        [SetUp]
        public void Setup()
        {
            AppHost.InitInstance<WFAppHost>();
        }

        [Test]
        public void Test_CalculatorPresenter_Common()
        {
            var view = Substitute.For<ICalculatorView>();

            var presenter = new CalculatorPresenter(view);
            presenter.UpdateView();
        }

        [Test]
        public void Test_DataMonitorPresenter_Common()
        {
            var view = Substitute.For<IDataMonitorView>();
            var model = new ALModel(null);

            var presenter = new DataMonitorPresenter(view);
            presenter.SetContext(model);
            presenter.UpdateView();
        }

        [Test]
        public void Test_AquariumEditorPresenter_Common()
        {
            var view = Substitute.For<IAquariumEditorView>();
            var model = new ALModel(null);
            var record = new Aquarium();

            var presenter = new AquariumEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_BrandEditorPresenter_Common()
        {
            var view = Substitute.For<IBrandEditorView>();
            var model = new ALModel(null);
            var record = new Brand();

            var presenter = new BrandEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_DeviceEditorPresenter_Common()
        {
            var view = Substitute.For<IDeviceEditorView>();
            var model = new ALModel(null);
            var record = new Device();

            var presenter = new DeviceEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_InhabitantEditorPresenter_Common()
        {
            var view = Substitute.For<IInhabitantEditorView>();
            var model = new ALModel(null);
            var record = new Inhabitant();

            var presenter = new InhabitantEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_InventoryEditorPresenter_Common()
        {
            var view = Substitute.For<IInventoryEditorView>();
            var model = new ALModel(null);
            var record = new Inventory();

            var presenter = new InventoryEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_MaintenanceEditorPresenter_Common()
        {
            var view = Substitute.For<IMaintenanceEditorView>();
            var model = new ALModel(null);
            var record = new Maintenance();

            var presenter = new MaintenanceEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_MeasureEditorPresenter_Common()
        {
            var view = Substitute.For<IMeasureEditorView>();
            var model = new ALModel(null);
            var record = new Measure();

            var presenter = new MeasureEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_NoteEditorPresenter_Common()
        {
            var view = Substitute.For<INoteEditorView>();
            var model = new ALModel(null);
            var record = new Note();

            var presenter = new NoteEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_NutritionEditorPresenter_Common()
        {
            var view = Substitute.For<INutritionEditorView>();
            var model = new ALModel(null);
            var record = new Nutrition();

            var presenter = new NutritionEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_ScheduleEditorPresenter_Common()
        {
            var view = Substitute.For<IScheduleEditorView>();
            var model = new ALModel(null);
            var record = new Schedule();

            var presenter = new ScheduleEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_SnapshotEditorPresenter_Common()
        {
            var view = Substitute.For<ISnapshotEditorView>();
            var model = new ALModel(null);
            var record = new Snapshot();

            var presenter = new SnapshotEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_SpeciesEditorPresenter_Common()
        {
            var view = Substitute.For<ISpeciesEditorView>();
            var model = new ALModel(null);
            var record = new Species();

            var presenter = new SpeciesEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_TransferEditorPresenter_Common()
        {
            var view = Substitute.For<ITransferEditorView>();
            var model = new ALModel(null);
            var record = new Transfer();

            var presenter = new TransferEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_TSPointEditorPresenter_Common()
        {
            var view = Substitute.For<ITSPointEditorView>();
            var model = new ALModel(null);
            var record = new TSPoint();

            var presenter = new TSPointEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }

        [Test]
        public void Test_TSValueEditorPresenter_Common()
        {
            var view = Substitute.For<ITSValueEditorView>();
            var model = new ALModel(null);
            var record = new TSValue();

            var presenter = new TSValueEditorPresenter(view);
            presenter.SetContext(model, record);
            Assert.IsTrue(presenter.ApplyChanges());
        }
    }
}
