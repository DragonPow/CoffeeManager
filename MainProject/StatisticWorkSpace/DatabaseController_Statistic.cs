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

                                Revenue = (long)detail.Revenue,
                                Amount = (int)detail.AmountBill
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
                        b.DETAILBILLs, 
                        b.TotalPrice

                    });

                Dictionary<DateTime, StatisticModel> dictionary = new Dictionary<DateTime, StatisticModel>();

                foreach (var bill in data)
                {

                    DateTime date = new DateTime(bill.CheckoutDay.Year, bill.CheckoutDay.Month, bill.CheckoutDay.Day, 0, 0, 0);
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
                    else { model = dictionary[date]; }

                    model.Amount += 1;

                    model.Revenue += bill.TotalPrice;

                    /*foreach (var detail in group.DETAILBILLs)
                    {
                        model.Revenue += detail.Quantity * detail.UnitPrice;
                    }*/

                }

                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background
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
                            Revenue = model.Revenue,
                            AmountBill = model.Amount
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

        public List<StatisticModel> statisticByName(DateTime minDate, DateTime maxDate)
        {
            using (mainEntities db = new mainEntities())
            {
                if (minDate.Year == maxDate.Year && minDate.Month == maxDate.Month)
                {
                    var data = db.REPORTSALES.Where(rp => rp.Year == minDate.Year && rp.Month == minDate.Month).FirstOrDefault();
                    if (data != null)
                    {
                        var rs = new List<StatisticModel>();
                        long total = 0;
                        foreach (var detail in data.DETAILREPORTSALES.Select(dt => new
                        {
                            ProductName = dt.PRODUCT.Name,
                            dt.Revenue,
                            dt.Amount,
                            dt.Rate
                        }))
                        {
                            var model = new StatisticModel()
                            {
                                TimeMin = minDate,
                                TimeMax = maxDate,
                                Title = detail.ProductName,
                                Revenue = detail.Revenue,
                                Amount = (int)detail.Amount,
                                Label = detail.Rate.ToString()
                            };
                            rs.Add(model);
                            total += model.Revenue;
                        }

                        foreach (StatisticModel model in rs)
                        {
                            model.Label = String.Format("{0:P}", model.Revenue * 1f / total);
                        }
                        return rs;
                    }
                }
            }

            // If this is first-time statistc. Starting calculate
            using (mainEntities db = new mainEntities())
            {
                var data = db.BILLs.Where(b => b.CheckoutDay >= minDate && b.CheckoutDay <= maxDate);

                Dictionary<String, StatisticModel> dictionary = new Dictionary<String, StatisticModel>();
                Dictionary<String, long> productIDs = new Dictionary<String, long>();

                foreach (var group in data)
                {
                    StatisticModel model;

                    foreach (var detail in group.DETAILBILLs.Select(dt => new
                    {
                        ProductID = dt.PRODUCT.ID,
                        ProductName = dt.PRODUCT.Name,
                        dt.Quantity,
                        dt.UnitPrice
                    }))
                    {
                        String name = detail.ProductName;
                        if (!dictionary.ContainsKey(name))
                        {
                            model = new StatisticModel
                            {
                                TimeMin = minDate,
                                TimeMax = maxDate,
                                Title = name,
                                Revenue = 0,
                                Amount = 0
                            };
                            dictionary.Add(name, model);
                            productIDs.Add(name, detail.ProductID);
                        }
                        else { model = dictionary[name]; }

                        model.Amount += (int)detail.Quantity;
                        model.Revenue += detail.Quantity * detail.UnitPrice;
                    }
                }
                long totalRevenue = 0;
                foreach (var model in dictionary.Values) { totalRevenue += model.Revenue; }
                foreach (var model in dictionary.Values) { model.Label = (model.Revenue * 100 / totalRevenue).ToString(); }

                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background
                , new Action<Dictionary<string, StatisticModel>, Dictionary<string, long>, int, int>(addReportSale)
                , dictionary, productIDs, minDate.Year, minDate.Month);


                return dictionary.Values.ToList();
            }
        }

        void addReportSale(Dictionary<string, StatisticModel> dictionary, Dictionary<string, long> productIDs, int year, int month)
        {
            using (mainEntities db = new mainEntities())
            {
                var tran = db.Database.BeginTransaction();
                try
                {
                    var report = new REPORTSALE()
                    {
                        Year = year,
                        Month = month
                    };

                    db.REPORTSALES.Add(report);

                    foreach(var model in dictionary.Values)
                    {
                        var detail = new DETAILREPORTSALE()
                        {
                            ID_Product = productIDs[model.Title],
                            Amount = model.Amount,
                            Revenue = model.Revenue,
                            Rate = long.Parse(model.Label)
                        };
                        report.DETAILREPORTSALES.Add(detail);
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
            DateTime minDate = new DateTime(2019, 1, 1, 6, 0, 0);
            DateTime maxDate = new DateTime(2021, 6, 30, 23, 59, 59);
            TimeSpan step = new TimeSpan(1, 0, 0, 0, 0);

            List<BILL> listBill = new List<BILL>();
            Random random = new Random();
            using (mainEntities db = new mainEntities())
            {
                var products = db.PRODUCTs.Select(pd => new
                {
                    pd.ID,
                    pd.Name,
                    pd.Price
                });
                var tables = db.TABLEs.Select(tb => new
                {
                    tb.ID
                }).ToArray();
                var transaction = db.Database.BeginTransaction();
                for (DateTime date = minDate; date < maxDate; date += step)
                {
                    int countBill = random.Next(30) + 1;
                    List<BILL> listBillInDay = new List<BILL>(countBill);
                    for (int k = 0; k < countBill; k++)
                    {
                        long total = 0;
                        var bill = new BILL
                        {
                            CheckoutDay = date.AddMinutes(random.Next(1000) + 240),
                            ID_Table = tables[random.Next(tables.Length)].ID
                        };

                        var listPDs = new List<Tuple<long, String, long>>();
                        foreach (var pd in products) { listPDs.Add(new Tuple<long, string, long>(pd.ID, pd.Name, (long)pd.Price)); }
                        int count = random.Next(5) + 1;
                        for (int i = 0; i < count; i++)
                        {
                            int temp = random.Next(listPDs.Count);
                            var dt = new DETAILBILL
                            {
                                ID_Product = listPDs[temp].Item1,
                                Quantity = random.Next(4) + 1,
                                UnitPrice = listPDs[temp].Item3
                            };
                            total += dt.Quantity * dt.UnitPrice;
                            bill.DETAILBILLs.Add(dt);
                            bill.MoneyCustomer = total + random.Next(5) * 10000;
                            listPDs.RemoveAt(temp);
                        }
                        bill.TotalPrice = total;
                        listBillInDay.Add(bill);
                    }
                    listBillInDay.Sort((b1, b2) => b1.CheckoutDay.CompareTo(b2.CheckoutDay));
                    listBill.AddRange(listBillInDay);
                }

                db.BILLs.AddRange(listBill);

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
