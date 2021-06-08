using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    public partial class STATUS_TABLE
    {
        public ObservableCollection<string> ListStatus { get; private set; }
        public static void loadListStatus()
        {
            using (var db = new mainEntities())
            {
                Instance.ListStatus = new ObservableCollection<string>(db.STATUS_TABLE.Select(i => i.Status).ToList());
                Instance.ListStatus.Add("Already");
            }
        }
        private static STATUS_TABLE _instance;
        public static STATUS_TABLE Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new STATUS_TABLE();
                }
                return _instance;
            }
        }
    }
}
