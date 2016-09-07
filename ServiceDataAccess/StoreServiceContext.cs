using DataAccess.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace ServiceDataAccess
{
    public interface IStoreServiceContext : IDisposable
    {        
        IDbSet<Book> Books { get; set; }
        IDbSet<Order> Orders { get; set; }
        IDbSet<Author> Authors { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
    public class StoreServiceContext : DbContext, IStoreServiceContext
    {
        public IDbSet<Book> Books { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<Author> Authors { get; set; }

        static StoreServiceContext()
        {
            Database.SetInitializer(new StoreServiceDbInitializer());
        }

        public StoreServiceContext() : base("Name=BooksDbConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public StoreServiceContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
