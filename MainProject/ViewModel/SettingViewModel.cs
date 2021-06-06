using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ViewModel
{
    class SettingViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Lịch sử";
        private const PackIconKind _iconDisplay = PackIconKind.ClipboardTextSearchOutline;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }
    }
}
