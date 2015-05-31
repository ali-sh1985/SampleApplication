using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Service.SearchCriterias
{
    public class InvoiceSearchCriteria
    {
        public string Find { get; set; }
        public int? InvoiceId { get; set; }
        public int? ClientId { get; set; }
        public decimal? TotalFrom { get; set; }
        public decimal? TotalTo { get; set; }
        public decimal? NetFrom { get; set; }
        public decimal? NetTo { get; set; }
    }
}
