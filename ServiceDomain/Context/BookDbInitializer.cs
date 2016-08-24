using ServiceDomain.Migrations;
using ServiceDomain.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace ServiceDomain.Context
{
    internal class BookDbInitializer : MigrateDatabaseToLatestVersion<BooksContext, Configuration>
    {
    }
}