using MainProject.Model;
using MainProject.ViewModel;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace NUnitTest
{
    [TestFixture]
    public class TableTest
    {
        TableViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new TableViewModel();
        }

        [Test]
        public void addTable()
        {
            ObservableCollection<TABLECUSTOM> list = this.viewModel.ListTable;
            viewModel.Insert();
            Assert.AreEqual(list.Count + 1, this.viewModel.ListTable.Count);
        }
    }
}
