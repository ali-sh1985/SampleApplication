using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleApplication.Web.Models
{
    public class ClientViewModel
    {
    }

    public class ClientFilterViewModel
    {
        public string Find { set; get; }
        public decimal? InvoiceFrom { set; get; }
        public decimal? InvoiceTo { set; get; }
        public decimal? BalanceFrom { set; get; }
        public decimal? BalanceTo { set; get; }
        public decimal? PaymentFrom { set; get; }
        public decimal? PaymentTo { set; get; }
        public string Currency { set; get; }
    }
}