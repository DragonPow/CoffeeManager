using MainProject.MainWorkSpace.Bill;
using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    public class HistoryViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Lịch sử";
        private const PackIconKind _iconDisplay = PackIconKind.ClipboardTextSearchOutline;
        public mainEntities db = new mainEntities();
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }

        #region Fields
        private ObservableCollection<BILL> _ListBill;
        private BILL _CurrentBill;
        private int _NumberPage;
        public static int Number_Bill_in_Page = 13;

        private DateTime? _BeginTime;
        private DateTime? _EndTime;

        ICommand _Load_Detail_Bill;
        ICommand _Exit_Detail_Bill;
        ICommand _Search_Bill;
        #endregion

        #region Properties
        public ObservableCollection<BILL> ListBill
        {
            get => _ListBill;
            set
            {
                if (_ListBill != value)
                {
                    _ListBill = value;
                    OnPropertyChanged();
                }
            }
        }
        public BILL CurrentBill
        {
            get => _CurrentBill;
            set
            {
                if (value != _CurrentBill)
                {
                    _CurrentBill = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberPage
        {
            get
            {
                return _NumberPage;
            }

            set
            {
                if (value != _NumberPage)
                {
                    if (value > NumberAllPage)
                    {
                        _NumberPage = NumberAllPage;
                    }
                    else if (value < 1)
                    {
                        if (NumberAllPage != 0) _NumberPage = 1;
                        else _NumberPage = 0;
                    }
                    else
                    {
                        _NumberPage = value;
                        LoadBillByNumberPage();
                    }

                    OnPropertyChanged();
                }
            }
        }
        public int NumberAllPage
        {
            get;
            set;
        }
        public DateTime BeginTime
        {
            get
            {
                if (_BeginTime.HasValue)
                {
                    return _BeginTime.Value;
                }
                else return DateTime.Now.Date;
            }
            set
            {
                if (value != _BeginTime)
                {
                    _BeginTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (_EndTime.HasValue)
                {
                    return _EndTime.Value;
                }
                else return DateTime.Now.Date;
            }
            set
            {
                if (value != _EndTime)
                {
                    _EndTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEndPage
        {
            get => NumberPage == NumberAllPage;
        }
        #endregion
        #region Init
        public HistoryViewModel()
        {

        }
        #endregion

        #region Command

        public ICommand Load_Detail_BillCommand
        {
            get
            {
                if (_Load_Detail_Bill == null)
                    _Load_Detail_Bill = new RelayingCommand<object>(a => Load_Detail_Bill());
                return _Load_Detail_Bill;
            }
        }

        public void Load_Detail_Bill()
        {

            BillView view = new BillView();
            view.DataContext = new BillViewModel() { Total = CurrentBill.TotalPrice, CurrentBill = CurrentBill };
            view.Show();

        }

        public ICommand Search_BillCommand
        {
            get
            {
                if (_Search_Bill == null)
                    _Search_Bill = new RelayingCommand<object>(a => 
                    {
                        try
                        {
                            Search_Bill();
                        }
                        catch (ArgumentNullException e1)
                        {
                            WindowService.Instance.OpenMessageBox("Phải điền đủ các giá trị", "Thông báo", System.Windows.MessageBoxImage.Warning);
                        }
                        catch (ArgumentException e2)
                        {
                            WindowService.Instance.OpenMessageBox("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Lỗi", System.Windows.MessageBoxImage.Error);
                        }
                    });
                return _Search_Bill;
            }
        }

        public void Search_Bill()
        {
            //if (Number_Bill_in_Page <= 0)
            //{
            //    WindowService.Instance.OpenMessageBox("Số lượng bill mỗi trang phải lớn hơn 0!!", "Lỗi hệ thống", System.Windows.MessageBoxImage.Error);
            //    NumberPage = 0;
            //    return;
            //}
            
            if (EndTime == null || BeginTime == null)
            {
                throw new ArgumentNullException("BeginTime or EndTime is null");
            }

            if (EndTime.Date < BeginTime.Date)
            {
                throw new ArgumentException("BeginTime is greater than EndTime");
            }
            DateTime newEndTime = EndTime.AddDays(1);
            //using (var db = new mainEntities())
            {
                int d = db.BILLs.Where(b => b.CheckoutDay >= BeginTime && b.CheckoutDay <= newEndTime).Count();

                if (d == 0)
                {
                    NumberAllPage = 0;
                    NumberPage = 0;
                    ListBill = null;
                    return;
                }

                NumberAllPage = (d / Number_Bill_in_Page) + (d % Number_Bill_in_Page != 0 ? 1 : 0);
                if (NumberPage == 1) LoadBillByNumberPage();
                else NumberPage = 1;
            }
        }

        #endregion

        void LoadBillByNumberPage()
        {
            DateTime newEndTime = EndTime.AddDays(1);
            //using (var db = myContext)
            {
                var listbill = (from b in db.BILLs.Where(b => b.CheckoutDay >= BeginTime && b.CheckoutDay <= newEndTime)
                                from t in db.TABLEs
                                .Where(t => t.ID == b.ID_Table)
                                .DefaultIfEmpty()
                                select new
                                {
                                    ID = b.ID,
                                    CheckoutDay = b.CheckoutDay,
                                    TotalPrice = b.TotalPrice,
                                    MoneyCustomer = b.MoneyCustomer,
                                    Table = t
                                }).OrderBy(b => b.ID).Skip((NumberPage - 1) * Number_Bill_in_Page).Take(Number_Bill_in_Page);

                if (listbill == null || listbill.ToList().Count == 0) return;

                ListBill = new ObservableCollection<BILL>();

                foreach (var b in listbill.ToList())
                {
                    ListBill.Add(new BILL() { 
                        CheckoutDay = b.CheckoutDay, 
                        ID = b.ID, 
                        TotalPrice = b.TotalPrice,
                        MoneyCustomer = b.MoneyCustomer,
                        TABLE = b.Table 
                    });
                }
            }
        }
    }
}
