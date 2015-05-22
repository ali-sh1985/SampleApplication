using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Web.Models
{
    public class InvoiceViewModel
    {
        public int? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public List<ItemViewModel> ItemList { get; set; }
    }
}