using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Domain.Repositories
{
    public interface IClientRepository: IRepository<Client>
    {
        void Remove(int clientId);
        decimal GetBalance(int clientId);
        decimal GetTotalInvoices(int clientId);
        decimal GetTotalPayments(int clientId);
    }
}
