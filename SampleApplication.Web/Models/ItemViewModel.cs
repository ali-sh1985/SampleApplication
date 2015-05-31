using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleApplication.Web.Models
{
    public class ItemViewModel
    {
        public int? ItemId { get; set; }
        public string Description { get; set; }
        public decimal Net { get; set; }
        public int Tax { get; set; }
        public int InvoiceId { get; set; }
        public bool IsDeleted { get; set; }
    }
}