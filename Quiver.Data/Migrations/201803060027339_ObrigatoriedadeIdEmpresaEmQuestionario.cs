namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObrigatoriedadeIdEmpresaEmQuestionario : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Questionario", new[] { "IdEmpresa" });
            AlterColumn("dbo.Questionario", "IdEmpresa", c => c.Int(nullable: false));
            CreateIndex("dbo.Questionario", "IdEmpresa");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Questionario", new[] { "IdEmpresa" });
            AlterColumn("dbo.Questionario", "IdEmpresa", c => c.Int());
            CreateIndex("dbo.Questionario", "IdEmpresa");
        }
    }
}
