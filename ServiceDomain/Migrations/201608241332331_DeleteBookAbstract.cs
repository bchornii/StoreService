namespace ServiceDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteBookAbstract : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "Abstract");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Abstract", c => c.String());
        }
    }
}
