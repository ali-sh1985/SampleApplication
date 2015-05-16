using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Domain.Repositories
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        List<Payment> GetPaymetListByClient(int clientId);
        Task<List<Payment>> GetPaymetListByClientAsync(int clientId);
        List<Payment> GetPaymetListByInvoice(int invoiceId);
        Task<List<Payment>> GetPaymetListByInvoiceAsync(int invoiceId);
    }
}
