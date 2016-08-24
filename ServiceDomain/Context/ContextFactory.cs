namespace ServiceDomain.Context
{
    public interface IDbContextFactory
    {
        IBooksDbContext CreateContext();
    }

    public class DbContextFactory : IDbContextFactory
    {
        public IBooksDbContext CreateContext()
        {
            return new BooksContext();
        }
    }
}