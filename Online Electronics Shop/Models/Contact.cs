using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Online_Electronics_Shop.Models
{
    public class Contact
    {
            public int con_id { get; set; }
        [Required]

       // public Nullable<int> MemberId { get; set; }
       // [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}