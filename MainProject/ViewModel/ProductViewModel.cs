
using MainProject.MainWorkSpace.Product;
using MainProject.Model;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;




namespace MainProject.ViewModel
{ 
    class ProductViewModel : BaseViewModel
    {
        #region Field

        private ObservableCollection<PRODUCT> _ListProduct;
        private int _IndexCurrentproduct;
        private TYPE_PRODUCT _Type;
        private TYPE_PRODUCT _Type_In_Edit_CATEGORY;
        string _SearchProduct;
        string _Type_in_Combobox_AddPro;
        TYPE_PRODUCT _Type_in_Combobox_AddProduct;
        PRODUCT _Newproduct;

        TableViewModel _Tableviewmodel;
        
        private ICommand _DeletePro;

        private ICommand _SearchByName;
        private ICommand _SearchByType;


        private ICommand _LoadViewUpdateProduct;
        private ICommand _UpdateProduct;
        private ICommand _ExitUpdateProduct;

        private ICommand _LoadAddProview;
        private ICommand _AddProduct;
        private ICommand _CancelAddProduct;
        private ICommand _ExitAddProview;

        
        private ICommand _ExitDetailProduct;
        private ICommand _OpenViewDetailProduct;
        private ICommand _AddDetailProToTableCommand;

        private ICommand _AddImageProduct;

        private ICommand _OpenViewEditCategory;
        private ICommand _ClickCheckboxSelectedPro;
        private ICommand _SaveEditCategory;
        #endregion


        #region Properties

        public ObservableCollection<PRODUCT> ListPoduct { get => _ListProduct; set { if (value != _ListProduct) { _ListProduct = value; OnPropertyChanged(); } } }

        public int IndexCurrentProduct { get => _IndexCurrentproduct; set { if (_IndexCurrentproduct != value) { _IndexCurrentproduct = value; OnPropertyChanged(); } } }
     
        public PRODUCT Newproduct 
        { 
            get => _Newproduct; 
            set 
            { 
                if (_Newproduct != value) 
                { 
                    _Newproduct = value; 
                    OnPropertyChanged();                   
                } 
            } 
        }

        public string SearchProduct { get => _SearchProduct; set { if (_SearchProduct != value) { _SearchProduct = value; OnPropertyChanged(); } } }
        public string Type_in_Combobox_AddPro { get => _Type_in_Combobox_AddPro; set { if (_Type_in_Combobox_AddPro != value) { _Type_in_Combobox_AddPro= value; OnPropertyChanged(); } } }

        public TYPE_PRODUCT Type_in_Combobox_AddProduct { get => _Type_in_Combobox_AddProduct; set { if (_Type_in_Combobox_AddProduct != value) { _Type_in_Combobox_AddProduct = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT Type { get => _Type; set { if (_Type != value) { _Type = value; OnPropertyChanged(); LoadProductByType(value.Type); } } }
        public TYPE_PRODUCT Type_In_Edit_CATEGORY { get => _Type_In_Edit_CATEGORY; set { if (_Type_In_Edit_CATEGORY != value) { _Type_In_Edit_CATEGORY = value; OnPropertyChanged(); } } }

        public TableViewModel Tableviewmodel { get => _Tableviewmodel; set { if (_Tableviewmodel != value) { _Tableviewmodel = value; OnPropertyChanged(); } } }
        #endregion

        #region Init

        public ProductViewModel()
        {
           /* Newproduct = new CUSTOMPRODUCT() { product = new PRODUCT() { DELETED = 0, Image = null, TYPE_PRODUCT = new ObservableCollection<TYPE_PRODUCT>() } };*/
                    /* lát phải xóa dòng trên đầu nhó*/
            Type = new TYPE_PRODUCT() { Type ="Tất cả"} ;

        }

        #endregion

        #region Icommand


        public ICommand LoadAddProductView_Command
        {
            get
            {
                if (_LoadAddProview == null)
                {
                    _LoadAddProview = new RelayingCommand<Object>(a => Loadaddproview());
                }
                return _LoadAddProview;
            }
        }


        public void Loadaddproview()
        {

            Newproduct =new PRODUCT() { DELETED = 0, Image = null, TYPE_PRODUCT = new TYPE_PRODUCT() } ;

            WindowService.Instance.OpenWindow(this, new CreateProd());
        }

        public ICommand AddProduct_Command_Command
        {
            get
            {
                if (_AddProduct == null)
                {
                    _AddProduct = new RelayingCommand<Object>(a => Add(a));
                }
                return _AddProduct;
            }
        }


        public void Add(object a)
        {
            using (var db = new mainEntities())
            {
                {
                    TYPE_PRODUCT type= db.TYPE_PRODUCT.Where(t => (t.Type == Type_in_Combobox_AddPro)).FirstOrDefault();
                
                    if (type == null) return;
                    Newproduct.TYPE_PRODUCT = type ;                


                    db.PRODUCTs.Add(Newproduct);

                    db.SaveChanges();
                }
            }

            ListPoduct.Add(Newproduct);
        }

        public ICommand CancelAddProduct_Command
        {
            get
            {
                if (_CancelAddProduct == null)
                {
                    _CancelAddProduct = new RelayingCommand<Object>(a => CancelAddProduct(a));
                }
                return _CancelAddProduct;
            }
        }


        public void CancelAddProduct(object a)
        {

            Newproduct = null;
        }

        public ICommand ExitAddProductView_Command
        {
            get
            {
                if (_ExitAddProview == null)
                {
                    _ExitAddProview = new RelayingCommand<Object>(a => Exitaddproview(a));
                }
                return _ExitAddProview;
            }
        }


        public void Exitaddproview(Object a)
        {
            Newproduct = null;
            //Exit view Add_pro(a)
        }


        public ICommand DeleteProduct_Command
        {
            get
            {
                if (_DeletePro == null)
                {
                    _DeletePro = new RelayingCommand<Object>(a => DeletePro());
                }
                return _DeletePro;
            }
        }

        public void DeletePro()
        {
            if (IndexCurrentProduct == null) return;

            using (var db = new mainEntities())
            {
                PRODUCT product = db.PRODUCTs.Where(p => (p.ID == ListPoduct.ElementAt(IndexCurrentProduct).ID) && (p.DELETED == 0)).FirstOrDefault();
                if (product != null)
                {
                    product.DELETED = 1;
                }
                db.SaveChanges();
            }
            ListPoduct.Remove(ListPoduct.ElementAt(IndexCurrentProduct));
        }

        public ICommand SearchByName_Command
        {
            get
            {
                if (_SearchByName == null)
                {
                    _SearchByName = new RelayingCommand<Object>(a => SearchName());
                }
                return _SearchByName;
            }
        }

        public void SearchName()
        {
            ObservableCollection<PRODUCT> listproduct;
            if (SearchProduct == "")
            {
                using (var db = new mainEntities())
                {
                    listproduct = new ObservableCollection<PRODUCT>(db.PRODUCTs.ToList());
                }
            }
            else
            {
                using (var db = new mainEntities())
                {
                    var listpro = db.PRODUCTs.Where(p => (ConvertToUnSign(p.Name).ToLower().Contains(ConvertToUnSign(SearchProduct).ToLower()) && p.DELETED == 0));
                    if (listpro == null) return;
                    listproduct = new ObservableCollection<PRODUCT>(listpro.ToList());
                }
            }

            foreach (PRODUCT p in listproduct)
            {
                ListPoduct.Add(p);
            }


        }

        public ICommand SearchByType_Command
        {
            get
            {
                if (_SearchByType == null)
                {
                    _SearchByType = new RelayingCommand<Object>(a => SearchType());
                }
                return _SearchByType;
            }
        }
        public void SearchType()
        {
            ObservableCollection<PRODUCT> listproduct;
            using (var db = new mainEntities())
            {
                var listpro = db.PRODUCTs.Where(p => (p.TYPE_PRODUCT.Type == Type.Type && p.DELETED == 0));
                if (listpro == null) return;
                listproduct = new ObservableCollection<PRODUCT>(listpro.ToList());
            }

            foreach (PRODUCT p in listproduct)
            {
                ListPoduct.Add(p);
            }

        }

        public ICommand LoadViewUpdateProduct_Command
        {
            get
            {
                if (_LoadViewUpdateProduct == null)
                {
                    _LoadViewUpdateProduct = new RelayingCommand<Object>(a => LoadViewUpdate());
                }
                return _LoadViewUpdateProduct;
            }
        }

        public void LoadViewUpdate()
        {
            //open viewUpdatate
        }

        public ICommand UpdateProduct_Command
        {
            get
            {
                if (_UpdateProduct == null)
                {
                    _UpdateProduct = new RelayingCommand<Object>(a => Update());
                }
                return _UpdateProduct;
            }
        }

        public void Update()
        {
            using (var db = new mainEntities())
            {
                PRODUCT pro = db.PRODUCTs.Where(p => (p.ID == ListPoduct.ElementAt(IndexCurrentProduct).ID) && (p.DELETED == 0)).FirstOrDefault();             

                pro.DELETED = 1;
                db.PRODUCTs.Add(Newproduct);
                db.SaveChanges();

                var item = ListPoduct.ElementAt(IndexCurrentProduct);
                if (item != null)
                {
                    item = Newproduct;
                }              
            }
        }

        public ICommand ExitUpdateProduct_Command
        {
            get
            {
                if (_ExitUpdateProduct == null)
                {
                    _ExitUpdateProduct = new RelayingCommand<Object>(a => ExitUpdate(a));
                }
                return _ExitUpdateProduct;
            }
        }


        public void ExitUpdate(Object a)
        {
            //Exit view Update_pro(a)
        }

        public ICommand OpenViewDetailProduct_Command
        {
            get
            {
                if (_OpenViewDetailProduct == null)
                {
                    _OpenViewDetailProduct = new RelayingCommand<PRODUCT>(a => OpenViewDetail());
                }
                return _OpenViewDetailProduct;
            }
        }

        public void OpenViewDetail()
        {
            WindowService.Instance.OpenWindow(this, new ProdDetail());
        }
        public ICommand ExitDetailProduct
        {
            get
            {
                if (_ExitDetailProduct == null)
                {
                    _ExitDetailProduct = new RelayingCommand<Object>(a => ExitDetail(a));
                }
                return _ExitDetailProduct;
            }
        }


        public void ExitDetail(object a)
        {
            //open view Add_pro(a)
        }

        public ICommand AddUpdateImageProductCommand
        {
            get
            {
                if (_AddImageProduct == null)
                {
                    _AddImageProduct = new RelayingCommand<Object>(a => Add_Update_ImageProduct());
                }
                return _AddImageProduct;
            }
        }


        public void Add_Update_ImageProduct()
        {
            string path = "";

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Pictures files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.jpg; *.jpeg; *.jpe; *.jfif; *.png|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                path = openFile.FileName;
                if (Newproduct == null)
                {
                    ListPoduct.ElementAt(IndexCurrentProduct).Image = converImgToByte(path);                
                }   
                else
                {
                    Newproduct.Image = converImgToByte(path);
                }                    
            }                    
        }

         public ICommand AddDetailProToTableCommand
        {
             get
             {
                 if (_AddDetailProToTableCommand == null)
                 {
                    _AddDetailProToTableCommand = new RelayingCommand<Object>(a => AddDetailProToTable());
                 }
                 return _AddDetailProToTableCommand;
             }
         }


         public void AddDetailProToTable()
         {
            Tableviewmodel.TotalCurrentTable += (long) ListPoduct.ElementAt(IndexCurrentProduct).Price;

            if (Tableviewmodel.Currentlistdetailpro != null)
            {
                foreach (var p in Tableviewmodel.Currentlistdetailpro)
                {
                    if (p.Pro.ID == ListPoduct.ElementAt(IndexCurrentProduct).ID)
                    {
                        ++p.Quantity;
                        return;
                    }
                }
            }
            else Tableviewmodel.Currentlistdetailpro = new ObservableCollection<DetailPro>();


            Tableviewmodel.Currentlistdetailpro.Add(new DetailPro(ListPoduct.ElementAt(IndexCurrentProduct)));
         }
        #endregion
        public ICommand OpenViewEditCategory_Command
        {
            get
            {
                if (_OpenViewEditCategory == null)
                {
                    _OpenViewEditCategory = new RelayingCommand<Object>(a => OpenViewEditCategory(a));
                }
                return _OpenViewEditCategory;
            }
        }


        public void OpenViewEditCategory(object a)
        {

            using (var db = new mainEntities())
            {
                if ( Type_In_Edit_CATEGORY == null) return;

                var l = db.PRODUCTs.Where(p => ((p.TYPE_PRODUCT.Type == Type_In_Edit_CATEGORY.Type || p.TYPE_PRODUCT.Type == "") && p.DELETED == 0)).ToList();

                if (l == null) return;

                ListPoduct = new ObservableCollection<PRODUCT>(l);
            }

            WindowService.Instance.OpenWindow(this, new EditType());

        }
        /*public ICommand CancelAddProduct_Command
        {
            get
            {
                if (_CancelAddProduct == null)
                {
                    _CancelAddProduct = new RelayingCommand<Object>(a => CancelAddProduct(a));
                }
                return _CancelAddProduct;
            }
        }


        public void CancelAddProduct(object a)
        {

            Newproduct = null;
            *//* close Addproductview*//*
        }
        public ICommand CancelAddProduct_Command
        {
            get
            {
                if (_CancelAddProduct == null)
                {
                    _CancelAddProduct = new RelayingCommand<Object>(a => CancelAddProduct(a));

                return _CancelAddProduct;
            }
        }


        public void CancelAddProduct(object a)
        {
            //lưu lại trên list type, nếu typ = type thì dùng type
            Newproduct = null;
            *//* close Addproductview*//*
        }*/


        private void LoadProductByType(string Type)
        {
            using (var db = new mainEntities())
            {
                if (Type == null) return;           

                if (Type.Contains("Tất cả"))
                {
                    ListPoduct = new ObservableCollection<PRODUCT>(db.PRODUCTs.Where(p => (p.DELETED == 0)).ToList());
                }
                else
                {
                    var p = db.PRODUCTs.Where(pro => ((pro.TYPE_PRODUCT.Type == Type) && (pro.DELETED == 0)));
                    if (p == null) return;
                    ListPoduct = new ObservableCollection<PRODUCT>(p.ToList());
                }
            }
        }

        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        private byte[] converImgToByte(string path)
        {
            FileStream fs;
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }

     
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }
    }
}
