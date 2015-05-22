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
        void Remove(int paymentId);
        List<Payment> GetPaymentListByClient(int clientId);
        Task<List<Payment>> GetPaymentListByClientAsync(int clientId);
        List<Payment> GetPaymentListByInvoice(int invoiceId);
        Task<List<Payment>> GetPaymentListByInvoiceAsync(int invoiceId);
    }
}
