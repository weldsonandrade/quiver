namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alternativa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        Peso = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                        NaoConformidade = c.Boolean(nullable: false),
                        ExigeJustificativa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Avaliacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataProgramada = c.DateTime(nullable: false),
                        DataCriacao = c.DateTime(nullable: false),
                        DataInicio = c.DateTime(),
                        DataFim = c.DateTime(),
                        LocalizacaoLatitude = c.Double(nullable: false),
                        LocalizacaoLongitude = c.Double(nullable: false),
                        Dispositivo = c.String(maxLength: 100),
                        PontuacaoMaxima = c.Int(),
                        PontuacaoEfetuada = c.Int(),
                        Assinatura = c.String(),
                        Observacao = c.String(maxLength: 500),
                        RotuloCalendario = c.String(maxLength: 100),
                        IdUnidade = c.Int(nullable: false),
                        IdUsuario = c.String(nullable: false, maxLength: 128),
                        Situacao = c.Int(nullable: false),
                        NomeResponsavel = c.String(maxLength: 100),
                        CargoResponsavel = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidade", t => t.IdUnidade)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdUnidade)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.AvaliacaoQuestionario",
                c => new
                    {
                        IdAvaliacao = c.Int(nullable: false),
                        IdQuestionario = c.Int(nullable: false),
                        Situacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdAvaliacao, t.IdQuestionario })
                .ForeignKey("dbo.Avaliacao", t => t.IdAvaliacao)
                .ForeignKey("dbo.Questionario", t => t.IdQuestionario)
                .Index(t => t.IdAvaliacao)
                .Index(t => t.IdQuestionario);
            
            CreateTable(
                "dbo.Questionario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordem = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 200),
                        Excluido = c.Boolean(nullable: false),
                        IdQuestionarioAnterior = c.Int(),
                        IdGrupo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grupo", t => t.IdGrupo)
                .ForeignKey("dbo.Questionario", t => t.IdQuestionarioAnterior)
                .Index(t => t.IdQuestionarioAnterior)
                .Index(t => t.IdGrupo);
            
            CreateTable(
                "dbo.Grupo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        IdEmpresa = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "dbo.Classificacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        InicioIntervalo = c.Int(nullable: false),
                        FimIntervalo = c.Int(nullable: false),
                        IdGrupo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grupo", t => t.IdGrupo)
                .Index(t => t.IdGrupo);
            
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Icone = c.String(nullable: false, maxLength: 100),
                        CNPJ = c.String(nullable: false, maxLength: 14),
                        Situacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Unidade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        IdEmpresa = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IdEmpresa = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Questao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 500),
                        Ordem = c.Int(nullable: false),
                        ExigeJustificativa = c.Boolean(nullable: false),
                        Tipo = c.Int(nullable: false),
                        ExigeResposta = c.Boolean(nullable: false),
                        IdQuestionario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionario", t => t.IdQuestionario)
                .Index(t => t.IdQuestionario);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdAlternativa = c.Int(),
                        IdQuestao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Alternativa", t => t.IdAlternativa)
                .ForeignKey("dbo.Questao", t => t.IdQuestao)
                .Index(t => t.IdAlternativa)
                .Index(t => t.IdQuestao);
            
            CreateTable(
                "dbo.Resposta",
                c => new
                    {
                        IdAvaliacao = c.Int(nullable: false),
                        IdQuestionario = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        Justificativa = c.String(maxLength: 500),
                        Pontos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AvaliacaoQuestionario", t => new { t.IdAvaliacao, t.IdQuestionario })
                .Index(t => new { t.IdAvaliacao, t.IdQuestionario });
            
            CreateTable(
                "dbo.Foto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fotografia = c.String(nullable: false),
                        IdResposta = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resposta", t => t.IdResposta)
                .Index(t => t.IdResposta);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UsuarioGrupo",
                c => new
                    {
                        Usuario_Id = c.String(nullable: false, maxLength: 128),
                        Grupo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Usuario_Id, t.Grupo_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.Usuario_Id, cascadeDelete: true)
                .ForeignKey("dbo.Grupo", t => t.Grupo_Id, cascadeDelete: true)
                .Index(t => t.Usuario_Id)
                .Index(t => t.Grupo_Id);
            
            CreateTable(
                "dbo.RespostaItem",
                c => new
                    {
                        Resposta_Id = c.Int(nullable: false),
                        Item_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Resposta_Id, t.Item_Id })
                .ForeignKey("dbo.Resposta", t => t.Resposta_Id, cascadeDelete: true)
                .ForeignKey("dbo.Item", t => t.Item_Id, cascadeDelete: true)
                .Index(t => t.Resposta_Id)
                .Index(t => t.Item_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Avaliacao", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Avaliacao", "IdUnidade", "dbo.Unidade");
            DropForeignKey("dbo.AvaliacaoQuestionario", "IdQuestionario", "dbo.Questionario");
            DropForeignKey("dbo.Questao", "IdQuestionario", "dbo.Questionario");
            DropForeignKey("dbo.RespostaItem", "Item_Id", "dbo.Item");
            DropForeignKey("dbo.RespostaItem", "Resposta_Id", "dbo.Resposta");
            DropForeignKey("dbo.Foto", "IdResposta", "dbo.Resposta");
            DropForeignKey("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" }, "dbo.AvaliacaoQuestionario");
            DropForeignKey("dbo.Item", "IdQuestao", "dbo.Questao");
            DropForeignKey("dbo.Item", "IdAlternativa", "dbo.Alternativa");
            DropForeignKey("dbo.Questionario", "IdQuestionarioAnterior", "dbo.Questionario");
            DropForeignKey("dbo.Questionario", "IdGrupo", "dbo.Grupo");
            DropForeignKey("dbo.Grupo", "IdEmpresa", "dbo.Empresa");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsuarioGrupo", "Grupo_Id", "dbo.Grupo");
            DropForeignKey("dbo.UsuarioGrupo", "Usuario_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "IdEmpresa", "dbo.Empresa");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Unidade", "IdEmpresa", "dbo.Empresa");
            DropForeignKey("dbo.Classificacao", "IdGrupo", "dbo.Grupo");
            DropForeignKey("dbo.AvaliacaoQuestionario", "IdAvaliacao", "dbo.Avaliacao");
            DropIndex("dbo.RespostaItem", new[] { "Item_Id" });
            DropIndex("dbo.RespostaItem", new[] { "Resposta_Id" });
            DropIndex("dbo.UsuarioGrupo", new[] { "Grupo_Id" });
            DropIndex("dbo.UsuarioGrupo", new[] { "Usuario_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Foto", new[] { "IdResposta" });
            DropIndex("dbo.Resposta", new[] { "IdAvaliacao", "IdQuestionario" });
            DropIndex("dbo.Item", new[] { "IdQuestao" });
            DropIndex("dbo.Item", new[] { "IdAlternativa" });
            DropIndex("dbo.Questao", new[] { "IdQuestionario" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "IdEmpresa" });
            DropIndex("dbo.Unidade", new[] { "IdEmpresa" });
            DropIndex("dbo.Classificacao", new[] { "IdGrupo" });
            DropIndex("dbo.Grupo", new[] { "IdEmpresa" });
            DropIndex("dbo.Questionario", new[] { "IdGrupo" });
            DropIndex("dbo.Questionario", new[] { "IdQuestionarioAnterior" });
            DropIndex("dbo.AvaliacaoQuestionario", new[] { "IdQuestionario" });
            DropIndex("dbo.AvaliacaoQuestionario", new[] { "IdAvaliacao" });
            DropIndex("dbo.Avaliacao", new[] { "IdUsuario" });
            DropIndex("dbo.Avaliacao", new[] { "IdUnidade" });
            DropTable("dbo.RespostaItem");
            DropTable("dbo.UsuarioGrupo");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Foto");
            DropTable("dbo.Resposta");
            DropTable("dbo.Item");
            DropTable("dbo.Questao");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Unidade");
            DropTable("dbo.Empresa");
            DropTable("dbo.Classificacao");
            DropTable("dbo.Grupo");
            DropTable("dbo.Questionario");
            DropTable("dbo.AvaliacaoQuestionario");
            DropTable("dbo.Avaliacao");
            DropTable("dbo.Alternativa");
        }
    }
}
