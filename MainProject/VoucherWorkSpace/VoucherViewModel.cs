using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.VoucherWorkSpace
{
    class VoucherViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Voucher";
        private const PackIconKind _IconDisplay = PackIconKind.GiftOutline;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _IconDisplay, Width = 30, Height = 30 };
            }
        }
    }
}
