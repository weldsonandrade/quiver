namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorClassificacaoAdicionada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classificacao", "Cor", c => c.String(nullable: false, maxLength: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classificacao", "Cor");
        }
    }
}
