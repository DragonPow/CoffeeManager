using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public partial class TABLE
    {
        private string _currentStatus;
        public string CurrentStatus
        {
            get => _currentStatus;
            set
            {
                if (value!=_currentStatus)
                {
                    _currentStatus = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
