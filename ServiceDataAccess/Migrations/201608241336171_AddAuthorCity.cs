namespace ServiceDataAccess
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "City");
        }
    }
}
