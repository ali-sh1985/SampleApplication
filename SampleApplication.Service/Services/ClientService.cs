using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.Common;
using SampleApplication.Service.Helpers;
using SampleApplication.Service.SearchCriterias;

namespace SampleApplication.Service.Services
{
    public interface IClientService : IService<Client>
    {
        void Remove(int clientId);
        decimal GetBalance(int clientId);
        decimal GetTotalInvoices(int clientId);
        decimal GetTotalPayments(int clientId);
        List<Client> Search(ClientSearchCriteria searchCriteria, Paging paging, Sort sorting);
    }
    public class ClientService : Service<Client>, IClientService
    {
        public ClientService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public override void Add(Client client)
        {
            _unitOfWork.ClientRepository.Add(client);
            _unitOfWork.SaveChanges();
        }

        public override void Update(Client client)
        {
            _unitOfWork.ClientRepository.Update(client);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(Client client)
        {
            _unitOfWork.ClientRepository.Remove(client);
            _unitOfWork.SaveChanges();
        }

        public override Client Find(object id)
        {
            return _unitOfWork.ClientRepository.FindById(id);
        }

        public override List<Client> GetAll()
        {
            return _unitOfWork.ClientRepository.GetAll();
        }

        public List<Client> Search(ClientSearchCriteria searchCriteria, Paging paging, Sort sorting)
        {
            var predicate = PredicateBuilder.True<Client>();

            if (!string.IsNullOrWhiteSpace(searchCriteria.Find))
            {
                predicate = predicate.And(x => x.Name.Contains(searchCriteria.Find));
            }

            if (searchCriteria.BalanceFrom.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax)) - x.InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total)) >= searchCriteria.BalanceFrom);
            }

            if (searchCriteria.BalanceTo.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax)) - x.InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total)) <= searchCriteria.BalanceTo);
            }

            if (searchCriteria.InvoiceFrom.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax)) >= searchCriteria.InvoiceFrom);
            }

            if (searchCriteria.InvoiceTo.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.ItemList.Sum(t => t.Net - t.Tax)) <= searchCriteria.InvoiceTo);
            }

            if (searchCriteria.PaymentFrom.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total)) >= searchCriteria.PaymentFrom);
            }

            if (searchCriteria.PaymentFrom.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceList.Sum(i => i.PaymentList.Sum(p => p.Total)) <= searchCriteria.PaymentTo);
            }

            
            if (sorting == null)
            {
                sorting = new Sort("ClientId");
            }
            if (String.IsNullOrWhiteSpace(sorting.ColumnName))
            {
                sorting = new Sort("ClientId", sorting.ColumnOrder == Order.Ascending ? "ASC" : "DESC");
            }

            var orderBy = sorting.GetOrderBy<Client>();

            return _unitOfWork.ClientRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageSize);
        }

        public void Remove(int clientId)
        {
            _unitOfWork.ClientRepository.Remove(clientId);
            _unitOfWork.SaveChanges();
        }

        public decimal GetBalance(int clientId)
        {
            return _unitOfWork.ClientRepository.GetBalance(clientId);
        }

        public decimal GetTotalInvoices(int clientId)
        {
            return _unitOfWork.ClientRepository.GetTotalInvoices(clientId);
        }

        public decimal GetTotalPayments(int clientId)
        {
            return _unitOfWork.ClientRepository.GetTotalPayments(clientId);
        }
    }
}
