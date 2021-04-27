using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject
{
    class DetailPro : BaseViewModel
    {
        private int _quantity;
        private PRODUCT pro;

        private ICommand _plusQuantity;
        private ICommand _minusQuantity;


        public DetailPro(PRODUCT pro, int quantity)
        {
            this.Pro = pro;
            this.Quantity = quantity;
        }

        public ICommand PlusQuantity
        {
            get
            {
                if (PlusQuantity == null)
                    _plusQuantity = new RelayingCommand<object>(a => Plus());
                return _plusQuantity;
            }

        }

        public ICommand MinusQuantity
        {
            get
            {
                if (_minusQuantity == null)
                    _minusQuantity = new RelayingCommand<object>(a => Minus());
                return _minusQuantity;
            }

        }

        public int Quantity { get => _quantity; set => _quantity = value; }
        public PRODUCT Pro { get => pro; set => pro = value; }

        public void Plus()
        {
            ++Quantity;
        }

        public void Minus()
        {
            --Quantity;
        }
    }
}