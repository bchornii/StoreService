namespace ServiceDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorZip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Zip", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "Zip");
        }
    }
}
