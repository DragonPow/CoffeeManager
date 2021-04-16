using MainProject.AccountWorkSpace;
using MainProject.HistoryWorkSpace;
using MainProject.LoginWorkSpace;
using MainProject.MainWorkSpace;
using MainProject.StatisticWorkSpace;
using MainProject.VoucherWorkSpace;
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
            //Show LoginView before
            LoginWinDow LoginView = new LoginWinDow();
            LoginView.ShowDialog();

            //Add list MainWorkSpace here
            WorkSpaces.Add(new MainViewModel());
            WorkSpaces.Add(new AccountViewModel());
            WorkSpaces.Add(new HistoryViewModel());
            WorkSpaces.Add(new StatisticViewModel());
            WorkSpaces.Add(new VoucherViewModel());

            //Define current workspace
            //CurrentWorkSpace = WorkSpaces[0];
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
