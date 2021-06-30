using MainProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    public interface IContext
    {
        DbSet<DETAILBILL> DETAILBILLs { get; }
        DbSet<PARAMETER> PARAMETERs { get;}
        DbSet<REPORTREVENUE> REPORTREVENUEs { get;}
        DbSet<STATUS_TABLE> STATUS_TABLE { get; }
        DbSet<TYPE_PRODUCT> TYPE_PRODUCT { get; }
        DbSet<TABLE> TABLEs { get; }
        DbSet<DETAILREPORTREVENUE> DETAILREPORTREVENUEs { get; }
        DbSet<DETAILREPORTSALE> DETAILREPORTSALES { get; }
        DbSet<REPORTSALE> REPORTSALES { get; }
        DbSet<BILL> BILLs { get; }
        DbSet<PRODUCT> PRODUCTs { get; }
        DbEntityEntry Entry(object entity);
        int SaveChanges();
    }
}
