using System.Data.Entity;

namespace ServiceDataAccess
{
    internal class StoreServiceDbInitializer : MigrateDatabaseToLatestVersion<StoreServiceContext, Configuration>
    {
    }
}