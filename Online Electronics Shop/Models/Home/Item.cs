using Online_Electronics_Shop.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Electronics_Shop.Models.Home
{
    public class Item
    {
        public Tbl_Product Product { get; set; }

        public int Quantity { get; set; }

    }
}