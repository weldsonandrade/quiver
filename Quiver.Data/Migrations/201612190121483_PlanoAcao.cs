namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanoAcao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlanoAcao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarefa = c.String(nullable: false, maxLength: 200),
                        Responsavel = c.String(maxLength: 50),
                        Data = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Resposta", "IdPlanoAcao", c => c.Int());
            CreateIndex("dbo.Resposta", "IdPlanoAcao");
            AddForeignKey("dbo.Resposta", "IdPlanoAcao", "dbo.PlanoAcao", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resposta", "IdPlanoAcao", "dbo.PlanoAcao");
            DropIndex("dbo.Resposta", new[] { "IdPlanoAcao" });
            DropColumn("dbo.Resposta", "IdPlanoAcao");
            DropTable("dbo.PlanoAcao");
        }
    }
}
