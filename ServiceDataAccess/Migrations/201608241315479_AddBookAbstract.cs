namespace ServiceDataAccess
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookAbstract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Abstract", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Abstract");
        }
    }
}
