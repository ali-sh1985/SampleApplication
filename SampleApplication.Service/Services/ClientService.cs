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
    public class ClientService
    {
        private IClientRepository _clientRepository;
        public ClientService(IUnitOfWork unitOfWork)
        {
            _clientRepository = unitOfWork.ClientRepository;
        }

        public List<Client> SelectClient(ClientSearchCriteria searchCriteria, Paging paging, Sort sorting = null)
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

            var orderBy = sorting == null? null: sorting.GetOrderBy<Client>();

            return _clientRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageIndex);
        }
    }
}
