namespace ServiceDataAccess
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "State");
        }
    }
}
