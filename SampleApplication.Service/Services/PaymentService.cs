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

    public interface IPaymentService : IService<Payment>
    {
        void Remove(int paymentId);
        List<Payment> GetPaymentListByClient(int clientId);
        PagedList<Payment> Search(PaymentSearchCriteria searchCriteria, Paging paging, Sort sorting);
    }
    public class PaymentService : Service<Payment>, IPaymentService
    {
        public PaymentService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public override void Add(Payment entity)
        {
            _unitOfWork.PaymentRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(Payment entity)
        {
            _unitOfWork.PaymentRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(Payment entity)
        {
            _unitOfWork.PaymentRepository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override Payment Find(object id)
        {
            return _unitOfWork.PaymentRepository.FindById(id);
        }

        public override List<Payment> GetAll()
        {
            return _unitOfWork.PaymentRepository.GetAll();
        }

        public void Remove(int paymentId)
        {
            _unitOfWork.PaymentRepository.Remove(paymentId);
            _unitOfWork.SaveChanges();
        }

        public List<Payment> GetPaymentListByClient(int clientId)
        {
            return _unitOfWork.PaymentRepository.GetPaymentListByClient(clientId);
        }

        public PagedList<Payment> Search(PaymentSearchCriteria searchCriteria, Paging paging, Sort sorting)
        {
            var predicate = PredicateBuilder.True<Payment>();
            if (searchCriteria.ClientId.HasValue)
            {
                predicate = predicate.And(x => x.Invoice.Client.ClientId == searchCriteria.ClientId.Value);
            }
            if (sorting == null)
            {
                sorting = new Sort("PaymentId");
            }
            if (String.IsNullOrWhiteSpace(sorting.ColumnName))
            {
                sorting = new Sort("PaymentId", sorting.ColumnOrder == Order.Ascending ? "ASC" : "DESC");
            }

            var orderBy = sorting.GetOrderBy<Payment>();

            var result = _unitOfWork.PaymentRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageSize);
            return new PagedList<Payment>(result, paging.PageIndex, paging.PageSize, _unitOfWork.PaymentRepository.GetAll(predicate, null, 0, int.MaxValue).Count);
        }
    }

    
}
