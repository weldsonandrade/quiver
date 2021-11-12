namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRelationshiptQuestionarioGrupoToManyToMany : DbMigration
    {
        public override void Up()
        {            
            DropForeignKey("dbo.AvaliacaoQuestionario", "IdQuestionario", "dbo.Questionario");
            RenameTable(name: "dbo.AvaliacaoQuestionario", newName: "AvaliacaoQuestionarioGrupo");
            DropForeignKey("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" }, "dbo.AvaliacaoQuestionario");
            DropIndex("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdQuestionario" });
            DropIndex("dbo.Questionario", new[] { "IdGrupo" });
            DropIndex("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" });
            DropPrimaryKey("dbo.AvaliacaoQuestionarioGrupo");
            CreateTable(
                "dbo.QuestionarioGrupo",
                c => new
                    {
                        IdQuestionario = c.Int(nullable: false),
                        IdGrupo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdQuestionario, t.IdGrupo })
                .ForeignKey("dbo.Questionario", t => t.IdQuestionario)
                .ForeignKey("dbo.Grupo", t => t.IdGrupo)
                .Index(t => t.IdQuestionario)
                .Index(t => t.IdGrupo);

            Sql("INSERT INTO dbo.QuestionarioGrupo (IdQuestionario, IdGrupo) SELECT Id, IdGrupo FROM dbo.Questionario");
            
            AddColumn("dbo.Resposta", "IdGrupo", c => c.Int(nullable: false));
            AddColumn("dbo.AvaliacaoQuestionarioGrupo", "IdGrupo", c => c.Int(nullable: false));

            Sql("UPDATE dbo.AvaliacaoQuestionarioGrupo SET IdGrupo = q.IdGrupo FROM dbo.AvaliacaoQuestionarioGrupo aqg " +
                "JOIN dbo.Questionario q ON q.Id = aqg.IdQuestionario");
            Sql("UPDATE dbo.Resposta SET IdGrupo = aqg.IdGrupo " +
                "FROM dbo.Resposta r JOIN dbo.AvaliacaoQuestionarioGrupo aqg ON aqg.IdAvaliacao = r.IdAvaliacao AND aqg.IdQuestionario = r.IdQuestionario");

            Sql("INSERT INTO dbo.Configuracao (Nome,Valor) VALUES('LastMobileVersion', '24')");

            AddPrimaryKey("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" });
            CreateIndex("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdQuestionario", "IdGrupo" });
            CreateIndex("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" });
            AddForeignKey("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdQuestionario", "IdGrupo" }, "dbo.QuestionarioGrupo", new[] { "IdQuestionario", "IdGrupo" });
            AddForeignKey("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" }, "dbo.AvaliacaoQuestionarioGrupo", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" });
            DropForeignKey("dbo.Questionario", "IdGrupo", "dbo.Grupo");
            DropColumn("dbo.Questionario", "IdGrupo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questionario", "IdGrupo", c => c.Int(nullable: false));
            AddForeignKey("dbo.Questionario", "IdGrupo" , "dbo.Grupo", "Id" );
            DropForeignKey("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" }, "dbo.AvaliacaoQuestionarioGrupo");
            DropForeignKey("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdQuestionario", "IdGrupo" }, "dbo.QuestionarioGrupo");
            DropForeignKey("dbo.QuestionarioGrupo", "IdQuestionario", "dbo.Questionario");
            DropIndex("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario", "IdGrupo" });
            DropIndex("dbo.QuestionarioGrupo", new[] { "IdGrupo" });
            DropIndex("dbo.QuestionarioGrupo", new[] { "IdQuestionario" });
            DropIndex("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdQuestionario", "IdGrupo" });
            DropPrimaryKey("dbo.AvaliacaoQuestionarioGrupo");
            DropColumn("dbo.AvaliacaoQuestionarioGrupo", "IdGrupo");
            DropColumn("dbo.Resposta", "IdGrupo");
            DropTable("dbo.QuestionarioGrupo");
            AddPrimaryKey("dbo.AvaliacaoQuestionarioGrupo", new[] { "IdAvaliacao", "IdQuestionario" });
            CreateIndex("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" });
            CreateIndex("dbo.Questionario", "IdGrupo");
            CreateIndex("dbo.AvaliacaoQuestionarioGrupo", "IdQuestionario");
            AddForeignKey("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" }, "dbo.AvaliacaoQuestionario", new[] { "IdAvaliacao", "IdQuestionario" });
            RenameTable(name: "dbo.AvaliacaoQuestionarioGrupo", newName: "AvaliacaoQuestionario");
            AddForeignKey("dbo.AvaliacaoQuestionario", "IdQuestionario", "dbo.Questionario", "Id");
        }
    }
}
