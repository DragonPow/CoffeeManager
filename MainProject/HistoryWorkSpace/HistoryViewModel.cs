using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.HistoryWorkSpace
{
    class HistoryViewModel : BaseViewModel, IMainWorkSpace
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
