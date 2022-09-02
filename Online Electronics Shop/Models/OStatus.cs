using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Electronics_Shop.Models
{
    public class OStatus
    {
       
        //public int d_id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Bill { get; set; }




    }
}