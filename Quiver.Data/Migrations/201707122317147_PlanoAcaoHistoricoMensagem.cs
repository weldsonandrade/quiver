namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanoAcaoHistoricoMensagem : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.RespostaItem", "Resposta_Id", "dbo.Resposta");
            //DropForeignKey("dbo.RespostaItem", "Item_Id", "dbo.Item");
            DropForeignKey("dbo.Resposta", "IdPlanoAcao", "dbo.PlanoAcao");
            DropIndex("dbo.Resposta", new[] { "IdPlanoAcao" });
            //DropIndex("dbo.RespostaItem", new[] { "Resposta_Id" });
            //DropIndex("dbo.RespostaItem", new[] { "Item_Id" });
            //CreateTable(
            //    "dbo.RespostaItem",
            //    c => new
            //        {
            //            IdResposta = c.Int(nullable: false),
            //            IdItem = c.Int(nullable: false),
            //            IdPlanoAcao = c.Int(),
            //        })
            //    .PrimaryKey(t => new { t.IdResposta, t.IdItem })
            //    .ForeignKey("dbo.Item", t => t.IdItem)
            //    .ForeignKey("dbo.PlanoAcao", t => t.IdPlanoAcao)
            //    .ForeignKey("dbo.Resposta", t => t.IdResposta)
            //    .Index(t => t.IdResposta)
            //    .Index(t => t.IdItem)
            //    .Index(t => t.IdPlanoAcao);
            
            CreateTable(
                "dbo.Historico",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataModificacao = c.DateTime(nullable: false),
                        NomeCampo = c.Int(nullable: false),
                        ValorAntigo = c.String(nullable: false, maxLength: 500),
                        ValorNovo = c.String(nullable: false, maxLength: 500),
                        Descricao = c.String(maxLength: 500),
                        IdPlanoAcao = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        IdUsuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlanoAcao", t => t.IdPlanoAcao)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdPlanoAcao)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Mensagem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Texto = c.String(nullable: false, maxLength: 500),
                        Data = c.DateTime(nullable: false),
                        IdPlanoAcao = c.Int(nullable: false),
                        IdUsuario = c.String(maxLength: 128),
                        EmailResponsavel = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlanoAcao", t => t.IdPlanoAcao)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdPlanoAcao)
                .Index(t => t.IdUsuario);
            
            AddColumn("dbo.PlanoAcao", "OQue", c => c.String(maxLength: 500));
            AddColumn("dbo.PlanoAcao", "Porque", c => c.String(maxLength: 500));
            AddColumn("dbo.PlanoAcao", "Quem", c => c.String(maxLength: 100));
            AddColumn("dbo.PlanoAcao", "Como", c => c.String(maxLength: 500));
            AddColumn("dbo.PlanoAcao", "Quanto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PlanoAcao", "Quando", c => c.DateTime());
            AddColumn("dbo.PlanoAcao", "Onde", c => c.String(maxLength: 200));
            AddColumn("dbo.PlanoAcao", "Atrasado", c => c.Boolean(nullable: false));
            AddColumn("dbo.PlanoAcao", "DataConclusao", c => c.DateTime());
            AddColumn("dbo.PlanoAcao", "Justificativa", c => c.String(maxLength: 500));
            AddColumn("dbo.PlanoAcao", "Situacao", c => c.Int(nullable: false));
            DropColumn("dbo.Resposta", "IdPlanoAcao");
            DropColumn("dbo.PlanoAcao", "Tarefa");
            DropColumn("dbo.PlanoAcao", "Data");
            //DropTable("dbo.RespostaItem");

            // Adicionado ao Script   
            AddColumn("dbo.RespostaItem", "IdPlanoAcao", c => c.Int());
            AddForeignKey("dbo.RespostaItem", "IdPlanoAcao", "dbo.PlanoAcao", "Id");
            CreateIndex("dbo.RespostaItem", "IdPlanoAcao");
        }

        public override void Down()
        {
            //CreateTable(
            //    "dbo.RespostaItem",
            //    c => new
            //        {
            //            Resposta_Id = c.Int(nullable: false),
            //            Item_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Resposta_Id, t.Item_Id });
            
            AddColumn("dbo.PlanoAcao", "Data", c => c.DateTime());
            AddColumn("dbo.PlanoAcao", "Tarefa", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Resposta", "IdPlanoAcao", c => c.Int());
            //DropForeignKey("dbo.RespostaItem", "IdResposta", "dbo.Resposta");
            //DropForeignKey("dbo.RespostaItem", "IdPlanoAcao", "dbo.PlanoAcao");
            DropForeignKey("dbo.Mensagem", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mensagem", "IdPlanoAcao", "dbo.PlanoAcao");
            DropForeignKey("dbo.Historico", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Historico", "IdPlanoAcao", "dbo.PlanoAcao");
            //DropForeignKey("dbo.RespostaItem", "IdItem", "dbo.Item");
            DropIndex("dbo.Mensagem", new[] { "IdUsuario" });
            DropIndex("dbo.Mensagem", new[] { "IdPlanoAcao" });
            DropIndex("dbo.Historico", new[] { "IdUsuario" });
            DropIndex("dbo.Historico", new[] { "IdPlanoAcao" });
            //DropIndex("dbo.RespostaItem", new[] { "IdPlanoAcao" });
            //DropIndex("dbo.RespostaItem", new[] { "IdItem" });
            //DropIndex("dbo.RespostaItem", new[] { "IdResposta" });
            DropColumn("dbo.PlanoAcao", "Situacao");
            DropColumn("dbo.PlanoAcao", "Justificativa");
            DropColumn("dbo.PlanoAcao", "DataConclusao");
            DropColumn("dbo.PlanoAcao", "Atrasado");
            DropColumn("dbo.PlanoAcao", "Onde");
            DropColumn("dbo.PlanoAcao", "Quando");
            DropColumn("dbo.PlanoAcao", "Quanto");
            DropColumn("dbo.PlanoAcao", "Como");
            DropColumn("dbo.PlanoAcao", "Quem");
            DropColumn("dbo.PlanoAcao", "Porque");
            DropColumn("dbo.PlanoAcao", "OQue");
            DropTable("dbo.Mensagem");
            DropTable("dbo.Historico");
            //DropTable("dbo.RespostaItem");
            //CreateIndex("dbo.RespostaItem", "Item_Id");
            //CreateIndex("dbo.RespostaItem", "Resposta_Id");
            CreateIndex("dbo.Resposta", "IdPlanoAcao");
            AddForeignKey("dbo.Resposta", "IdPlanoAcao", "dbo.PlanoAcao", "Id");
            //AddForeignKey("dbo.RespostaItem", "Item_Id", "dbo.Item", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.RespostaItem", "Resposta_Id", "dbo.Resposta", "Id", cascadeDelete: true);

            //Adicionado ao Script
            DropForeignKey("dbo.RespostaItem", "IdPlanoAcao", "dbo.PlanoAcao");
            DropIndex("dbo.RespostaItem", new[] { "IdPlanoAcao" });
            DropColumn("dbo.RespostaItem", "IdPlanoAcao");
        }
    }
}
