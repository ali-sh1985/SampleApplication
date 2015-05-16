using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Domain.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public decimal Net { get; set; }
        public int Tax { get; set; }
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
