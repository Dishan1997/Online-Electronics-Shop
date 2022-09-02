using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Electronics_Shop.Models
{
    public class Order
    {
       
            public int o_id { get; set; }
            public Nullable<int> o_fk_pro { get; set; }
            public Nullable<int> o_fk_invoice { get; set; }
            public Nullable<System.DateTime> o_date { get; set; }
            public Nullable<int> o_qty { get; set; }
            public Nullable<double> o_bill { get; set; }
            public Nullable<double> o_unitprice { get; set; }
        

    }

    public class ShippingDetails
    {
        public int ShippingDetailId { get; set; }
        [Required]

        public Nullable<int> MemberId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }
        public Nullable<int> OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }

        public Nullable<decimal> AmountPaid { get; set; }
        [Required]
        public string PaymentType { get; set; }
    }

}