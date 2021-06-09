
using MainProject.MainWorkSpace.Product;
using MainProject.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;




namespace MainProject.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        #region Field

        private ObservableCollection<PRODUCT> _ListProduct;
        private PRODUCT _Currentproduct;

        private TYPE_PRODUCT _Type;    
        private TYPE_PRODUCT _Type_in_Combobox_AddProduct;
      
        private string _SearchProduct;
        private string _Type_in_Combobox_AddPro;
        private PRODUCT _Newproduct;

        private TableViewModel _Tableviewmodel;

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

        private ICommand _ClickCheckboxSelectedPro;
        /* private ICommand _OpenViewEditCategory;       
         private ICommand _SaveEditCategory;
         private ICommand _DeleteTypeEditCategory;

         private ICommand _CloseEditCategory;*/



        #endregion


        #region Properties

        public ObservableCollection<PRODUCT> ListPoduct { get => _ListProduct; set { if (value != _ListProduct) { _ListProduct = value; OnPropertyChanged(); } } }
        public string SearchProduct { get => _SearchProduct; set { if (_SearchProduct != value) { _SearchProduct = value; OnPropertyChanged(); SearchName(); } } }

        public PRODUCT Currentproduct
        {
            get => _Currentproduct;
            set
            {
                if (_Currentproduct != value)
                {
                    _Currentproduct = value;

                    using (var db = new mainEntities())
                    {
                        if (value != null) _Currentproduct.TYPE_PRODUCT = db.TYPE_PRODUCT.Where(t => t.ID == value.ID_Type).FirstOrDefault(); ;
                    }
                    OnPropertyChanged();
                }
            }
        }

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

        /* public string SearchProduct { get => _SearchProduct; set { if (_SearchProduct != value) { _SearchProduct = value; OnPropertyChanged(); SearchName(); } } }*/
        /*public string EditTypeInEditCatefory { get => _EditTypeInEditCatefory; set { if (_EditTypeInEditCatefory != value) { _EditTypeInEditCatefory = value; TypeInEditCATEGORYCombobox.Type = value; OnPropertyChanged(); } } }*/
        public string Type_in_Combobox_AddPro { get => _Type_in_Combobox_AddPro; set { if (_Type_in_Combobox_AddPro != value) { _Type_in_Combobox_AddPro = value; OnPropertyChanged(); } } }

        public TYPE_PRODUCT Type_in_Combobox_AddProduct { get => _Type_in_Combobox_AddProduct; set { if (_Type_in_Combobox_AddProduct != value) { _Type_in_Combobox_AddProduct = value; OnPropertyChanged(); } } }
        public TYPE_PRODUCT Type { get => _Type; set { if (_Type != value) { _Type = value; OnPropertyChanged(); LoadProductByType(value); } } }
        /* public TYPE_PRODUCT TypeInEditCATEGORYCombobox { get => _TypeInEditCATEGORYCombobox; set { if (_TypeInEditCATEGORYCombobox != value) { _TypeInEditCATEGORYCombobox = value; OnPropertyChanged(); EditTypeInEditCatefory = value.Type; LoadProductBYType_EditType();  } } }*/

        public TableViewModel Tableviewmodel { get => _Tableviewmodel; set { if (_Tableviewmodel != value) { _Tableviewmodel = value; OnPropertyChanged(); } } }
        #endregion

        #region Init

        public ProductViewModel()
        {
            /* Newproduct = new CUSTOMPRODUCT() { product = new PRODUCT() { DELETED = 0, Image = null, TYPE_PRODUCT = new ObservableCollection<TYPE_PRODUCT>() } };*/
            /* lát phải xóa dòng trên đầu nhó*/
            /* Type = new TYPE_PRODUCT() { Type ="Tất cả"} ;*/

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

            Newproduct = new PRODUCT() { Image = imageToByteArray(Properties.Resources.Empty_Image), TYPE_PRODUCT = new TYPE_PRODUCT() };

            WindowService.Instance.OpenWindowWithoutBorderControl(this, new CreateProd());
        }

        public ICommand AddProduct_Command_Command
        {
            get
            {
                if (_AddProduct == null)
                {
                    _AddProduct = new RelayingCommand<bool>(isValid => Add(isValid));
                }
                return _AddProduct;
            }
        }


        public void Add(bool isValid)
        {
            if (!isValid) return;

            using (var db = new mainEntities())
            {
                try
                {
                   
                    if (Newproduct.Name == null)
                    {
                        WindowService.Instance.OpenMessageBox("Vui lòng nhập tên sản phẩm!", "Lỗi", System.Windows.MessageBoxImage.Error);
                        return;
                    }

                    if (Type_in_Combobox_AddProduct == null)
                    {
                        WindowService.Instance.OpenMessageBox("Vui lòng chọn danh mục!", "Lỗi", System.Windows.MessageBoxImage.Error);
                        return;
                    }

                    if (Newproduct.Price == 0)
                    {
                        WindowService.Instance.OpenMessageBox("Vui lòng nhập giá sản phẩm!", "Lỗi", System.Windows.MessageBoxImage.Error);
                        return;
                    }

                    TYPE_PRODUCT type;

                    type = db.TYPE_PRODUCT.Where(t => (t.Type == Type_in_Combobox_AddProduct.Type)).FirstOrDefault();

                    if (type == null) Newproduct.ID_Type = null;
                    else
                        Newproduct.ID_Type = type.ID;

                    Newproduct.TYPE_PRODUCT = null;

                    db.PRODUCTs.Add(Newproduct);

                    ListPoduct.Add(Newproduct);

                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }

            Exitaddproview();
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
            Newproduct = new PRODUCT() { Image = imageToByteArray(Properties.Resources.Empty_Image), TYPE_PRODUCT = new TYPE_PRODUCT() };
        }

        public ICommand ExitAddProductView_Command
        {
            get
            {
                if (_ExitAddProview == null)
                {
                    _ExitAddProview = new RelayingCommand<Object>(a => Exitaddproview());
                }
                return _ExitAddProview;
            }
        }


        public void Exitaddproview()
        {
            Newproduct = null;
            Window window = WindowService.Instance.FindWindowbyTag("Create product").First();
            window.Close();
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
            if (Currentproduct == null) return;

            using (var db = new mainEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    PRODUCT product = db.PRODUCTs.Where(p => (p.ID == Currentproduct.ID)).FirstOrDefault();


                    if (product == null) return;

                    try
                    {
                        var list = db.DETAILREPORTSALES.Where(r => r.ID_Product == product.ID);
                        if (list != null)
                        {
                            db.DETAILREPORTSALES.RemoveRange(list);
                        }

                        db.PRODUCTs.Remove(product);

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exp)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                    }

                }
            }

            foreach (var p in Tableviewmodel.Currentlistdetailpro)
            {
                if (p.Pro == Currentproduct)
                {
                    Tableviewmodel.Currentlistdetailpro.Remove(p);
                    return;
                }
            }

            ListPoduct.Remove(Currentproduct);           
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
            using (var db = new mainEntities())
            {

                if (SearchProduct == "")
                {
                    LoadProductByType(Type);
                    /* ListPoduct = new ObservableCollection<PRODUCT>(db.PRODUCTs.ToList());   */
                    return;
                }

                string s = ConvertToUnSign(SearchProduct).ToLower();
                var listpro = db.PRODUCTs.ToList();

                if (listpro == null)
                {
                    ListPoduct = new ObservableCollection<PRODUCT>();
                    return;
                }

                ListPoduct = new ObservableCollection<PRODUCT>();

                foreach (var p in listpro)
                {
                    if (ConvertToUnSign(p.Name).ToLower().Contains(s)) ListPoduct.Add(p);
                }

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
                var listpro = db.PRODUCTs.Where(p => (p.TYPE_PRODUCT.Type == Type.Type));
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

            WindowService.Instance.OpenWindowWithoutBorderControl(this, new EditProd());
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
                var pro = db.PRODUCTs.Where(p => (p.ID == Currentproduct.ID)).FirstOrDefault();

                pro.Image = Currentproduct.Image;
                pro.Name = Currentproduct.Name;
                pro.Price = Currentproduct.Price;
                pro.Decription = Currentproduct.Decription;
                if (Type_in_Combobox_AddProduct != null) pro.ID_Type = Type_in_Combobox_AddProduct.ID;


                db.SaveChanges();
            }



            /* var item = Currentproduct;
             if (item != null)
             {
                 item = Newproduct;
             }*/
            LoadProductByType(Type);
            ExitUpdate();
        }

        public ICommand ExitUpdateProduct_Command
        {
            get
            {
                if (_ExitUpdateProduct == null)
                {
                    _ExitUpdateProduct = new RelayingCommand<Object>(a => ExitUpdate());
                }
                return _ExitUpdateProduct;
            }
        }


        public void ExitUpdate()
        {
            var window = WindowService.Instance.FindWindowbyTag("EditPro").First();
            window.Close();
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
            WindowService.Instance.OpenWindowWithoutBorderControl(this, new ProdDetail());
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
            var window = WindowService.Instance.FindWindowbyTag("DetailPro").First();
            window.Close();
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
            /*     Newproduct = new PRODUCT() { Image = imageToByteArray(Properties.Resources.Empty_Image), TYPE_PRODUCT = new TYPE_PRODUCT() };*/

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
                    Currentproduct.Image = converImgToByte(path);
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
            if (!ListPoduct.Contains(Currentproduct)) return;

            Tableviewmodel.TotalCurrentTable += (long)Currentproduct.Price;

            if (Tableviewmodel.Currentlistdetailpro != null)
            {
                foreach (var p in Tableviewmodel.Currentlistdetailpro)
                {
                    if (p.Pro.ID == Currentproduct.ID)
                    {
                        ++p.Quantity;
                        return;
                    }
                }
            }
            else Tableviewmodel.Currentlistdetailpro = new ObservableCollection<DetailPro>();


            Tableviewmodel.Currentlistdetailpro.Add(new DetailPro(Currentproduct));
        }
        #endregion
        /*public ICommand OpenViewEditCategory_Command
        {
            get
            {
                if (_OpenViewEditCategory == null)
                {
                    _OpenViewEditCategory = new RelayingCommand<Object>(a => OpenViewEditCategory());
                }
                return _OpenViewEditCategory;
            }
        }


        public void OpenViewEditCategory()
        {

            TypeInEditCATEGORYCombobox = Type;
            
            WindowService.Instance.OpenWindowWithoutBorderControl(this, new EditType());

            LoadProductByType(Type.Type);

        }*/

        /* public ICommand ClickCheckboxSelectedPro_Command
         {
             get
             {
                 if (_ClickCheckboxSelectedPro == null)
                 {
                     _ClickCheckboxSelectedPro = new RelayingCommand<Object>(a => ClickCheckboxSelectedPro());
                 }
                 return _ClickCheckboxSelectedPro;
             }
         }


        public void ClickCheckboxSelectedPro()
         {
             if (Currentproduct.ID_Type== null)
             {
                 Currentproduct.ID_Type =  TypeInEditCATEGORYCombobox.ID;

             }                 
             else
             {
                 Currentproduct.ID_Type = null ;

             }
         }
          public ICommand SaveEditCategory_Command
        {
            get
            {
               if (_SaveEditCategory == null)
               {
                   _SaveEditCategory = new RelayingCommand<Object>(a => SaveEditCategory(a));
               }
               return _SaveEditCategory;

            }
        }


       public void SaveEditCategory(object a)
        {
           if (EditTypeInEditCatefory =="")
           {
               WindowService.Instance.OpenMessageBox("Vui lòng nhập tên danh mục!", "Lỗi", System.Windows.MessageBoxImage.Error);
               return;
           }    
           using (var db = new mainEntities())
           {
               var type = db.TYPE_PRODUCT.Where(t => t.ID== TypeInEditCATEGORYCombobox.ID).FirstOrDefault();


               var list = db.PRODUCTs.Where(p => (p.ID_Type == type.ID || p.ID_Type == null)).ToList();
               if (list == null) return;

               int i = 0; 
               foreach ( var p in list)
               {
                   p.ID_Type = ListPoduct.ElementAt(i).ID_Type;
                   ++i;
               }

               db.SaveChanges();
           }

       }

       public ICommand CloseEditCategory_Command
       {
           get
           {
               if (_CloseEditCategory == null)
               {
                   _CloseEditCategory = new RelayingCommand<Object>(a => CloseEditCategory());
               }
               return _CloseEditCategory;
           }
       }


       public void CloseEditCategory()
       {
           Window window = WindowService.Instance.FindWindowbyTag("Edit category").First();
           window.Close();
           LoadProductByType(Type.Type);
       }
 */


        public void LoadProductByType(TYPE_PRODUCT Type)
        {
            using (var db = new mainEntities())
            {

                if (Type == null || Type.Type.Contains("Tất cả"))
                {
                    ListPoduct = new ObservableCollection<PRODUCT>(db.PRODUCTs.ToList());
                }
                else
                {
                    var p = db.PRODUCTs.Where(pro => (pro.TYPE_PRODUCT.Type == Type.Type));
                    if (p.ToList().Count == 0) ListPoduct = new ObservableCollection<PRODUCT>();
                    else ListPoduct = new ObservableCollection<PRODUCT>(p.ToList());
                }
            }
        }



        /*public ICommand DeleteTypeEditCategory_Command
        {
            get
            {
                if (_DeleteTypeEditCategory == null)
                {
                    _DeleteTypeEditCategory = new RelayingCommand<Object>(a => DeleteTypeEditCategory());
                }
                return _DeleteTypeEditCategory;
            }
        }


        public void DeleteTypeEditCategory()
        {
            if (TypeInEditCATEGORYCombobox == null) return;

            using (var db = new mainEntities())
            {
                var list = db.PRODUCTs.Where(p => (p.ID_Type== TypeInEditCATEGORYCombobox.ID )).ToList();
                if (list == null) return;

                foreach( var p in list)
                {
                    p.TYPE_PRODUCT = null;
                }

                db.TYPE_PRODUCT.Attach(TypeInEditCATEGORYCombobox);
                db.TYPE_PRODUCT.Remove(TypeInEditCATEGORYCombobox);

                db.SaveChanges();
            }

            TypeInEditCATEGORYCombobox = null;


          }*/
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

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
