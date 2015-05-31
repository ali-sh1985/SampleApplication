using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleApplication.Web.Models
{
    public class BalanceSheetViewModel
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int Reference { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
    }
}