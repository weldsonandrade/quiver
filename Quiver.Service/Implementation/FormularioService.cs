using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using Quiver.DTO.Questionario;
using Quiver.Service.Mappers;
using Quiver.Core.Models;
using System.Linq;
using Quiver.Data;
using System.Data.Entity;
using Quiver.DTO.Grupo;

namespace Quiver.Service.Implementation
{
    public class FormularioService : IFormularioService
    {
        private IUnitOfWork _uow;

        public FormularioService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public QuestionarioDTO GetAtivoById(int idQuestionario)
        {
            var questionarioDb = GetByID(idQuestionario, filtraApenasAtivos: true);

            return FormularioMapper.MapQuestionarioToQuestionarioDTO(questionarioDb);
        }

        public QuestionarioDTO GetById(int idQuestionario)
        {
            var questionarioDb = GetByID(idQuestionario);

            return FormularioMapper.MapQuestionarioToQuestionarioDTO(questionarioDb);
        }

        public IEnumerable<QuestionarioDTO> GetQuestionariosAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome)
        {
            return FormularioMapper.MapQuestionarioToQuestionarioDTO(_uow.QuestionarioRepository.GetAtivosByEmpresaAndStartWithNome(idEmpresa, startWithNome));
        }

        public void AtualizarGrupo(GrupoDTO grupoDTO)
        {
            Grupo grupo = _uow.GrupoRepository.GetByID(grupoDTO.Id);
            List<int> questionariosDoGrupo = grupoDTO.Questionarios.Select(q => q.IdQuestionario).ToList();

            List<int> questionariosInseridosAoGrupo = new List<int>();
            List<int> questionariosRemovidosDoGrupo = new List<int>();
            // Identificar quais foram inseridos e quais foram removidos.
            List<QuestionarioGrupo> questionariosGrupo = _uow.QuestionarioGrupoRepository.GetAtivosByGrupo(grupo.Id).ToList();
            // Find inseridos.
            questionariosInseridosAoGrupo.AddRange(questionariosDoGrupo.Except(questionariosGrupo.Select(qg => qg.IdQuestionario)));
            // Find removidos.
            questionariosRemovidosDoGrupo.AddRange(questionariosGrupo.Select(qg => qg.IdQuestionario).Except(questionariosDoGrupo));

            // Inclusão de questionário ao Grupo
            // Cria relacinamento entre grupo e questionário.
            // Adiciona o questionário grupo para as avaliações que ainda não foram realizadas.
            IEnumerable<Avaliacao> avaliacoesNaoAvaliadasDoGrupo = _uow.AvaliacaoRepository.GetByGrupoAndSituacao(grupo.Id, SituacaoAvaliacao.NAO_AVALIADO).ToList();
            foreach (int idQuestionario in questionariosInseridosAoGrupo)
            {
                Questionario questionario = _uow.QuestionarioRepository.GetAtivoById(idQuestionario);
                QuestionarioGrupo qg = _uow.QuestionarioGrupoRepository.GetByQuestionarioAndGrupo(idQuestionario, grupo.Id);
                if (qg == null)
                {
                    qg = new QuestionarioGrupo()
                    {
                        Grupo = grupo,
                        Questionario = questionario
                    };
                    _uow.QuestionarioGrupoRepository.Insert(qg);
                }
                foreach (Avaliacao avaliacao in avaliacoesNaoAvaliadasDoGrupo)
                {
                    _uow.AvaliacaoQuestionarioGrupoRepository.Insert(new AvaliacaoQuestionarioGrupo()
                    {
                        QuestionarioGrupo = qg,
                        Avaliacao = avaliacao
                    });
                }
            }

            // Remoção de questionário ao grupo.
            foreach (int idQuestionario in questionariosRemovidosDoGrupo)
            {
                IEnumerable<Avaliacao> avaliacoesAvaliadasDoQuestionario = _uow.AvaliacaoRepository.GetAvaliadasByQuestionario(idQuestionario);
                // Se não existe avaliação realizada apega o questionário do grupo.
                if (avaliacoesAvaliadasDoQuestionario.Count() == 0)
                {
                    QuestionarioGrupo qgToDelete = _uow.QuestionarioGrupoRepository.GetByQuestionarioAndGrupo(idQuestionario, grupo.Id);
                    _uow.QuestionarioGrupoRepository.Delete(qgToDelete);
                    List<AvaliacaoQuestionarioGrupo> aqgToDelete = _uow.AvaliacaoQuestionarioGrupoRepository.GetNaoAvaliadosByQuestionarioAndGrupo(idQuestionario, grupo.Id).ToList();
                    List<int> avaliacoesToDelete = new List<int>();
                    foreach (AvaliacaoQuestionarioGrupo aqg in aqgToDelete)
                    {
                        if (aqg.Avaliacao.QuestionariosAvaliacao.Count == 1)
                        {
                            avaliacoesToDelete.Add(aqg.IdAvaliacao);
                        }
                        _uow.AvaliacaoQuestionarioGrupoRepository.Delete(aqg);
                    }
                    foreach (int idAvaliacao in avaliacoesToDelete)
                    {
                        _uow.AvaliacaoRepository.Delete(idAvaliacao);
                    }
                }
                else
                {
                    // Não se pode apagar o relacionamento QuestionarioGrupo devido a avaliação já realizada.
                    // Faz uma cópia do Questionário, cria os relacionamentos com grupos e substitui as avaliações que não foram avaliadas pelo da cópia.
                    // Ex: A1 (S = não avaliada) para q1 e g1. Cria q2 (cópia de q1) relacionado à g1. 
                    // E todas as avaliaões em aberto para q1g1 vira q2g1.
                    // Faz isso para todos os grupos dos formulário.
                    Questionario questionario = _uow.QuestionarioRepository.GetAtivoById(idQuestionario);
                    QuestionarioDTO qDTO = FormularioMapper.MapQuestionarioToQuestionarioDTO(questionario);
                    questionario = FormularioMapper.MapQuestionarioDTOToQuestionario(qDTO);

                    Questionario copiaQuestionario = CopiarForumulario(questionario);
                    // Remover o questionário do grupo.
                    copiaQuestionario.Grupos.Remove(copiaQuestionario.Grupos.Single(qg => qg.IdGrupo == grupo.Id));
                    _uow.QuestionarioRepository.Insert(copiaQuestionario);
                }
            }
            _uow.SaveChanges();
        }

        public void Insert(QuestionarioDTO questionario)
        {
            questionario.Grupos = FormularioMapper.MapQuestionarioGrupoToQuestionarioGrupoDTO(_uow.QuestionarioGrupoRepository.GetByQuestionario(questionario.Id));
            var questionarioToInsert = FormularioMapper.MapQuestionarioDTOToQuestionario(questionario);
            // Se for atualização de um questionário existente.
            if (questionarioToInsert.Id != 0)
            {
                CopiarForumulario(questionarioToInsert);                
            }

            _uow.QuestionarioRepository.Insert(questionarioToInsert);
            _uow.SaveChanges();
        }

        private Questionario CopiarForumulario(Questionario questionarioToInsert)
        {
            var questionarioDb = _uow.QuestionarioRepository.GetByID(questionarioToInsert.Id);
            questionarioDb.Excluido = true;
            
            // Removendo as avaliações que não foram realizadas.
            foreach (QuestionarioGrupo qg in questionarioDb.Grupos)
            {
                List<AvaliacaoQuestionarioGrupo> aQParaRemover = new List<AvaliacaoQuestionarioGrupo>();
                aQParaRemover.AddRange(_uow.AvaliacaoQuestionarioGrupoRepository.GetNaoAvaliadosByQuestionarioAndGrupo(qg.IdQuestionario, qg.IdGrupo).ToList());

                // Guardando os Ids para inserir com o novo questionário.
                List<int> avaliacaoIds = aQParaRemover.Select(aq => aq.Avaliacao.Id).ToList();
                aQParaRemover.ForEach(qgToRemove => questionarioDb.Grupos.ToList().ForEach(aqg => aqg.Avaliacoes.Remove(qgToRemove)));
                _uow.QuestionarioRepository.Update(questionarioDb);

                QuestionarioGrupo questionarioGrupoToInsert = questionarioToInsert.Grupos.FirstOrDefault(qgToInsert => qgToInsert.IdGrupo == qg.IdGrupo && qgToInsert.IdQuestionario == qg.IdQuestionario);
                if (questionarioGrupoToInsert != null)
                {
                    Grupo grupo = _uow.GrupoRepository.GetByID(questionarioGrupoToInsert.IdGrupo);
                    questionarioGrupoToInsert.Grupo = grupo;
                    // Inserir novo questionários para as avaliações que existiam.
                    foreach (var aId in avaliacaoIds)
                    {
                        var avaliacao = _uow.AvaliacaoRepository.GetByID(aId);
                        questionarioGrupoToInsert.Avaliacoes.Add(new AvaliacaoQuestionarioGrupo()
                        {
                            Avaliacao = avaliacao,                            
                            Situacao = SituacaoAvaliacao.NAO_AVALIADO
                        });
                    }
                }
            }

            questionarioToInsert.QuestionarioAnterior = questionarioDb;           

            return questionarioToInsert;
        }

        public void Delete(int questionario)
        {
            // Excluir todas as avaliações NÃO AVALIADAS que possuam apenas este questionario.
            var avaliacoesNaoAvaliadasQuePossuemOQuestionario = _uow.AvaliacaoRepository.GetNaoAvaliadasByQuestionario(questionario);
            foreach (Avaliacao avaliacao in avaliacoesNaoAvaliadasQuePossuemOQuestionario.ToList())
            {
                if (avaliacao.QuestionariosAvaliacao.Where(q => q.QuestionarioGrupo.Questionario.Excluido == false).Count() == 1)
                {
                    // Excluir avalição e avaliação questionarios.
                    avaliacao.QuestionariosAvaliacao.ToList().ForEach(aq => _uow.AvaliacaoQuestionarioGrupoRepository.Delete(aq));
                    _uow.AvaliacaoRepository.Delete(avaliacao);
                }
            }

            // Marca o questionário como deletado.
            var questionarioToDelete = GetByID(questionario, true);
            questionarioToDelete.Excluido = true;
            _uow.QuestionarioRepository.Update(questionarioToDelete);

            _uow.SaveChanges();
        }

        private Questionario GetByID(int idQuestionario, bool filtraApenasAtivos = false)
        {
            if (idQuestionario <= 0)
                throw new ArgumentOutOfRangeException(Resources.Exception.ID_MENOR_OU_IGUAL_A_ZERO);

            var questionario = filtraApenasAtivos ? _uow.QuestionarioRepository.GetAtivoById(idQuestionario) : _uow.QuestionarioRepository.GetByID(idQuestionario);

            if (questionario == null)
                throw new ArgumentOutOfRangeException(String.Format(Resources.Exception.ENTIDADE_NAO_ENCONTRADA, typeof(Questionario).Name));

            return questionario;
        }
    }
}
