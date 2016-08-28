using ServiceDomain.Migrations;
using System.Data.Entity;

namespace ServiceDomain.Context
{
    internal class BookDbInitializer : MigrateDatabaseToLatestVersion<BooksContext, Configuration>
    {
    }
}