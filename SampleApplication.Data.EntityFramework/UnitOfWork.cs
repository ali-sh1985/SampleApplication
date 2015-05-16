using SampleApplication.Data.EntityFramework.Repositories;
using SampleApplication.Domain;
using SampleApplication.Domain.Repositories;
using System.Threading.Tasks;

namespace SampleApplication.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private IExternalLoginRepository _externalLoginRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IClientRepository _clientRepository;
        private IInvoiceRepository _invoiceRepository;
        private IItemRepository _itemRepository;
        private IPaymentRepository _paymentRepository;
       
        #endregion

        #region Constructors
        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new ApplicationDbContext(nameOrConnectionString);
        }
        #endregion

        #region IUnitOfWork Members
        public IExternalLoginRepository ExternalLoginRepository
        {
            get { return _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(_context)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public IClientRepository ClientRepository
        {
            get { return _clientRepository ?? (_clientRepository = new ClientRepository(_context)); }
        }

        public IInvoiceRepository InvoiceRepository
        {
            get { return _invoiceRepository ?? (_invoiceRepository = new InvoiceRepository(_context)); }
        }

        public IItemRepository ItemRepository
        {
            get { return _itemRepository ?? (_itemRepository = new ItemRepository(_context)); }
        }

        public IPaymentRepository PaymentRepository
        {
            get { return _paymentRepository ?? (_paymentRepository = new PaymentRepository(_context)); }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _externalLoginRepository = null;
            _roleRepository = null;
            _userRepository = null;
            _context.Dispose();
        }
        #endregion
    }
}