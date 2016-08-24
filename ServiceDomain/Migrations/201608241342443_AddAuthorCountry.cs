namespace ServiceDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorCountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "Country");
        }
    }
}
