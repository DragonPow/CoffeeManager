using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MainProject.MainWorkSpace.Bill
{
    public class BillViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Fields

        private BILL _CurrentBill;
        private string _CodeDiscount;
        private int _Discount;
        private long _Total;
        private TABLECUSTOM _Current_table;
        private int _BillCode;

        private long _GiveMoney;

        public bool IsClose = false;

        //private ObservableCollection<DETAILBILL> _ListDetailBill;
        //StoreInfor : namestore, phone, address

        private ICommand _PaymentCommand;


        #endregion
        public string Error { get => null; }
        public mainEntities Context = new mainEntities();

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
            get => Context.BILLs.Count() + 1;
            //set
            //{
            //    if (_BillCode != value)
            //    {
            //        _BillCode = value;
            //    }
            //}
        }
        public long Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                    OnPropertyChanged("Refund");
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

        //public ObservableCollection<DETAILBILL> ListDetailBill
        //{
        //    get => _ListDetailBill;
        //    set
        //    {
        //        if (_ListDetailBill != value)
        //        {
        //            _ListDetailBill = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        public long GiveMoney
        {
            get { return _GiveMoney; }
            set
            {
                if (_GiveMoney != value)
                {
                    _GiveMoney = value;
                    OnPropertyChanged("Refund");
                    OnPropertyChanged();
                }
            }
        }

        public long Refund
        {
            get
            {
                if (GiveMoney - Total < 0) return 0;
                else return GiveMoney - Total;
            }

        }
        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "Refund")
                {
                    if (Refund < 0)
                    {
                        result = "Yêu cầu đưa thêm tiền";
                    }
                }
                return result;
            }
        }

        public string StoreName
        {
            get
            {
                var name = Context.PARAMETERs.Where(p => p.NAME == "StoreName").FirstOrDefault();
                if (name != null) return name.Value;
                return null;
            }
        }

        public string StorePhone
        {
            get
            {
                var name = Context.PARAMETERs.Where(p => p.NAME == "StorePhone").FirstOrDefault();
                if (name != null) return name.Value;
                return null;
            }
        }

        public string StoreAddress
        {
            get
            {
                var name = Context.PARAMETERs.Where(p => p.NAME == "StoreAddress").FirstOrDefault();
                if (name != null) return name.Value;
                return null;
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
                    _PaymentCommand = new RelayingCommand<BillView>(para =>
                    {
                        try
                        {
                            Payment();

                            //Đóng cửa sổ
                            WindowService.Instance.FindWindowbyTag("BillView").First().Close();

                            //Xuất đơn ra PDF
                            PrintPDF.Instance.createBill(CurrentBill);
                        }
                        catch (InvalidOperationException e)
                        {
                            WindowService.Instance.OpenMessageBox("Tiền đưa không đủ!", "Thông báo", MessageBoxImage.Information);
                        }
                    });
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

            Discount = 0;
            CurrentBill.CheckoutDay = DateTime.Now;
            Total = CurrentTable.Total;
        }

        #endregion

        public void Payment()
        {
            if (GiveMoney - Total < 0)
            {
                throw new InvalidOperationException("Money Customer give is lower than Total price of bill");
            }

            CurrentBill.ID_Table = CurrentTable.table.ID;
            CurrentBill.TotalPrice = Total;
            CurrentBill.MoneyCustomer = GiveMoney;

            foreach (var p in CurrentTable.ListPro)
            {
                CurrentBill.DETAILBILLs.Add(new DETAILBILL()
                {
                    Quantity = p.Quantity,
                    UnitPrice = (long)p.Pro.Price,
                    PRODUCT = Context.PRODUCTs.FirstOrDefault(i => i.ID == p.Pro.ID)
                });
            }

            foreach (var i in CurrentBill.DETAILBILLs)
            {
                Context.SetUnchanged(i.PRODUCT);
                //Context.Entry(i.PRODUCT).State = System.Data.Entity.EntityState.Unchanged;
            }
            Context.BILLs.Add(CurrentBill);
            Context.SaveChanges();

            CurrentTable.ListPro = null;
            IsClose = true;

            //Đổi giá trị bàn thành "Already"
            /* CurrentTable.table.CurrentStatus = "Already";*/
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
