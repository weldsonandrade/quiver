namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColunasExcluidosGruposQuestionarios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grupo", "Excluido", c => c.Boolean(nullable: false));
            AddColumn("dbo.Unidade", "Excluido", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Unidade", "Excluido");
            DropColumn("dbo.Grupo", "Excluido");
        }
    }
}
