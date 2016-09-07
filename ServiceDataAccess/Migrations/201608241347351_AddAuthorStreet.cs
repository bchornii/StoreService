namespace ServiceDataAccess
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorStreet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Street", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "Street");
        }
    }
}
