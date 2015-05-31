using System;
using System.Collections.Generic;
using System.Linq;
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
    public interface IInvoiceService : IService<Invoice>
    {
        List<Invoice> GetOutstandingInvocies(DateTime fromDate,DateTime toDate);
        void Remove(int invoiceId);
        List<Invoice> GetInvoiceListByClient(int clientId);
        PagedList<Invoice> Search(InvoiceSearchCriteria searchCriteria, Paging paging, Sort sorting);
    }
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        public InvoiceService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public override void Add(Invoice entity)
        {
            _unitOfWork.InvoiceRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(Invoice entity)
        {
            _unitOfWork.InvoiceRepository.Update(entity);

            var deletedItems = entity.ItemList.Where(t => t.IsDeleted).ToArray();
            foreach (Item t in deletedItems)
            {
                _unitOfWork.ItemRepository.Remove(t);
            }

            foreach (var item in entity.ItemList.Where(t=>!t.IsDeleted))
            {
                if (item.ItemId > 0)
                {
                    _unitOfWork.ItemRepository.Update(item);
                }
                else
                {
                    _unitOfWork.ItemRepository.Add(item);
                }
            }
             
            _unitOfWork.SaveChanges();
        }

        public override void Remove(Invoice entity)
        {
            _unitOfWork.InvoiceRepository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override Invoice Find(object id)
        {
            return _unitOfWork.InvoiceRepository.FindById(id);
        }

        public override List<Invoice> GetAll()
        {
            return _unitOfWork.InvoiceRepository.GetAll();
        }

        public List<Invoice> GetOutstandingInvocies(DateTime fromDate, DateTime toDate)
        {
            return _unitOfWork.InvoiceRepository.GetAll(x => x.Date >= fromDate 
                && x.Date <= toDate 
                && x.ItemList.Any() 
                && (!x.PaymentList.Any() 
                || x.PaymentList.Sum(p => p.Total) <= x.ItemList.Sum(t => t.Net - t.Tax)), null, 0, int.MaxValue);
        }

        public void Remove(int invoiceId)
        {
            _unitOfWork.InvoiceRepository.Remove(invoiceId);
            _unitOfWork.SaveChanges();
        }

        public List<Invoice> GetInvoiceListByClient(int clientId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceListByClient(clientId);
        }

        public PagedList<Invoice> Search(InvoiceSearchCriteria searchCriteria, Paging paging, Sort sorting)
        {
            var predicate = PredicateBuilder.True<Invoice>();
            if (!string.IsNullOrWhiteSpace(searchCriteria.Find))
            {
                predicate = predicate.And(x => x.Client.Name.Contains(searchCriteria.Find));
            }
            if (searchCriteria.InvoiceId.HasValue)
            {
                predicate = predicate.And(x => x.InvoiceId == searchCriteria.InvoiceId);
            }
            if (searchCriteria.ClientId.HasValue)
            {
                predicate = predicate.And(x => x.Client.ClientId == searchCriteria.ClientId);
            }
            if (searchCriteria.NetFrom.HasValue)
            {
                predicate = predicate.And(x => x.ItemList.Sum(t => t.Net) >= searchCriteria.NetFrom);
            }
            if (searchCriteria.NetTo.HasValue)
            {
                predicate = predicate.And(x => x.ItemList.Sum(t => t.Net) <= searchCriteria.NetTo);
            }
            if (searchCriteria.TotalFrom.HasValue)
            {
                predicate = predicate.And(x => x.ItemList.Sum(t => t.Net - t.Tax) >= searchCriteria.TotalFrom);
            }
            if (searchCriteria.TotalTo.HasValue)
            {
                predicate = predicate.And(x => x.ItemList.Sum(t => t.Net - t.Tax) <= searchCriteria.TotalTo);
            }

            if (sorting == null)
            {
                sorting = new Sort("InvoiceId");
            }
            if (String.IsNullOrWhiteSpace(sorting.ColumnName))
            {
                sorting = new Sort("InvoiceId", sorting.ColumnOrder == Order.Ascending ? "ASC" : "DESC");
            }

            var orderBy = sorting.GetOrderBy<Invoice>();
            var result = _unitOfWork.InvoiceRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageSize);
            return new PagedList<Invoice>(result, paging.PageIndex, paging.PageSize, _unitOfWork.InvoiceRepository.GetAll(predicate,null,0,int.MaxValue).Count);
        }
    }
}
