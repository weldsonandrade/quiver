using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Quiver.Data.Interfaces
{
    public interface IQuiverDbContext
    {
        DbSet<Empresa> Empresas { get; set; }

        DbSet<Grupo> Grupos { get; set; }

        IDbSet<Unidade> Unidades { get; set; }

        DbSet<Questionario> Questionarios { get; set; }

        DbSet<Questao> Questoes { get; set; }

        DbSet<Item> Itens { get; set; }

        DbSet<Avaliacao> Avalicoes { get; set; }

        DbSet<AvaliacaoQuestionarioGrupo> QuestionariosAvaliacaoGrupo { get; set; }

        DbSet<Notificacao> Notificacoes { get; set; }

        DbSet<PlanoAcao> PlanosAcao { get; set; }

        DbSet<Historico> Historicos { get; set; }

        DbSet<MensagemGestor> MensagensGestor { get; set; }

        DbSet<MensagemResponsavel> MensagensResponsavel { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void SaveChanges();

        void Dispose();
    }
}
