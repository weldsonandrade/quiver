namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionarioEmpresaRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questionario", "IdEmpresa", c => c.Int());
            CreateIndex("dbo.Questionario", "IdEmpresa");
            AddForeignKey("dbo.Questionario", "IdEmpresa", "dbo.Empresa", "Id");

            Sql(" UPDATE dbo.Questionario SET IdEmpresa = g.IdEmpresa FROM dbo.Questionario q " +
                " JOIN dbo.QuestionarioGrupo qg ON qg.IdQuestionario = q.Id" +
                " JOIN dbo.Grupo g ON g.Id = qg.IdGrupo " +
                " WHERE qg.IdQuestionario = q.Id");

            Sql(" UPDATE Questionario SET IdEmpresa = 9 WHERE IdEmpresa is NULL ");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questionario", "IdEmpresa", "dbo.Empresa");
            DropIndex("dbo.Questionario", new[] { "IdEmpresa" });
            DropColumn("dbo.Questionario", "IdEmpresa");
        }
    }
}
