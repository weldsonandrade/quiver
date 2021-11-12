namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitacaoNoNomeDoUsuario : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Nome", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Nome", c => c.String());
        }
    }
}
