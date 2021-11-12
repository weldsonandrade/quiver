namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelNotificacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notificacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUsuarioNotificado = c.String(nullable: false, maxLength: 128),
                        IdAvaliacao = c.Int(nullable: false),
                        Lida = c.Boolean(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Tipo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Avaliacao", t => t.IdAvaliacao)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuarioNotificado)
                .Index(t => t.IdUsuarioNotificado)
                .Index(t => t.IdAvaliacao);
            
            AddColumn("dbo.Avaliacao", "IdRecorrencia", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notificacao", "IdUsuarioNotificado", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notificacao", "IdAvaliacao", "dbo.Avaliacao");
            DropIndex("dbo.Notificacao", new[] { "IdAvaliacao" });
            DropIndex("dbo.Notificacao", new[] { "IdUsuarioNotificado" });
            DropColumn("dbo.Avaliacao", "IdRecorrencia");
            DropTable("dbo.Notificacao");
        }
    }
}
