using MainProject.MainWorkSpace;
using MainProject.StatisticWorkSpace;
using MainProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProject.ApplicationWorkSpace
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Fields
        private List<IMainWorkSpace> _workSpaces;
        private IMainWorkSpace _currentWorkSpace;
        #endregion //Fields



        #region Constructors
        public ApplicationViewModel()
        {
            //LoginView LoginView = new LoginView();
            //LoginView.DataContext = new LoginViewModel();
            //LoginView.ShowDialog();
            TableViewModel table = new TableViewModel();
            ProductViewModel prod = new ProductViewModel();
            MainViewModel main = new MainViewModel(table, prod);
            //Add list MainWorkSpace here
            WorkSpaces.Add(main);
            //WorkSpaces.Add(new AccountViewModel());
            WorkSpaces.Add(new ManageProductviewModel(main));
            WorkSpaces.Add(table);
            WorkSpaces.Add(new HistoryViewModel());
            WorkSpaces.Add(new StatisticViewModel());
            WorkSpaces.Add(new SettingViewModel());
            //WorkSpaces.Add(new VoucherViewModel());

            //Define current workspace
            CurrentWorkSpace = WorkSpaces[0];
        }
        #endregion //Constructors



        #region Properties
        public List<IMainWorkSpace> WorkSpaces
        {
            get
            {
                if (_workSpaces == null)
                {
                    _workSpaces = new List<IMainWorkSpace>();
                }
                return _workSpaces;
            }
        }

        public IMainWorkSpace CurrentWorkSpace
        {
            get => _currentWorkSpace;
            set
            {
                if (value!=_currentWorkSpace)
                {
                    _currentWorkSpace = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion //Propeties
    }
}
