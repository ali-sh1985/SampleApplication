using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Domain.Repositories
{
    public interface IInvoiceRepository:IRepository<Invoice>
    {
        void Remove(int invoiceId);
        List<Invoice> GetInvoiceListByClient(int clientId);
        Task<List<Invoice>> GetInvoiceListByClientAsync(int clientId);
    }
}
