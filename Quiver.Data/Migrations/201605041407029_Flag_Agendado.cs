namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Flag_Agendado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Avaliacao", "Agendada", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Avaliacao", "Agendada");
        }
    }
}
