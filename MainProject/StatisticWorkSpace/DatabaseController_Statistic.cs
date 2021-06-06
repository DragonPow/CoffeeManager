using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    class DatabaseController_Statistic
    {
        public List<StatisticModel> statisticByDay(DateTime minDate, DateTime maxDate, string productName)
        {
            using (mainEntities db = new mainEntities())
            {
                var data = db.BILLs.Where(b => b.CheckoutDay >= minDate && b.CheckoutDay <= maxDate)
                    .Join(db.DETAILBILLs, b => b.ID, dt => dt.ID_Bill,
                    (b, dt) => new
                    {
                        PD_ID = dt.ID_Product,
                        Date = b.CheckoutDay,
                        b.VOUCHER,
                        dt.Amount
                    }).Join(productName == null
                            ?db.PRODUCTs
                            :db.PRODUCTs.Where(pd => (pd.DELETED == 0) && pd.Name == productName)
                            , r => r.PD_ID, pd => pd.ID,
                    (r, pd) => new
                    {
                        pd.Name,
                        Revenue = pd.Price * r.Amount,
                        r.Amount,
                        r.VOUCHER,
                        r.Date
                    });
                Dictionary<DateTime, StatisticModel> dictionary = new Dictionary<DateTime, StatisticModel>();

                foreach (var group in data)
                {
                    float voucher = 1f;
                    if (group.VOUCHER != null) { voucher = group.VOUCHER.Percent / 100f; }
                    DateTime date = new DateTime(group.Date.Value.Year, group.Date.Value.Month, group.Date.Value.Day, 0, 0, 0);
                    StatisticModel model;
                    if (!dictionary.ContainsKey(date))
                    {
                        model = new StatisticModel
                        {
                            TimeMin = date,
                            TimeMax = date.AddDays(1).AddSeconds(-1),
                            Revenue = 0,
                            Amount = 0
                        };
                        dictionary.Add(date, model);
                    }

                    model = dictionary[date];
                    model.Revenue += (int)(group.Revenue * voucher);
                    model.Amount += group.Amount;
                }

                return dictionary.Values.ToList();
            }
        }

        public List<StatisticModel> statisticByWeek(DateTime minDate, DateTime maxDate, string productName)
        {
            using (mainEntities db = new mainEntities())
            {
                var data = db.BILLs.Where(b => b.CheckoutDay >= minDate && b.CheckoutDay <= maxDate)
                    .Join(db.DETAILBILLs, b => b.ID, dt => dt.ID_Bill,
                    (b, dt) => new
                    {
                        PD_ID = dt.ID_Product,
                        Date = b.CheckoutDay,
                        b.VOUCHER,
                        dt.Amount
                    }).Join(productName == null
                            ? db.PRODUCTs
                            : db.PRODUCTs.Where(pd => (pd.DELETED == 0) && pd.Name == productName)
                            , r => r.PD_ID, pd => pd.ID,
                    (r, pd) => new
                    {
                        pd.Name,
                        Revenue = pd.Price * r.Amount,
                        r.Amount,
                        r.VOUCHER,
                        r.Date
                    });
                Dictionary<DateTime, StatisticModel> dictionary = new Dictionary<DateTime, StatisticModel>();
                foreach (var group in data)
                {
                    float voucher = 1f;
                    if (group.VOUCHER != null) { voucher = group.VOUCHER.Percent / 100f; }
                    DateTime date = new DateTime(group.Date.Value.Year, group.Date.Value.Month, group.Date.Value.Day, 0, 0, 0);
                    while (date.DayOfWeek != DayOfWeek.Monday) { date = date.AddDays(-1); }
                    StatisticModel model;
                    if (!dictionary.ContainsKey(date))
                    {
                        model = new StatisticModel
                        {
                            TimeMin = date,
                            TimeMax = date.AddDays(7).AddSeconds(-1),
                            Revenue = group.Revenue,
                            Amount = group.Amount
                        };
                        if (model.TimeMax > maxDate) { model.TimeMax = maxDate; }
                        else if (model.TimeMin < minDate) { model.TimeMin = minDate; }
                        dictionary.Add(date, model);
                    }
                    model = dictionary[date];
                    model.Revenue += (int)(group.Revenue * voucher);
                    model.Amount += group.Amount;

                }
                return dictionary.Values.ToList();
            }
        }

        public List<StatisticModel> statisticByMonth(DateTime minDate, DateTime maxDate, string productName)
        {
            using (mainEntities db = new mainEntities())
            {
                var data = db.BILLs.Where(b => b.CheckoutDay >= minDate && b.CheckoutDay <= maxDate)
                    .Join(db.DETAILBILLs, b => b.ID, dt => dt.ID_Bill,
                    (b, dt) => new
                    {
                        PD_ID = dt.ID_Product,
                        Date = b.CheckoutDay,
                        b.VOUCHER,
                        dt.Amount
                    }).Join(productName == null
                            ? db.PRODUCTs
                            : db.PRODUCTs.Where(pd => (pd.DELETED == 0) && pd.Name == productName)
                            , r => r.PD_ID, pd => pd.ID,
                    (r, pd) => new
                    {
                        pd.Name,
                        Revenue = pd.Price * r.Amount,
                        r.Amount,
                        r.VOUCHER,
                        r.Date
                    });
                Dictionary<DateTime, StatisticModel> dictionary = new Dictionary<DateTime, StatisticModel>();
                foreach (var group in data)
                {
                    float voucher = 1f;
                    if (group.VOUCHER != null) { voucher = group.VOUCHER.Percent / 100f; }
                    DateTime date = new DateTime(group.Date.Value.Year, group.Date.Value.Month, 1, 0, 0, 0);
                    StatisticModel model;
                    if (!dictionary.ContainsKey(date))
                    {
                        model = new StatisticModel
                        {
                            TimeMin = date,
                            TimeMax = date.AddMonths(1).AddSeconds(-1),
                            Revenue = group.Revenue,
                            Amount = group.Amount
                        };
                        dictionary.Add(date, model);
                    }
                    model = dictionary[date];
                    model.Revenue += (int)(group.Revenue * voucher);
                    model.Amount += group.Amount;
                }
                return dictionary.Values.ToList();
            }
        }

        public List<string> getProductNames()
        {
            using (mainEntities db = new mainEntities())
            {
                List<string> rs = new List<string>();
                var data = db.PRODUCTs.Where(pd => pd.DELETED == 0);
                foreach (var pd in data)
                {
                    rs.Add(pd.Name);
                }
                return rs;
            }
        }

        public PRODUCT getProductID(string name)
        {
            using (mainEntities db = new mainEntities())
            {
                var data = db.PRODUCTs.Where(pd => (pd.DELETED == 0) && pd.Name == name).FirstOrDefault();
                return data;
            }
        }

        public void createTemplateData()
        {
            DateTime start = DateTime.Now;
            DateTime minDate = new DateTime(2021, 1, 1, 0, 0, 0);
            DateTime maxDate = new DateTime(2021, 6, 3, 23, 59, 59);
            TimeSpan step = new TimeSpan(6, 0, 0, 0, 0);

            var productIDs = new long[] { 1, 2, 3, 4, 5 };

            Random random = new Random();
            using (mainEntities db = new mainEntities())
            {
                var transaction = db.Database.BeginTransaction();
                for (DateTime date = minDate; date < maxDate; date += step)
                {
                    var bill = new BILL
                    {
                        CheckoutDay = date,
                        TotalPrice = 0
                    };

                    var listPDs = new List<long>(productIDs);
                    int count = random.Next(3) + 1;
                    for (int i = 0; i < count; i++)
                    {
                        int temp = random.Next(listPDs.Count);
                        var dt = new DETAILBILL
                        {
                            ID_Product = listPDs[temp],
                            Amount = random.Next(4) + 1
                        };
                        bill.DETAILBILLs.Add(dt);
                        listPDs.RemoveAt(temp);
                    }
                    db.BILLs.Add(bill);
                }
                try
                {
                    Console.WriteLine("Begin save to database");
                    db.SaveChanges();
                    Console.WriteLine("Begin commit");
                    transaction.Commit();
                    Console.WriteLine("Add template data success");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    
                    Console.WriteLine("FAILED: Data has been rollback");
                    Console.WriteLine(e.Message);
                }
            }
            TimeSpan timeSpan = DateTime.Now - start;
            Console.WriteLine("TIMER: Done task in {0}s", timeSpan.ToString());
        }
    }
}
