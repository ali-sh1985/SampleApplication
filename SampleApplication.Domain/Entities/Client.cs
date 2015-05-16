using System.Collections.Generic;

namespace SampleApplication.Domain.Entities
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public virtual ICollection<Invoice> InvoiceList { get; set; }
    }
}
