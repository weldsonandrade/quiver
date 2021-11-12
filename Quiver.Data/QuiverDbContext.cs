using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.Data.Entity.Infrastructure;
using Quiver.Data.Interfaces;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Quiver.Data
{
    public class QuiverDbContext : IdentityDbContext<Usuario>, IQuiverDbContext
    {
        public QuiverDbContext() :  base("QuiverConnection")
        {
        }

        public static QuiverDbContext Create()
        {
            return new QuiverDbContext();
        }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Grupo> Grupos { get; set; }

        public IDbSet<Unidade> Unidades { get; set; }

        public DbSet<Questionario> Questionarios { get; set; }

        public DbSet<Questao> Questoes { get; set; }

        public DbSet<Item> Itens { get; set; }

        public DbSet<Avaliacao> Avalicoes { get; set; }

        public DbSet<AvaliacaoQuestionarioGrupo> QuestionariosAvaliacaoGrupo { get; set; }

        public DbSet<Notificacao> Notificacoes { get; set; }

        public DbSet<PlanoAcao> PlanosAcao { get; set; }

        public DbSet<Historico> Historicos{ get; set; }

        public DbSet<MensagemGestor> MensagensGestor { get; set; }

        public DbSet<MensagemResponsavel> MensagensResponsavel { get; set; }

        public DbSet<Configuracao> Configuracoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Trocar nome das tabelas se desejar
            //modelBuilder.Entity<CustomUserRole>().ToTable("UserRoles", "Security");
            //modelBuilder.Entity<CustomUserLogin>().ToTable("UserLogins", "Security");

            modelBuilder.Entity<RespostaItem>()
                .HasKey(x => new { x.IdResposta, x.IdItem })
                .HasOptional(x => x.PlanoAcao)
                .WithOptionalDependent(p => p.RespostaItem)
                .Map(configurationAction: new Action<ForeignKeyAssociationMappingConfiguration>(x => x.MapKey("IdPlanoAcao"))); ;

            modelBuilder.Entity<PlanoAcao>()
                .HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }

        IDbSet<TEntity> IQuiverDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry<TEntity>(entity);
        }

        void IQuiverDbContext.SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
