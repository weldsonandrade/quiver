using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using System;
using System.Data.Entity.Validation;
using System.Text;
using Quiver.Data.Interfaces;

namespace Quiver.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuiverDbContext context = new QuiverDbContext();

        private IRepository<Alternativa> _alternativaRepository;
        private IAvaliacaoRepository _avaliacaoRepository;
        private IAvaliacaoQuestionarioGrupoRepository _avaliacaoQuestionarioGrupoRepository;
        private IGrupoRepository _grupoRepository;
        private IUnidadeRepository _unidadeRepository;
        private IRepository<Classificacao> _classificacaoRepository;
        private IEmpresaRepository _empresaRepository;
        private IRepository<Foto> _fotoRepository;
        private IRepository<Item> _itemRepository;
        private IRepository<Questao> _questaoRepository;
        private IQuestionarioRepository _questionarioRepository;
        private IRepository<Resposta> _respostaRepository;
        private IUsuarioRepository _usuarioRepository;
        private IPerfilRepository _roleRepository;
        private INotificacaoRepository _notificacaoRepository;
        private IPlanoAcaoRepository _planoAcaoRepository;
        private IRepository<Historico> _historicoRepository;
        private IRepository<MensagemGestor> _mensagemGestorRepository;
        private IRepository<MensagemResponsavel> _mensagemResponsavelRepository;
        private IConfiguracaoRepository _configuracaoRepository;
        private IQuestionarioGrupoRepository _questionarioGrupoRepository;

        public IRepository<Alternativa> AlternativaRepository
        {
            get
            {
                if (_alternativaRepository == null)
                {
                    _alternativaRepository = new GenericRepository<Alternativa>(context);
                }

                return _alternativaRepository;
            }
        }

        public IAvaliacaoRepository AvaliacaoRepository
        {
            get
            {
                if (_avaliacaoRepository == null)
                {
                    _avaliacaoRepository = new AvaliacaoRepository(context);
                }

                return _avaliacaoRepository;
            }
        }

        public IAvaliacaoQuestionarioGrupoRepository AvaliacaoQuestionarioGrupoRepository
        {
            get
            {
                if (_avaliacaoQuestionarioGrupoRepository == null)
                {
                    _avaliacaoQuestionarioGrupoRepository = new AvaliacaoQuestionarioGrupoRepository(context);
                }

                return _avaliacaoQuestionarioGrupoRepository;
            }
        }

        public IGrupoRepository GrupoRepository
        {
            get
            {
                if (_grupoRepository == null)
                {
                    _grupoRepository = new GrupoRepository(context);
                }

                return _grupoRepository;
            }
        }

        public IUnidadeRepository UnidadeRepository
        {
            get
            {
                if (_unidadeRepository == null)
                {
                    _unidadeRepository = new UnidadeRepository(context);
                }

                return _unidadeRepository;
            }
        }

        public IRepository<Classificacao> ClassificacaoRepository
        {
            get
            {
                if (_classificacaoRepository == null)
                {
                    _classificacaoRepository = new GenericRepository<Classificacao>(context);
                }

                return _classificacaoRepository;
            }
        }

        public IEmpresaRepository EmpresaRepository
        {
            get
            {
                if (_empresaRepository == null)
                {
                    _empresaRepository = new EmpresaRepository(context);
                }

                return _empresaRepository;
            }
        }

        public IRepository<Foto> FotoRepository
        {
            get
            {
                if (_fotoRepository == null)
                {
                    _fotoRepository = new GenericRepository<Foto>(context);
                }

                return _fotoRepository;
            }
        }

        public IRepository<Item> ItemRepository
        {
            get
            {
                if (_itemRepository == null)
                {
                    _itemRepository = new GenericRepository<Item>(context);
                }

                return _itemRepository;
            }
        }

        public IRepository<Questao> QuestaoRepository
        {
            get
            {
                if (_questaoRepository == null)
                {
                    _questaoRepository = new GenericRepository<Questao>(context);
                }

                return _questaoRepository;
            }
        }

        public IQuestionarioRepository QuestionarioRepository
        {
            get
            {
                if (_questionarioRepository == null)
                {
                    _questionarioRepository = new QuestionarioRepository(context);
                }

                return _questionarioRepository;
            }
        }

        public IRepository<Resposta> RespostaRepository
        {
            get
            {
                if (_respostaRepository == null)
                {
                    _respostaRepository = new GenericRepository<Resposta>(context);
                }

                return _respostaRepository;
            }
        }

        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                if (_usuarioRepository == null)
                {
                    _usuarioRepository = new UsuarioRepository(context);
                }

                return _usuarioRepository;
            }
        }

        public IPerfilRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new PerfilRepository(context);
                }

                return _roleRepository;
            }
        }

        public INotificacaoRepository NotificacaoRepository
        {
            get
            {
                if (_notificacaoRepository == null)
                {
                    _notificacaoRepository = new NotificacaoRepository(context);
                }

                return _notificacaoRepository;
            }
        }

        public IPlanoAcaoRepository PlanoAcaoRepository
        {
            get
            {
                if (_planoAcaoRepository == null)
                {
                    _planoAcaoRepository = new PlanoAcaoRepository(context);
                }

                return _planoAcaoRepository;
            }
        }

        public IRepository<Historico> HistoricoRepository
        {
            get
            {
                if (_historicoRepository == null)
                {
                    _historicoRepository = new GenericRepository<Historico>(context);
                }

                return _historicoRepository;
            }
        }

        public IRepository<MensagemGestor> MensagemGestorRepository
        {
            get
            {
                if (_mensagemGestorRepository == null)
                {
                    _mensagemGestorRepository = new GenericRepository<MensagemGestor>(context);
                }

                return _mensagemGestorRepository;
            }
        }

        public IRepository<MensagemResponsavel> MensagemResponsavelRepository
        {
            get
            {
                if (_mensagemResponsavelRepository == null)
                {
                    _mensagemResponsavelRepository = new GenericRepository<MensagemResponsavel>(context);
                }

                return _mensagemResponsavelRepository;
            }
        }

        public IConfiguracaoRepository ConfiguracaoRepository
        {
            get
            {
                if (_configuracaoRepository == null)
                {
                    _configuracaoRepository = new ConfiguracaoRepository(context);
                }

                return _configuracaoRepository;
            }
        }

        public IQuestionarioGrupoRepository QuestionarioGrupoRepository
        {
            get
            {
                if (_questionarioGrupoRepository == null)
                {
                    _questionarioGrupoRepository = new QuestionarioGrupoRepository(context);
                }

                return _questionarioGrupoRepository;
            }
        }

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.Append(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw new Exception(sb.ToString());
            }
        }

        public QuiverDbContext GetDbContext<QuiverDbContext>() where QuiverDbContext : System.Data.Entity.DbContext
        {
            return context as QuiverDbContext;
        }
    }
}
