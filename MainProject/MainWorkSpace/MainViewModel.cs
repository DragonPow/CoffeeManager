using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MainProject.MainWorkSpace
{
    class MainViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Home";
        private const PackIconKind _iconDisplay = PackIconKind.Home;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width=30, Height=30 };
            }
        }
    }
}
