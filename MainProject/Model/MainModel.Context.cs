﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MainProject.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mainEntities : DbContext
    {
        public mainEntities()
            : base("name=mainEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<DETAILBILL> DETAILBILLs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<POSITION_EMPLOYEE> POSITION_EMPLOYEE { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<REGULATION> REGULATIONs { get; set; }
        public virtual DbSet<STATUS_TABLE> STATUS_TABLE { get; set; }
        public virtual DbSet<TABLE> TABLES { get; set; }
        public virtual DbSet<TYPE_PRODUCT> TYPE_PRODUCT { get; set; }
        public virtual DbSet<VOUCHER> VOUCHERs { get; set; }
    }
}
