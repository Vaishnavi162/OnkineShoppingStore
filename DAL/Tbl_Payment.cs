//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShopingStore.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Payment()
        {
            this.Tbl_Order = new HashSet<Tbl_Order>();
        }
    
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string Status { get; set; }
        public Nullable<int> ProductId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Order> Tbl_Order { get; set; }
        public virtual Tbl_Product Tbl_Product { get; set; }
    }
}
