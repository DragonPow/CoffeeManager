using MainProject.ApplicationWorkSpace;
using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MainProject.LoginWorkSpace
{
    public class LoginViewModel : BaseViewModel
    {
        #region Fields
        private EMPLOYEE _currentAccount;
        private bool _loginSuccess;
        private ICommand _loginCommand;
        #endregion //Fiedls


        #region Properties
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayingCommand<LoginView>(view => Login(view)); ;
                }
                return _loginCommand;
            }
        }
        public bool isLoginSuccess
        {
            get => _loginSuccess;
            set
            {
                if (_loginSuccess!=value)
                {
                    _loginSuccess = value;
                    OnPropertyChanged();
                }
            }
        }
        public EMPLOYEE CurrentAccount
        {
            get
            {
                if(_currentAccount == null)
                {
                    _currentAccount = new EMPLOYEE();
                }
                return _currentAccount;
            }
            set
            {
                if (value!=_currentAccount)
                {
                    _currentAccount = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion //Properties


        #region Helper methods
        /// <summary>
        /// Check CurrentAccount is existing, if true close Login View
        /// </summary>
        private void Login(LoginView view)
        {
            //something missing here
            //if (!AccountModel.isContain(CurrentAccount))
            {
                isLoginSuccess = true;
                view.Close();
            }
        }
        #endregion //Helper methods
    }
}
