using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public partial class TABLE
    {
        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            OnPropertyChanged(propertyName);
            if (propertyName == "Status")
                if ((string)after == "Fix" || (string)before == "Fix")
                {
                    //ChangeStatusDB((string)after == "Fix");
                }
        }
        private void ChangeStatusDB(bool isFixStatus)
        {
            using (var db = new mainEntities())
            {
                string status = db.STATUS_TABLE.Include("STATUS_TABLE").Where(table => this.ID == table.ID).Select(i => i.Status).FirstOrDefault();
                if (isFixStatus) status = "Fix";
                else status = "Normal";
                db.SaveChanges();
            }
        }
    }
}
