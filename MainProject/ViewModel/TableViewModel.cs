
using MainProject.MainWorkSpace.Bill;
using MainProject.MainWorkSpace.Table;
using MainProject.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Data.Entity;

namespace MainProject.ViewModel
{
    public class TableViewModel : BaseViewModel, IMainWorkSpace
    {
        public string NameWorkSpace => "Quản lý bàn";
        private const PackIconKind _iconDisplay = PackIconKind.TableChair;
        public PackIcon IconDisplay
        {
            get
            {
                return new PackIcon() { Kind = _iconDisplay, Width = 30, Height = 30 };
            }
        }
        public IContext Context = new mainEntities();

        #region Field
        private ObservableCollection<TABLECUSTOM> _ListTable;
        private ObservableCollection<int> _ListFloor;
        private int _Currentfloors;
        private TABLECUSTOM _CurrentTable;
        private TABLECUSTOM _CurrentTableInTabManager;
        private DetailPro _CurrentDetailPro;
        private ObservableCollection<DetailPro> _Currentlistdetailpro;
        private long _TotalCurrentTable;
        private bool _Isbringtohome;

        private BillViewModel _Billviewmodel;

        private ICommand _plusQuantityDetailProCommand;
        private ICommand _minusQuantityDetailProCommand;
        private ICommand _ClickQuantityDetailProCommand;
        private ICommand _DeleteDetailPro;

        private ICommand _OpenViewChooseTable;
        private ICommand _CloseViewChooseTable;

        private ICommand _PayCommand;

        private ICommand _DeleteTableCommand;
        private ICommand _InsertTableCommand;
        private ICommand _UpdateStatusTableCommand;
        private ICommand _UpdateStatusNormalTableCommand;
        private ICommand _UpdateStatusNomalTable;

        private ICommand _AddFloor;
        private ICommand _DeleteFloor;

        private string _TableName = "Chọn bàn";

        #endregion


        #region Init


        public TableViewModel()
        {
            TotalCurrentTable = 0;

            LoadTable();
        }
       //init for unittest
        public TableViewModel( bool t)
        {
            TotalCurrentTable = 0;
        }

        public TableViewModel(mainEntities Context)
        {
            this.Context = Context;
        }
        #endregion



        #region Properties

        public bool Isbringtohome
        {
            get => _Isbringtohome;
            set
            {
                if (_Isbringtohome != value)
                {
                    _Isbringtohome = value;
                    OnPropertyChanged();
                    if (value == true)
                    {
                        TableName = "Mang về";
                        if (CurrentTable != null)
                        {
                            CurrentTable.table.CurrentStatus = "Normal";
                            CurrentTable = null;
                        }
                    }
                    else
                         if (CurrentTable == null) TableName = "Chọn bàn";
                }
            }
        }
        public BillViewModel Billviewmodel
        {
            get => _Billviewmodel;
            set
            {
                if (_Billviewmodel != value)
                {
                    _Billviewmodel = value;
                    OnPropertyChanged();
                }
            }
        }
        public long TotalCurrentTable { get => _TotalCurrentTable; set { if (_TotalCurrentTable != value) { _TotalCurrentTable = value; OnPropertyChanged(); } } }
        public ObservableCollection<DetailPro> Currentlistdetailpro
        {
            get => _Currentlistdetailpro;
            set
            {
                if (_Currentlistdetailpro != value)
                {
                    _Currentlistdetailpro = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<int> ListFloor
        {
            get => _ListFloor;
            set
            {
                if (_ListFloor != value)
                {
                    _ListFloor = value; OnPropertyChanged();
                }
            }
        }

        /*  public int CurrentFloors
          {
              get => _Currentfloors;
              set
              {
                  if (value != _Currentfloors)
                  {
                      _Currentfloors = value;
                      OnPropertyChanged();

                      using (var db = new mainEntities())
                      {
                          var listtab = db.TABLEs.Where(t => (t.Floor == CurrentFloors)).ToList();

                          if (listtab == null) return;

                          List<TABLECUSTOM> Tablecustoms = new List<TABLECUSTOM>();

                          foreach (TABLE t in listtab)
                          {
                              Tablecustoms.Add(new TABLECUSTOM() { Total = 0, table = t, ListPro = null });
                          }

                          ListTable = new ObservableCollection<TABLECUSTOM>(Tablecustoms);

                      }

                  }
              }
          }*/
        public TABLECUSTOM CurrentTable
        {
            get => _CurrentTable;
            set
            {
                if (value != _CurrentTable)
                {
                    if (value != null)
                    {
                        if (value.table.CurrentStatus == "Fix" || value.table.CurrentStatus == "Already")
                        {
                            WindowService.Instance.OpenMessageBox(value.table.CurrentStatus == "Fix" ? "Không thể chọn bàn đang sữa chữa" : "Không thể chọn bàn đang có khách", "Lỗi", MessageBoxImage.Error);
                            return;
                        }
                        TableName = "Bàn: " + value.table.Name.ToString();
                        value.table.CurrentStatus = "Already";
                        if (_CurrentTable != null) _CurrentTable.table.CurrentStatus = "Normal";
                        _CurrentTable = value;
                        Isbringtohome = false;
                    }
                    else
                    {
                        if (!Isbringtohome) TableName = "Chọn bàn";
                        _CurrentTable = value;
                    }
                    OnPropertyChanged();
                }
            }

        }

        public TABLECUSTOM CurrentTableInTabManager
        {
            get => _CurrentTableInTabManager;
            set
            {
                if (value != _CurrentTableInTabManager)
                {
                    _CurrentTableInTabManager = value;
                    OnPropertyChanged();
                }
            }

        }

        public DetailPro CurrentDetailPro
        {
            get => _CurrentDetailPro;
            set
            {
                if (value != _CurrentDetailPro)
                {
                    _CurrentDetailPro = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TABLECUSTOM> ListTable
        {
            get
            {
                if (_ListTable == null)
                {
                    _ListTable = new ObservableCollection<TABLECUSTOM>();
                }
                return _ListTable;
            }
            set
            {
                if (_ListTable != value)
                {
                    _ListTable = value;
                    OnPropertyChanged();
                }
            }
        }


        public string TableName
        {
            get => _TableName;
            set
            {
                if (_TableName != value)
                {
                    _TableName = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Command

        public ICommand PlusDetailProQuantityCommand
        {
            get
            {
                if (_plusQuantityDetailProCommand == null)
                    _plusQuantityDetailProCommand = new RelayingCommand<DetailPro>(a => Plus(a));
                return _plusQuantityDetailProCommand;
            }
        }

        public void Plus(DetailPro pro)
        {
            CurrentDetailPro = pro;
            CurrentDetailPro.Quantity++;
            TotalCurrentTable += (long)CurrentDetailPro.Pro.Price;

        }
        public ICommand MinusDetailProQuantityCommand
        {
            get
            {
                if (_minusQuantityDetailProCommand == null)
                    _minusQuantityDetailProCommand = new RelayingCommand<DetailPro>(a => Minus(a));

                return _minusQuantityDetailProCommand;
            }

        }

        public void Minus(DetailPro pro)
        {
            CurrentDetailPro = pro;

            if (CurrentDetailPro == null || CurrentDetailPro.Quantity < 1) return;

            CurrentDetailPro.Quantity--;
            TotalCurrentTable -= (long)CurrentDetailPro.Pro.Price;

            if (CurrentDetailPro.Quantity == 0) Currentlistdetailpro.Remove(CurrentDetailPro);
        }

        public ICommand ClickQuantityDetailProCommand
        {
            get
            {
                if (_ClickQuantityDetailProCommand == null)
                {
                    _ClickQuantityDetailProCommand = new RelayingCommand<int>(a => ChangeQuantityCommand(a));
                }
                return _ClickQuantityDetailProCommand;
            }
        }

        public void ChangeQuantityCommand(int Number)
        {
            TotalCurrentTable -= CurrentDetailPro.Quantity * (int)CurrentDetailPro.Pro.Price;
            CurrentDetailPro.Quantity = Number;
            TotalCurrentTable += Number * (int)CurrentDetailPro.Pro.Price;
        }


        public ICommand OpenViewChooseTableCommand
        {
            get
            {
                if (_OpenViewChooseTable == null)
                {
                    _OpenViewChooseTable = new RelayingCommand<Object>(a => OpenChooseTable());
                }
                return _OpenViewChooseTable;
            }
        }

        public void OpenChooseTable()
        {
            SelectTableView v = new SelectTableView();
            WindowService.Instance.OpenWindowWithoutBorderControl(this, v);
        }

        public ICommand CloseViewChooseTableCommand
        {
            get
            {
                if (_CloseViewChooseTable == null)
                {
                    _CloseViewChooseTable = new RelayingCommand<Object>(a => CloseChooseTable());
                }
                return _CloseViewChooseTable;
            }
        }

        public void CloseChooseTable()
        {
            Window window = WindowService.Instance.FindWindowbyTag("Selected Table").First();
            window.Close();
        }

        public ICommand DeleteDetailProCommand
        {
            get
            {
                if (_DeleteDetailPro == null)
                {
                    _DeleteDetailPro = new RelayingCommand<Object>(a => RemoveDetail());
                }
                return _DeleteDetailPro;
            }
        }
        public void RemoveDetail()
        {
            TotalCurrentTable -= CurrentDetailPro.Quantity + (int)CurrentDetailPro.Pro.Price;
            Currentlistdetailpro.Remove(CurrentDetailPro);
        }

        public ICommand Pay_Command
        {
            get
            {
                if (_PayCommand == null)
                {
                    _PayCommand = new RelayingCommand<Object>(a =>
                    {
                        try
                        {
                            Pay(Isbringtohome, CurrentTable);
                            OpenBillView();
                        }
                        catch (ArgumentException e)
                        {
                            switch (e.ParamName)
                            {
                                case "TableNULL":
                                    WindowService.Instance.OpenMessageBox("Chưa chọn bàn!", "Lỗi", MessageBoxImage.Error);
                                    return;
                                   
                                case "ListProNULL":
                                    WindowService.Instance.OpenMessageBox("Chưa chọn món!", "Lỗi", MessageBoxImage.Information);
                                    return;
                                  
                            }
                        }
                    });
                }
                return _PayCommand;
            }
           
        }
        public void Pay( bool Isbringtohome, TABLECUSTOM CurrentTable)
        {
            if (Isbringtohome)
            {
                CurrentTable = new TABLECUSTOM() { table = new TABLE() };
            }
            if (CurrentTable == null && !Isbringtohome)
            {
                throw new ArgumentException("Chưa chọn bàn!", "TableNULL");
             
            }

            if (Currentlistdetailpro == null || Currentlistdetailpro.Count == 0)
            {
                throw new ArgumentException("Chưa chọn món!", "ListProNULL");
               
            }

            
        }

        private void OpenBillView()
        {
            CurrentTable.ListPro = Currentlistdetailpro;

            Billviewmodel = new BillViewModel(CurrentTable);

            BillView billView = new BillView();
            billView.DataContext = Billviewmodel;

            billView.ShowDialog();

            if (Billviewmodel.IsClose)
            {
                this.CurrentTable = null;
                Currentlistdetailpro = new ObservableCollection<DetailPro>();
                TotalCurrentTable = 0;
            }
        }


        public ICommand DeleteTableCommand
        {
            get
            {
                if (_DeleteTableCommand == null)
                {
                    _DeleteTableCommand = new RelayingCommand<object>(a =>
                    {
                        try
                        {
                            Delete();
                        }
                        catch (ArgumentException e)
                        {
                            switch (e.ParamName)
                            {
                                case "NotHaveTable":
                                    WindowService.Instance.OpenMessageBox("Không còn bàn để xóa", "Lỗi", MessageBoxImage.Error);
                                    break;
                                case "NotPayment":
                                    WindowService.Instance.OpenMessageBox("Vui lòng thanh toán bàn " + (ListTable.Count - 1) + " trước khi xóa!", "Thông báo", MessageBoxImage.Information);
                                    break;
                            }
                        }
                    });
                }
                return _DeleteTableCommand;
            }
        }

        public void Delete()
        {
            if (ListTable == null || ListTable.Count == 0)
            {
                throw new ArgumentException("Not table to delete", "NotHaveTable");
            }
            int number = ListTable.Count - 1;

            if (ListTable[number].table.CurrentStatus == "Already")
            {
                throw new ArgumentException("The table is not payment","NotPayment");
            }
            long id_remove = ListTable[number].table.ID;
            ListTable.RemoveAt(number);

            //long max = Context.TABLEs.Max(p => p.ID);
            Context.TABLEs.Remove(Context.TABLEs.Where(t => t.ID == id_remove).First());
            Context.SaveChanges();
        }

        public ICommand InsertTableCommand
        {
            get
            {
                if (_InsertTableCommand == null)
                {
                    _InsertTableCommand = new RelayingCommand<object>(a => Insert());
                }
                return _InsertTableCommand;
            }
        }

        public void Insert()
        {
            TABLE tab;


            {
                int number = Context.TABLEs.Count();
                tab = new TABLE() { STATUS_TABLE = Context.STATUS_TABLE.Where(i => i.Status == "Normal").FirstOrDefault(), Name = number == 0 ? 1 : Context.TABLEs.Max(t => t.Name) + 1 };

                Context.TABLEs.Add(tab);
                Context.SaveChanges();
            }

            ListTable.Add(new TABLECUSTOM() { table = tab });
        }

        public ICommand UpdateStatusFixTableCommand
        {
            get
            {
                if (_UpdateStatusTableCommand == null)
                {
                    _UpdateStatusTableCommand = new RelayingCommand<Object>(a => UpdateFix());
                }
                return _UpdateStatusTableCommand;
            }
        }

        public void UpdateFix()
        {
            if (CurrentTableInTabManager == CurrentTable && Currentlistdetailpro.Count != 0)
            {
                WindowService.Instance.OpenMessageBox("Vui lòng thanh toán bàn trước khi cập nhật!", "Lỗi", MessageBoxImage.Error);
                return;
            }
            if (CurrentTableInTabManager.table.CurrentStatus == "Fix")
            {
                return;
            }
            CurrentTableInTabManager.table.CurrentStatus = "Fix";

            {
                var t = Context.TABLEs.Where(tab => tab.ID == CurrentTableInTabManager.table.ID).FirstOrDefault();
                t.ID_Status = 2;
                Context.SaveChanges();
            }

        }

        public ICommand UpdateStatus_Done_FixTableCommand
        {
            get
            {
                if (_UpdateStatusNormalTableCommand == null)
                {
                    _UpdateStatusNormalTableCommand = new RelayingCommand<Object>(a => UpdateStatusNormalTable());
                }
                return _UpdateStatusNormalTableCommand;
            }
        }

        public void UpdateStatusNormalTable()
        {
            if (CurrentTableInTabManager.table.CurrentStatus == "Nomal")
            {
                return;
            }
            if (CurrentTableInTabManager == CurrentTable && Currentlistdetailpro.Count != 0)
            {
                WindowService.Instance.OpenMessageBox("Vui lòng thanh toán bàn trước khi cập nhật!", "Lỗi", MessageBoxImage.Error);
                return;
            }
            CurrentTableInTabManager.table.CurrentStatus = "Normal";

            {
                var t = Context.TABLEs.Where(tab => tab.ID == CurrentTableInTabManager.table.ID).FirstOrDefault();
                t.ID_Status = 1;
                Context.SaveChanges();
            }

        }
        public ICommand UpdateStatus_Leave_TableCommand
        {
            get
            {
                if (_UpdateStatusNomalTable == null)
                {
                    _UpdateStatusNomalTable = new RelayingCommand<Object>(a => UpdateStatus_Leave_Table());
                }
                return _UpdateStatusNomalTable;
            }
        }

        public void UpdateStatus_Leave_Table()
        {
            if (CurrentTableInTabManager.table.CurrentStatus == "Fix" || CurrentTableInTabManager.table.CurrentStatus == "Normal")
            {
                return;
            }
            if (CurrentTableInTabManager == CurrentTable && Currentlistdetailpro.Count != 0)
            {
                WindowService.Instance.OpenMessageBox("Vui lòng thanh toán bàn trước khi cập nhật!", "Lỗi", MessageBoxImage.Error);
                return;
            }
            CurrentTableInTabManager.table.CurrentStatus = "Normal";

            {
                var t = Context.TABLEs.Where(tab => tab.ID == CurrentTableInTabManager.table.ID).FirstOrDefault();
                t.ID_Status = 1;
                Context.SaveChanges();
            }

        }

        #endregion
        void LoadTable()
        {

            {
                var listtab = Context.TABLEs.Include(p => p.STATUS_TABLE).ToList();
                if (listtab == null) return;

                foreach (TABLE t in listtab)
                {
                    ListTable.Add(new TABLECUSTOM() { table = t, ListPro = null });
                }
            }
        }
    }
}