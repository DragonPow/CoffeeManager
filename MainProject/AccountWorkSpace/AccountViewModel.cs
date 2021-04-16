using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MainProject.AccountWorkSpace
{
    class AccountViewModel : BaseViewModel, IMainWorkSpace
    {
        #region Fields
        private const PackIconKind _iconDisplay = PackIconKind.CardAccountDetailsOutline;
        #endregion //Fields


        #region Properties
        public string NameWorkSpace => "Tài khoản";
        public PackIcon IconDisplay
        {
            get
            {
                var p = new PackIcon() { Kind = _iconDisplay, Width=30, Height=30 };
                return p;
            }
        }
        #endregion //Properties
    }
}
