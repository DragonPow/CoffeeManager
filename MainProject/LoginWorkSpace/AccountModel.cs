using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.LoginWorkSpace
{
    public class AccountModel : BaseViewModel
    {
        private EMPLOYEE _user;

        public EMPLOYEE User
        {
            get => _user;
            set => _user = value;
        }

        public bool isLoginSuccessfull()
        {
            mainEntities db = new mainEntities();
            return db.EMPLOYEEs.Contains(User);
        }
    }
}
