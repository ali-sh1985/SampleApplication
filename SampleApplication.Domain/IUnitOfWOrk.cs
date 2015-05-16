using System;
using System.Threading;
using System.Threading.Tasks;
using SampleApplication.Domain.Repositories;

namespace SampleApplication.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IClientRepository ClientRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IItemRepository ItemRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        #endregion

        #region Methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}
