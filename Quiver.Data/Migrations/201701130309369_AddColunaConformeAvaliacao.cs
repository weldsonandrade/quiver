namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColunaConformeAvaliacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Avaliacao", "Conforme", c => c.Boolean(nullable: true, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Avaliacao", "Conforme");
        }
    }
}
