using System;
using System.Data.Entity;
using ServiceDomain.Models;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ServiceDomain.Context
{
    public interface IBooksDbContext : IDisposable
    {
        IDbSet<Author> Authors { get; set; }
        IDbSet<Book> Books { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entry);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    public class BooksContext : DbContext, IBooksDbContext
    {
        public IDbSet<Author> Authors { get; set; }
        public IDbSet<Book> Books { get; set; }

        static BooksContext()
        {
            Database.SetInitializer(new BookDbInitializer());
        }

        public BooksContext() : base("Name=BooksDbConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public BooksContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
 
        }
    }
}