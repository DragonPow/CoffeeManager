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
                    OnPropertyChanged("Status");
                }
            }
        }
        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "CurrentStatus":
                    //Change status in DB
                    if ((string)after == "Fix" || (string)before == "Fix")
                    {
                        ChangeStatusDB((string)after == "Fix");
                    }
                    break;
                case "STATUS_TABLE":
                    if (after != null)
                    {
                        _currentStatus = ((STATUS_TABLE)after).Status;
                    }
                    break;
            }
        }
        private void ChangeStatusDB(bool isFixStatus)
        {
            using (var db = new mainEntities())
            {
                var status = db.TABLEs.Include("STATUS_TABLE").Where(table => this.ID == table.ID).Select(i => i.STATUS_TABLE).FirstOrDefault();
                status = db.STATUS_TABLE.Where(s => s.Status == (isFixStatus ? "Fix" : "Normal")).FirstOrDefault();
                db.SaveChanges();
            }
        }
    }
}
