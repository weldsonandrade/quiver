using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Quiver.Common.Utils;
using Quiver.DTO.Avaliacao;
using Quiver.Core.Models;
using Quiver.Service.Mappers;

namespace Quiver.Service.Implementation
{
    public class AgendaService : IAgendaService
    {
        private IUnitOfWork _uow;

        public AgendaService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Delete(int idAvaliacao)
        {
            var avaliacaoToDelete = _uow.AvaliacaoRepository.GetByID(idAvaliacao);

            // Colocar texto em um resource e lançar uma exceção especifica.
            if (avaliacaoToDelete.Situacao != SituacaoAvaliacao.NAO_AVALIADO)
                throw new Exception("Não é permitido alterar a data de uma avaliação já realizada.");

            foreach (AvaliacaoQuestionarioGrupo questionario in avaliacaoToDelete.QuestionariosAvaliacao.ToList())
            {
                _uow.AvaliacaoQuestionarioGrupoRepository.Delete(questionario);
            }

            _uow.AvaliacaoRepository.Delete(avaliacaoToDelete);
            _uow.SaveChanges();
        }


        public IEnumerable<AvaliacaoDTO> GetAvaliacoesByEmpresa(int idEmpresa)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByEmpresa(idEmpresa);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }

        public IEnumerable<AvaliacaoDTO> GetAvaliacaoByFilter(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario, 
            int idGrupo, bool? apenasAgendadas, bool? apenasConformes)
        {
            var avaliacoes = _uow.AvaliacaoRepository.GetByFilter(idEmpresa, dataInicial, dataFinal, 
                idUnidade, idUsuario, idGrupo, apenasAgendadas, apenasConformes);

            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }


        public AvaliacaoDTO GetAvaliacaoById(int idAvaliacao)
        {
            var avaliacao = GetAvaliacaoByID(idAvaliacao, false);

            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacao);
        }

        public AvaliacaoDTO GetAvaliacaoAvaliadaById(int idAvaliacao)
        {
            var avaliacao = GetAvaliacaoByID(idAvaliacao, true);

            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacao);
        }

        public IEnumerable<AvaliacaoDTO> GetAvaliacoesAtrasadasByEmpresa(int idEmpresa)
        {
            IEnumerable<Avaliacao> avaliacoesAtrasadas = _uow.AvaliacaoRepository.GetAtrasadasByEmpresa(idEmpresa);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesAtrasadas);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesByEmpresaAndStartWithRotulo(int idEmpresa, string termo)
        {
            var avaliacoes = _uow.AvaliacaoRepository.GetByEmpresaAndStartWithRotulo(idEmpresa, termo);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesByUsuario(string idUsuario)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByUsuario(idUsuario);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }
        
        public IList<AvaliacaoDTO> GetAvaliacoesByUnidade(int idUnidade)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByUnidade(idUnidade);
          return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }
        
        public IList<AvaliacaoDTO> GetAvaliacoesByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByUsuarioAndPeriodo(idUsuario, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }
        
        public IList<AvaliacaoDTO> GetAvaliacoesByUnidadeAndPeriodo(string idUnidade, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByUsuarioAndPeriodo(idUnidade, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }
        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetFinalizadasByUsuarioAndPeriodo(idUsuario, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetFinalizadasComPontosByUsuarioAndPeriodo(idUsuario, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoes);
        }

        public void Insert(AvaliacaoDTO avaliacao)
        {
            var avaliacaoToInsert = AgendaMapper.MapAvaliacaoDTOToAvaliacao(avaliacao);
            avaliacaoToInsert.Situacao = SituacaoAvaliacao.NAO_AVALIADO;
            avaliacaoToInsert.Agendada = true;
            avaliacaoToInsert.DataCriacao = TZUtil.GetDataDeBrasilia();
            avaliacaoToInsert.DataFim = null;
            avaliacaoToInsert.DataInicio = null;

            var questionariosGrupo = _uow.QuestionarioGrupoRepository.GetAtivosByGrupo(avaliacao.IdGrupo);
            foreach (QuestionarioGrupo questionarioGrupo in questionariosGrupo)
            {
                _uow.AvaliacaoQuestionarioGrupoRepository.Insert(new AvaliacaoQuestionarioGrupo()
                {
                    QuestionarioGrupo = questionarioGrupo,
                    Avaliacao = avaliacaoToInsert,
                    Situacao = SituacaoAvaliacao.NAO_AVALIADO
                });
            }

            _uow.AvaliacaoRepository.Insert(avaliacaoToInsert);
            _uow.SaveChanges();
        }

        public void Update(AvaliacaoDTO avaliacao)
        {
            var avaliacaoToUpdate = _uow.AvaliacaoRepository.GetByID(avaliacao.Id);

            // Só permite alterar avaliações que não foram finalizadas.
            if (avaliacaoToUpdate.Situacao == SituacaoAvaliacao.NAO_AVALIADO)
            {
                avaliacaoToUpdate.DataProgramada = avaliacao.DataProgramada;
                avaliacaoToUpdate.RotuloCalendario = avaliacao.RotuloCalendario;

                var idGrupo = avaliacaoToUpdate.QuestionariosAvaliacao.First().IdGrupo;
                if (idGrupo != avaliacao.IdGrupo)
                {
                    avaliacaoToUpdate.QuestionariosAvaliacao = null;
                    var questionariosGrupo = _uow.QuestionarioGrupoRepository.GetAtivosByGrupo(avaliacao.IdGrupo);
                    foreach (QuestionarioGrupo qg in questionariosGrupo)
                    {
                        _uow.AvaliacaoQuestionarioGrupoRepository.Insert(new AvaliacaoQuestionarioGrupo()
                        {
                            QuestionarioGrupo = qg,
                            Avaliacao = avaliacaoToUpdate,
                            Situacao = SituacaoAvaliacao.NAO_AVALIADO
                        });
                    }
                }               

                avaliacaoToUpdate.Unidade = _uow.UnidadeRepository.GetByID(avaliacao.IdUnidade);
                avaliacaoToUpdate.Usuario = _uow.UsuarioRepository.GetByID(avaliacao.IdUsuario);

                _uow.AvaliacaoRepository.Update(avaliacaoToUpdate);
                _uow.SaveChanges();
            }

            // Só será usado depois de migrar a API para utilizar a camada de serviço
            if (avaliacaoToUpdate.Situacao == SituacaoAvaliacao.AVALIADO)
            {
                avaliacaoToUpdate.Conforme = PossuiAlgumaRespostaNaoConformidade(avaliacao);
            }
        }

        public void UpdateDataProgramada(AvaliacaoDTO avaliacao)
        {
            var avaliacaoToUpdate = _uow.AvaliacaoRepository.GetByID(avaliacao.Id);

            if (avaliacaoToUpdate.Situacao == SituacaoAvaliacao.AVALIADO) { 
                throw new Exception(Resources.Exception.ALTERAR_DATA_INSPECAO_REALIZADA);
            }

            avaliacaoToUpdate.DataProgramada = avaliacao.DataProgramada;
            _uow.AvaliacaoRepository.Update(avaliacaoToUpdate);
            _uow.SaveChanges();
        }

        private Avaliacao GetAvaliacaoByID(int idAvaliacao, bool apenasAvaliada)
        {
            if (idAvaliacao <= 0)
                throw new ArgumentOutOfRangeException(Resources.Exception.ID_MENOR_OU_IGUAL_A_ZERO);

            Avaliacao avaliacao = null;
            if (apenasAvaliada)
            {
                avaliacao = _uow.AvaliacaoRepository.GetAvaliadaById(idAvaliacao);
            }
            else
            { 
                avaliacao = _uow.AvaliacaoRepository.GetById(idAvaliacao);
            }

            if (avaliacao == null)
                throw new ArgumentOutOfRangeException(String.Format(Resources.Exception.ENTIDADE_NAO_ENCONTRADA, typeof(Avaliacao).Name));

            return avaliacao;
        }

        public IEnumerable<AvaliacaoDTO> GetAvaliacoesEmAndamentoByEmpresa(int idEmpresa)
        {
            IEnumerable<Avaliacao> avaliacoesEmAndamento = _uow.AvaliacaoRepository.GetEmAndamentoByEmpresa(idEmpresa);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesEmAndamento);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoesFinalizadas = _uow.AvaliacaoRepository.GetFinalizadasByUnidadeAndPeriodo(idUnidade, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesFinalizadas);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoesFinalizadas = _uow.AvaliacaoRepository.GetFinalizadasComPontosByUnidadeAndPeriodo(idUnidade, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesFinalizadas);
        }


        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoesFinalizadas = _uow.AvaliacaoRepository.GetFinalizadasByGrupoAndPeriodo(idGrupo, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesFinalizadas);
        }

        public IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<Avaliacao> avaliacoesFinalizadas = _uow.AvaliacaoRepository.GetFinalizadasComPontosByGrupoAndPeriodo(idGrupo, dataInicial, dataFinal);
            return AgendaMapper.MapAvaliacaoToAvaliacaoDTO(avaliacoesFinalizadas);
        }

        public IList<EventoDTO> GetAvaliacaoByEmpresaAndPeriodoAndUnidadeAndUsuario(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario)
        {
            IEnumerable<Avaliacao> avaliacoes = _uow.AvaliacaoRepository.GetByEmpresaAndPeriodoAndUnidadeAndUsuario(idEmpresa, dataInicial, dataFinal, idUnidade, idUsuario); 
           
            return AgendaMapper.MapAvaliacaoToEventoDTO(avaliacoes);
        }

        private bool PossuiAlgumaRespostaNaoConformidade(AvaliacaoDTO avaliacao)
        {
            foreach (var questionario in avaliacao.QuestionariosGrupo)
            {
                foreach (var questao in questionario.QuestionarioGrupo.Questionario.Questoes)
                {
                    foreach (var item in questao.Itens)
                    {
                        if (item.Alternativa != null && item.Alternativa.NaoConformidade == true)
                        {
                            // Verifica se a o item está entre as respostas.
                            if (questionario.Respostas.FirstOrDefault(r => r.Itens.Contains(item.Id)) != null)
                            {
                                return true;
                            };
                        }
                    }
                }
            }
            return false;
        }

        public int GetIdEmpresaOfAvaliacao(int IdAvaliacao)
        {
            return _uow.AvaliacaoRepository.GetIdEmpresaById(IdAvaliacao);
        }
    }
}
