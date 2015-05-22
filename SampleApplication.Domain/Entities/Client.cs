using System.Collections.Generic;
using System.Linq;
using SampleApplication.Domain.Enums;

namespace SampleApplication.Domain.Entities
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Town { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public Currency DefaultCurrency { get; set; }
        public virtual ICollection<Invoice> InvoiceList { get; set; }

        public decimal TotalInvoiced
        {
            get
            {
                decimal totalInvoiced = 0;
                if (InvoiceList != null) totalInvoiced = InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax));
                return totalInvoiced;
            }
        }
        public decimal TotalPaid
        {
            get
            {
                decimal totalpaid = 0;
                if (InvoiceList != null) totalpaid = InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total));
                return totalpaid;
            }
        }
        public decimal Balance
        {
            get
            {
                decimal totalpaid = 0;
                if (InvoiceList != null) totalpaid = InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax)) - InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total));
                return totalpaid;
            }
        }
    }
}
