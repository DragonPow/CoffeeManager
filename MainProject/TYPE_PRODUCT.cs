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
                return TYPE_PRODUCT == null ? false : true;
            }
        }
    }
}
