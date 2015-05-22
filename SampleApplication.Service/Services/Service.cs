using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;

namespace SampleApplication.Service.Services
{
    public interface IService<TEntity>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        TEntity Find(object id);
        List<TEntity> GetAll();
    }

    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected IUnitOfWork _unitOfWork;

        protected Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract void Add(TEntity entity);
        public abstract void Update(TEntity entity);
        public abstract void Remove(TEntity entity);
        public abstract TEntity Find(object id);
        public abstract List<TEntity> GetAll();
    }
}
