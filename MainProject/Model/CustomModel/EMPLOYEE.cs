using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public partial class EMPLOYEE
    {
        public void Clone(EMPLOYEE value)
        {
            this.BILLs = value.BILLs;
            this.Birthday = value.Birthday;
            this.DELETED = value.DELETED;
            this.ID = value.ID;
            this.ID_Position = value.ID_Position;
            this.Name = value.Name;
            this.Password = value.Password;
            this.Phone = value.Phone;
            this.POSITION_EMPLOYEE = value.POSITION_EMPLOYEE;
        }
    }
}
