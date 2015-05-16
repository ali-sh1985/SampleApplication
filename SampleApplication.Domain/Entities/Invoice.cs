using System;
using System.Collections.Generic;

namespace SampleApplication.Domain.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Item> ItemList { get; set; }
        public virtual ICollection<Payment> PaymentList { get; set; }
    }
}