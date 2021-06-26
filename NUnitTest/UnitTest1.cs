using NUnit.Framework;
using MainProject.ViewModel;
using MainProject.Model;
using System.Collections.ObjectModel;

namespace NUnitTest
{
    [TestFixture]
    public class TableTest
    {
        [TestCase]
        public void AddTale()
        {
            TableViewModel tableVM = new TableViewModel();
            tableVM.ListTable.Clear();
           
            
        }

    }
}