using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;

namespace SampleApplication.Data.EntityFramework.Repositories
{
    internal class ClientRepository : Repository<Client>, IClientRepository
    {
        internal ClientRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void Remove(int clientId)
        {
            var client = Set.Find(clientId);
            Remove(client);
        }

        public decimal GetBalance(int clientId)
        {
            return GetTotalInvoices(clientId) - GetTotalPayments(clientId);
        }

        public decimal GetTotalInvoices(int clientId)
        {
            return Set.Find(clientId).InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax));
        }

        public decimal GetTotalPayments(int clientId)
        {
            return Set.Find(clientId).InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total));
        }

    }
}
