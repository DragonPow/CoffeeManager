using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public partial class BILL
    {
        public bool isDateGreaterEqualthan(DateTime time)
        {
            return this.CheckoutDay.Date >= time.Date;
        }
        public bool isDateSmallerEqualthan(DateTime time)
        {
            return this.CheckoutDay.Date <= time.Date;
        }
    }
}
