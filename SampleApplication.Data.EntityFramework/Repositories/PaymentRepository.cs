using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;

namespace SampleApplication.Data.EntityFramework.Repositories
{
    internal class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        internal PaymentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void Remove(int paymentId)
        {
            var client = Set.Find(paymentId);
            Remove(client);
        }

        public List<Payment> GetPaymentListByClient(int clientId)
        {
            return Set.Where(p => p.Invoice.ClientId == clientId).ToList();
        }

        public Task<List<Payment>> GetPaymentListByClientAsync(int clientId)
        {
            return Set.Where(p => p.Invoice.ClientId == clientId).ToListAsync();
        }

        public List<Payment> GetPaymentListByInvoice(int invoiceId)
        {
            return Set.Where(p => p.Invoice.InvoiceId == invoiceId).ToList();
        }

        public Task<List<Payment>> GetPaymentListByInvoiceAsync(int invoiceId)
        {
            return Set.Where(p => p.Invoice.InvoiceId == invoiceId).ToListAsync();
        }
    }
}
