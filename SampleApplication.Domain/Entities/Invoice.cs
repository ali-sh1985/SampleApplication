using System;
using System.Collections.Generic;
using System.Linq;

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

        public decimal Total
        {
            get
            {
                decimal total = 0;
                if (ItemList != null) total = ItemList.Sum(t => t.Net - t.Tax);
                return total;
            }
        }

        public decimal Net
        {
            get
            {
                decimal net = 0;
                if (ItemList != null) net = ItemList.Sum(t => t.Net);
                return net;
            }
        }

        public decimal Tax
        {
            get
            {
                decimal tax = 0;
                if (ItemList != null) tax = ItemList.Sum(t => t.Tax);
                return tax;
            }
        }

        public string Description
        {
            get
            {
                string description = string.Empty;
                if (ItemList != null)
                {
                    description = ItemList.Aggregate(description, (current, item) => item.Description + "," + current);
                }
                    
                return description.TrimEnd(',');
            }
        }
    }
}