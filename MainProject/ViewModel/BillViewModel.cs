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

        private BILL _CurrentBill;
        private string _CodeDiscount;
        private int _Discount;
        private long _Total;
        private TABLECUSTOM _Current_table;
        private bool IsDiscount = false;
        private int _BillCode;

        private long _GiveMoney;
        private long _Refund;

        public bool IsClose = false;

        private ObservableCollection<DETAILBILL> _ListDetailBill;
        //StoreInfor : namestore, phone, address

        private ICommand _PaymentCommand;
        private ICommand _CheckDiscountCommand;


        #endregion


        #region Properties
        public BILL CurrentBill
        {
            get
            {
                return _CurrentBill;
            }
            set
            {
                if (value != _CurrentBill)
                {
                    _CurrentBill = value;
                    OnPropertyChanged();

                }
            }
        }

        public int Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }
        public int BillCode
        {
            get
            {
                using (var db = new mainEntities())
                {
                    return db.BILLs.Count() + 1;
                }
            }
            set
            {
                if (_BillCode != value)
                {
                    _BillCode = value;
                }
            }
        }
        public long Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CodeDiscount
        {
            get { return _CodeDiscount; }
            set
            {
                if (value != _CodeDiscount)
                {
                    _CodeDiscount = value;
                    OnPropertyChanged();
                }
            }
        }


        public TABLECUSTOM CurrentTable
        {
            get => _Current_table;
            set
            {
                if (value != _Current_table)
                {
                    _Current_table = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<DETAILBILL> ListDetailBill
        {
            get => _ListDetailBill;
            set
            {
                if (_ListDetailBill != value)
                {
                    _ListDetailBill = value;
                    OnPropertyChanged();

                }
            }
        }

        public long GiveMoney
        {
            get { return _GiveMoney; }
            set
            {
                if (_GiveMoney != value)
                {
                    _GiveMoney = value;
                    OnPropertyChanged();
                    Refund = Total - GiveMoney;
                }
            }
        }

        public long Refund
        {
            get { return _Refund; }
            set
            {
                if (_Refund != value)
                {
                    _Refund = value;
                    OnPropertyChanged();
                }
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
                if (_PaymentCommand == null)
                {
                    _PaymentCommand = new RelayingCommand<BillView>(para => Payment(para));
                }
                return _PaymentCommand;
            }
        }



        /// <summary>
        /// Auto load discount percent when eligible
        /// </summary>
        /*  public ICommand CheckDiscountCommand
          {
              get
              {
                  if (_CheckDiscountCommand == null)
                  {
                      _CheckDiscountCommand = new RelayingCommand<Object>(para => LoadDiscount());
                  }
                  return _CheckDiscountCommand;
              }
          }*/
        #endregion


        #region Constructors
        public BillViewModel()
        {
            CurrentBill = new BILL();
        }
        public BillViewModel(TABLECUSTOM Table)
        {
            CurrentBill = new BILL();
            CurrentTable = Table;

            foreach (var p in CurrentTable.ListPro)
            {
                CurrentBill.DETAILBILLs.Add(new DETAILBILL() { Amount = p.Quantity, ID_Product = p.Pro.ID });
            }

            Discount = 0;
            CurrentBill.CheckoutDay = DateTime.Now;
            Total = CurrentTable.Total;
        }

        #endregion

        private void Payment(BillView view)
        {
            if (GiveMoney == null || GiveMoney < Total)
            {
                WindowService.Instance.OpenMessageBox("Tiền khách đưa không đủ!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }

            CurrentBill.ID_Tables = CurrentTable.table.ID;
            CurrentBill.TotalPrice = Total;

            using (var db = new mainEntities())
            {
                db.BILLs.Add(CurrentBill);
                db.SaveChanges();
            }

            CurrentTable.ListPro = null;
            CurrentTable.Total = 0;
            IsClose = true;

            view.Close();

            //Xuất đơn ra PDF  
        }
    }
      /*  private void LoadDiscount()
        {

            if (IsDiscount)
            {
                WindowService.Instance.OpenMessageBox("Đã sử dụng mã khác!", "Lỗi", System.Windows.MessageBoxImage.Error);
                return;
            }
            using (var db = new mainEntities())
            {

                var t = db.VOUCHERs.Where(v => (v.CODE == CodeDiscount && v.DELETED == 0 && v.BeginTime <= DateTime.Now && v.EndTime >= DateTime.Now)).FirstOrDefault();
                if (t != null && !IsDiscount)
                {
                    CurrentBill.ID_Voucher = t.ID;
                    Discount = (int)(CurrentTable.Total * t.Percent) / 100;
                    Total -= Discount;
                    IsDiscount = true;
                }
                else
                {
                        Total = CurrentTable.Total;
                        CurrentBill.ID_Voucher = null;
                        CodeDiscount = "";
                        Discount = 0;
                       
                    WindowService.Instance.OpenMessageBox("Nhập sai mã!", "Lỗi", System.Windows.MessageBoxImage.Error);
                }
            }
        }
*/
}
