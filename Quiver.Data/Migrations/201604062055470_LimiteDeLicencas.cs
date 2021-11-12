namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimiteDeLicencas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresa", "LimiteLicencas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresa", "LimiteLicencas");
        }
    }
}
