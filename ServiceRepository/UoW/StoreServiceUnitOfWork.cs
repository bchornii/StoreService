using ServiceDataAccess;
using System;
using System.Threading.Tasks;

namespace ServiceRepository
{
    public interface IStoreServiceUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }        
        Task<int> Complete();
    }
    public class StoreServiceUnitOfWork : IStoreServiceUnitOfWork
    {        
        private readonly IStoreServiceContext _context;

        public StoreServiceUnitOfWork(IStoreServiceContext context)
        {
            _context = context;
            Books = new BookRepository(_context);
        }
        public IBookRepository Books { get; private set; }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
