using Online_Electronics_Shop.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Electronics_Shop.Models
{
    public class ViewModel
    {
        public tbl_invoice ivc { get; set; }
        public tbl_order ods { get; set; }
        public Tbl_ShippingDetails shd { get; set; }
        public SiteUser su { get; set; }
        public tbl_delivery dv { get; set; }
        public Tbl_Product tp { get; set; }
        public Tbl_Contact conc { get; set; }


    }
}