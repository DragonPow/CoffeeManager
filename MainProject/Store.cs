using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    public class Store
    {
        #region Field
        static string _name;
        static string _phone;
        static string _address;
        static string _nameInnkeeper;
        #endregion

        #region Property

        public static string StoreName
        {
            get
            {
                using (var db = new mainEntities())
                {
                    var name = db.PARAMETERs.Where(p => p.NAME == "StoreName").FirstOrDefault();
                    if (name != null)
                        return name.Value;
                }
                return null;
            }
        }
        public static string StorePhone
        {
            get
            {
                using (var db = new mainEntities())
                {
                    var name = db.PARAMETERs.Where(p => p.NAME == "StorePhone").FirstOrDefault();
                    if (name != null)
                        return name.Value;
                }
                return null;
            }
        }

        public static string StoreAddress
        {
            get
            {
                using (var db = new mainEntities())
                {
                    var name = db.PARAMETERs.Where(p => p.NAME == "StoreAddress").FirstOrDefault();
                    if (name != null) return name.Value;
                }
                return null;
            }
        }
        public static string NameInnkeeper { get => _nameInnkeeper; set => _nameInnkeeper = value; }

        #endregion
    }
}
