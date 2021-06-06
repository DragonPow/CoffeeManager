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
                if (minDate.Year == maxDate.Year && minDate.Month == maxDate.Month)
                {
                    var data = db.REPORTREVENUEs.Where(rp => rp.Year == minDate.Year && rp.Month == minDate.Month).FirstOrDefault();
                    if (data != null)
                    {
                        var rs = new List<StatisticModel>();
                        foreach (var detail in data.DETAILREPORTREVENUEs)
                        {
                            var model = new StatisticModel()
                            {
                                TimeMin = new DateTime(minDate.Year, minDate.Month, (int)detail.Day, 0, 0, 0),
                                TimeMax = new DateTime(minDate.Year, minDate.Month, (int)detail.Day, 23, 59, 59),
                                Revenue = detail.Revenue,
                                Amount = 0
                            };
                            rs.Add(model);
                        }
                        return rs;
                    }
                }
            }

            // If this is first-time statistc. Starting calculate
            using (mainEntities db = new mainEntities())
            {
                var data = db.BILLs.Where(b => b.CheckoutDay >= minDate && b.CheckoutDay <= maxDate)
                    .Select(b => new
                    {
                        b.CheckoutDay,
                        b.DETAILBILLs
                    });

                Dictionary<DateTime, StatisticModel> dictionary = new Dictionary<DateTime, StatisticModel>();

                foreach (var group in data)
                {
                    DateTime date = new DateTime(group.CheckoutDay.Year, group.CheckoutDay.Month, group.CheckoutDay.Day, 0, 0, 0);
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
                    model.Amount += 1;

                    foreach (var detail in group.DETAILBILLs)
                    {
                        model.Revenue += detail.Quantity * detail.UnitPrice;
                    }
                }

                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Background
                , new Action<Dictionary<DateTime, StatisticModel>, int, int>(addReportRevenue)
                , dictionary, minDate.Year, minDate.Month);


                return dictionary.Values.ToList();
            } 
        }

        void addReportRevenue(Dictionary<DateTime, StatisticModel> dictionary, int year, int month)
        {
            using (mainEntities db = new mainEntities())
            {
                var tran = db.Database.BeginTransaction();
                try
                {
                    var report = new REPORTREVENUE()
                    {
                        Year = year,
                        Month = month
                    };

                    db.REPORTREVENUEs.Add(report);

                    foreach (var model in dictionary.Values)
                    {
                        var detail = new DETAILREPORTREVENUE()
                        {
                            Day = model.TimeMin.Day,
                            Revenue = model.Revenue
                        };
                        report.DETAILREPORTREVENUEs.Add(detail);
                    }
                    db.SaveChanges();
                    tran.Commit();
                    Console.WriteLine("SUCCESS: Saved report to database");
                }
                catch (Exception e)
                {
                    Console.WriteLine("FAILED: Cannot saved report to database");
                    Console.WriteLine(e.Message);
                    tran.Rollback();
                }
                dictionary.Clear();
            }
        }

        public List<StatisticModel> statisticByWeek(DateTime minDate, DateTime maxDate, string productName)
        {
            return new List<StatisticModel>();
            /*using (mainEntities db = new mainEntities())
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
            }*/
        }

        public List<StatisticModel> statisticByMonth(DateTime minDate, DateTime maxDate, string productName)
        {
            return new List<StatisticModel>();
           /* using (mainEntities db = new mainEntities())
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
            }*/
        }

        public List<string> getProductNames()
        {
            using (mainEntities db = new mainEntities())
            {
                List<string> rs = new List<string>();
                foreach (var pd in db.PRODUCTs)
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
                var data = db.PRODUCTs.Where(pd => pd.Name == name).FirstOrDefault();
                return data;
            }
        }

        public void createTemplateData()
        {
            DateTime start = DateTime.Now;
            DateTime minDate = new DateTime(2021, 4, 1, 6, 0, 0);
            DateTime maxDate = new DateTime(2021, 6, 3, 23, 59, 59);
            TimeSpan step = new TimeSpan(1, 0, 0, 0, 0);

            var productIDs = new long[] { 1, 2, 3, 4, 5};
            var prices = new long[] { 10000, 20000, 15000, 32000, 14000 };

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
                            Quantity = random.Next(4) + 1,
                            UnitPrice = prices[temp]
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
