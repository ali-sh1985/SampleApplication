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
        List<Payment> Search(PaymentSearchCriteria searchCriteria, Paging paging, Sort sorting);
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

        public List<Payment> Search(PaymentSearchCriteria searchCriteria, Paging paging, Sort sorting)
        {
            var predicate = PredicateBuilder.True<Payment>();
            var orderBy = sorting == null ? null : sorting.GetOrderBy<Payment>();
            return _unitOfWork.PaymentRepository.GetAll(predicate, orderBy, paging.PageSize * paging.PageIndex, paging.PageSize);
        }
    }

    
}
