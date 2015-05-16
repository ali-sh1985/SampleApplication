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

        public List<Payment> GetPaymetListByClient(int clientId)
        {
            return Set.Where(p => p.Invoice.ClientId == clientId).ToList();
        }

        public Task<List<Payment>> GetPaymetListByClientAsync(int clientId)
        {
            return Set.Where(p => p.Invoice.ClientId == clientId).ToListAsync();
        }

        public List<Payment> GetPaymetListByInvoice(int invoiceId)
        {
            return Set.Where(p => p.Invoice.InvoiceId == invoiceId).ToList();
        }

        public Task<List<Payment>> GetPaymetListByInvoiceAsync(int invoiceId)
        {
            return Set.Where(p => p.Invoice.InvoiceId == invoiceId).ToListAsync();
        }
    }
}
