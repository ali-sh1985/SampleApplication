using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Service.SearchCriterias
{
    public class ClientSearchCriteria
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
