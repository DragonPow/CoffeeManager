using MainProject.DatabaseController;
using System;
using System.Windows.Input;

namespace MainProject.Model.Product
{
    class ProductViewModel : BaseViewModel
    {
        #region Flieds
        PRODUCT _prodduct;

        ICommand _OpenViewUpdateProduct;
        ICommand _OpenViewDetailProduct;
        ICommand _UpdateProduct;

        #endregion
        #region Init
        public ProductViewModel(PRODUCT  p)
        {
            Prodduct = p;
        }

        public ProductViewModel()
        {
            Prodduct = new PRODUCT();
        }
        #endregion

        #region Properties
        public PRODUCT Prodduct 
        { 
            get
            {
                if (_prodduct == null)
                {
                    _prodduct = new PRODUCT();
                }
                return _prodduct;
            }
            set
            {
                if ( value != _prodduct)
                {
                    _prodduct = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Icommand

        public ICommand OpenViewUpdateProduct
        {
            get
            {
                if (_OpenViewUpdateProduct == null)
                {
                    _OpenViewUpdateProduct = new RelayingCommand<object>(a => OpenViewUpdate());
                }
                return _OpenViewUpdateProduct;
            }
        }

        public void OpenViewUpdate()
        {
            //open new view to update product
        }

        public ICommand OpenViewDetailProduct
        {
            get
            {
                if (_OpenViewDetailProduct == null)
                {
                    _OpenViewDetailProduct = new RelayingCommand<Object>(a => OpenViewDetail());
                }
                return _OpenViewDetailProduct;
            }
        }

        public void OpenViewDetail()
        {
            //open new View detail product
        }

        public ICommand UpdateProduct
        {
            get 
            {
                if (_UpdateProduct == null)
                {
                    _UpdateProduct = new RelayingCommand<PRODUCT>(a => Update(a));
                }
                return _UpdateProduct;
            }
        }
        
        public void Update(PRODUCT pro)
        {
            this.Prodduct = pro;
            DataController.UpdateProduct(this.Prodduct);
        }
        #endregion

    }
}
