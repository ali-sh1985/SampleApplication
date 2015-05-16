using System;
using SampleApplication.Domain.Enums;

namespace SampleApplication.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}