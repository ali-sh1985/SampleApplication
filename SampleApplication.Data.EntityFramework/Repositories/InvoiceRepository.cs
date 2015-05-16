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
    internal class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        internal InvoiceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public List<Invoice> GetInvoiceListByClient(int clientId)
        {
            return Set.Where(i => i.ClientId == clientId).ToList();
        }

        public Task<List<Invoice>> GetInvoiceListByClientAsync(int clientId)
        {
            return Set.Where(i => i.ClientId == clientId).ToListAsync();
        }
    }
}
