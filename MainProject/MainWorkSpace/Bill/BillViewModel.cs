using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject.MainWorkSpace.Bill
{
    public class BillViewModel : BaseViewModel
    {
        #region Fields
        private BILL _currentBill;
        private int _discount;
        private ICommand _paymentCommand;
        private ICommand _loadDiscountCommand;
        #endregion


        #region Properties
        public BILL CurrentBill
        {
            get
            {
                return _currentBill;
            }
            private set
            {
                if (value != _currentBill)
                {
                    _currentBill = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Discount
        {
            get { return _discount; }
            set
            {
                if (value != _discount)
                {
                    _discount = value;
                    OnPropertyChanged();
                }
            }
        }

        public long TotalPrice
        {
            get
            {
                return CurrentBill.DETAILBILLs.Sum(bill => bill.AMOUNT * bill.PRODUCT.PRICE);
            }
        }
        #endregion


        #region Commands
        /// <summary>
        /// Payment bill and quit the form
        /// </summary>
        public ICommand PaymentCommand
        {
            get
            {
                if (_paymentCommand == null)
                {
                    _paymentCommand = new RelayingCommand<BillView>(para => Payment(para));
                }
                return _paymentCommand;
            }
        }

        /// <summary>
        /// Auto load discount percent when eligible
        /// </summary>
        public ICommand LoadDiscountCommand
        {
            get
            {
                if (_loadDiscountCommand==null)
                {
                    _loadDiscountCommand = new RelayingCommand<Object>(para => LoadDiscount());
                }
                return _loadDiscountCommand;
            }
        }
        #endregion


        #region Constructors
        public BillViewModel()
        {
            CurrentBill = new BILL();
            CurrentBill.DETAILBILLs.Add(new DETAILBILL() { AMOUNT = 2, PRODUCT = new PRODUCT() { NAME = "Thach", PRICE = 1000 } });
        }

        public BillViewModel(BILL bill)
        {
            CurrentBill = bill;
        }
        #endregion

        private void Payment(BillView view)
        {
            using (var db = new mainEntities())
            {
                //var newBill = new BILL();
                //db.BILLs.Add(CurrentBill);
                //db.SaveChanges();

                //testing
                CurrentBill.DETAILBILLs[0].AMOUNT = 3;
            }
            //view.Close();
        }
        private void LoadDiscount()
        {
            Console.WriteLine("Load discount");
        }
    }
}
