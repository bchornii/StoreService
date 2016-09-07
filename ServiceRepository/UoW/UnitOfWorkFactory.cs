using ServiceDataAccess;

namespace ServiceRepository
{
    public interface IUnitOfWorkFactory
    {
        IStoreServiceUnitOfWork CreateEfUnitOfWork();
    }
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IStoreServiceUnitOfWork CreateEfUnitOfWork()
        {
            return new StoreServiceUnitOfWork(new StoreServiceContext());
        }
    }
}
