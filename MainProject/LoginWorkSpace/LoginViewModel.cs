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
        private ICommand _loginCommand;
        #endregion //Fiedls


        #region Properties
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayingCommand<Object>(a => Login()); ;
                }
                return _loginCommand;
            }
        }
        public EMPLOYEE CurrentAccount
        {
            get
            {
                if(_currentAccount==null)
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
        private void Login()
        {
            using (var context = new mainEntities())
            {
                
            }
        }
        #endregion //Helper methods
    }
}
