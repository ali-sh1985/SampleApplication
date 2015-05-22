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
        List<Invoice> GetInvoiceListByClient(int clientId);
        List<Invoice> Search(InvoiceSearchCriteria searchCriteria, Paging paging, Sort sorting);
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

        public List<Invoice> GetInvoiceListByClient(int clientId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceListByClient(clientId);
        }

        public List<Invoice> Search(InvoiceSearchCriteria searchCriteria, Paging paging, Sort sorting)
        {
            var predicate = PredicateBuilder.True<Invoice>();
            var orderBy = sorting == null ? null : sorting.GetOrderBy<Invoice>();
            return _unitOfWork.InvoiceRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageSize);
        }
    }
}
