using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleApplication.Domain.Enums;

namespace SampleApplication.Web.Models
{
    public class PaymentViewModel
    {
        public int? PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod Method { get; set; }
        public string Description { get; set; }
        public int InvoiceId { get; set; }
    }
}