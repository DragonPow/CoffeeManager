//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DETAILBILL
    {
        public long ID_PRODUCT { get; set; }
        public long ID_BILL { get; set; }
        public Nullable<int> AMOUNT { get; set; }
    
        public virtual BILL BILL { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
