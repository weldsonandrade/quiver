using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using System;
using System.Data.Entity;

namespace Quiver.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Alternativa> AlternativaRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        IAvaliacaoQuestionarioGrupoRepository AvaliacaoQuestionarioGrupoRepository { get; }
        IGrupoRepository GrupoRepository { get; }
        IRepository<Classificacao> ClassificacaoRepository { get; }
        IEmpresaRepository EmpresaRepository { get; }
        IRepository<Foto> FotoRepository { get; }
        IRepository<Item> ItemRepository { get; }
        IRepository<Questao> QuestaoRepository { get; }
        IQuestionarioRepository QuestionarioRepository { get; }
        IRepository<Resposta> RespostaRepository { get; }
        IUnidadeRepository UnidadeRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        IPerfilRepository RoleRepository { get; }
        INotificacaoRepository NotificacaoRepository { get; }
        IPlanoAcaoRepository PlanoAcaoRepository { get; }
        IRepository<Historico> HistoricoRepository { get; }
        IRepository<MensagemGestor> MensagemGestorRepository { get; }
        IRepository<MensagemResponsavel> MensagemResponsavelRepository { get; }        
        IConfiguracaoRepository ConfiguracaoRepository { get; }
        IQuestionarioGrupoRepository QuestionarioGrupoRepository { get; }

        TContext GetDbContext<TContext>() where TContext : DbContext;

        void SaveChanges();
    }
}
