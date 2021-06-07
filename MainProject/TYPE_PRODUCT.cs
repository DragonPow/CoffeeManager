namespace MainProject.Model
{
    using System;
    using System.Collections.ObjectModel;

    public partial class PRODUCT : BaseViewModel
    {
        public bool IsChecked
        {
            get
            {   
                return ID_Type == null ? false : true;
            }
            set
            {

            }
        }
    }
}
