using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiver.DTO.Grupo;
using Quiver.Service.Mappers;
using Quiver.Core.Models;

namespace Quiver.Service.Implementation
{
    public class GrupoService : IGrupoService
    {
        private IUnitOfWork _uow;

        public GrupoService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public IList<GrupoDTO> GetAtivosByEmpresa(int IdEmpresa)
        {
            return GrupoMapper.MapGrupoToGrupoDTO(_uow.GrupoRepository.GetAtivosByEmpresa(IdEmpresa));
        }

        public IList<GrupoDTO> GetAtivosByEmpresaWithAtLeastOneQuestionario(int idEmpresa)
        {
            var grupos = _uow.GrupoRepository.GetAtivosByEmpresaWithAtLeastOneQuestionario(idEmpresa);
            return GrupoMapper.MapGrupoToGrupoDTO(grupos);
        }

        public IList<GrupoDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo)
        {
            var grupos = _uow.GrupoRepository.GetAtivosByEmpresaAndStartWithNome(idEmpresa, termo);
            return GrupoMapper.MapGrupoToGrupoDTO(grupos);
        }

        public GrupoDTO GetById(int IdGrupo)
        {
            var grupoDb = GetByID(IdGrupo);

            return GrupoMapper.MapGrupoToGrupoDTO(grupoDb);
        }

        public void Insert(GrupoDTO grupo)
        {
            var grupoToInsert = new Grupo()
            {
                Nome = grupo.Nome.Trim(),
                IdEmpresa = grupo.IdEmpresa,
                Classificacoes = grupo.Classificacoes.Select(cl => new Classificacao()
                {
                  Descricao = cl.Descricao,
                  Cor = cl.Cor,
                  InicioIntervalo = cl.InicioIntervalo,
                  FimIntervalo = cl.FimIntervalo 
                }).ToList()
            };
            _uow.GrupoRepository.Insert(grupoToInsert);
            _uow.SaveChanges();
        }

        public void Update(GrupoDTO grupo)
        {
            var grupoToUpdate = GetByID(grupo.Id);

            grupoToUpdate.Nome = grupo.Nome.Trim();

            List<Classificacao> classificacoesParaApagar = grupoToUpdate.Classificacoes.ToList();

            foreach (var classificacao in grupo.Classificacoes)
            {
                if (classificacao.Id != 0)
                {
                    var classificacaoDb = _uow.ClassificacaoRepository.GetByID(classificacao.Id);
                    classificacaoDb.Descricao = classificacao.Descricao;
                    classificacaoDb.InicioIntervalo = classificacao.InicioIntervalo;
                    classificacaoDb.FimIntervalo = classificacao.FimIntervalo;
                    classificacaoDb.Cor = classificacao.Cor;
                    _uow.ClassificacaoRepository.Update(classificacaoDb);

                    // Se existe não precisa ser removido, logo remove desta lista.
                    classificacoesParaApagar.Remove(classificacaoDb);
                }
                else
                {
                    classificacao.IdGrupo = grupo.Id;
                    var classificacaoToInsert = GrupoMapper.MapClassificacaoDTOToClassificacao(classificacao);
                    _uow.ClassificacaoRepository.Insert(classificacaoToInsert);
                }
            }

            foreach (var classificacaoParaApagar in classificacoesParaApagar)
            {
                _uow.ClassificacaoRepository.Delete(classificacaoParaApagar);
            }

            _uow.GrupoRepository.Update(grupoToUpdate);
            _uow.SaveChanges();
        }

        public void Delete(int grupoToDelete)
        {
            var grupo = GetByID(grupoToDelete);

            // Remove todas as avaliações (com seus respectivos questionários) que não foram avaliadas.
            var avaliacoes = _uow.AvaliacaoRepository.GetByGrupoAndSituacao(grupoToDelete, SituacaoAvaliacao.NAO_AVALIADO);
            foreach (var avaliacao in avaliacoes)
            {
                avaliacao.QuestionariosAvaliacao.Clear();
                _uow.AvaliacaoRepository.Delete(avaliacao);
            }

            // Exclui logicamente.
            grupo.Excluido = true;

            // Altera todos os questionários para excluidos.
            foreach(QuestionarioGrupo questionarioGrupo in grupo.Questionarios)
            {
                questionarioGrupo.Questionario.Excluido = true;
            }

            _uow.GrupoRepository.Update(grupo);
            _uow.SaveChanges();
        }

        private Grupo GetByID(int IdGrupo)
        {
            if (IdGrupo <= 0)
                throw new ArgumentOutOfRangeException(Resources.Exception.ID_MENOR_OU_IGUAL_A_ZERO);

            var grupo = _uow.GrupoRepository.GetByID(IdGrupo);

            if (grupo == null)
                throw new ArgumentOutOfRangeException(String.Format(Resources.Exception.ENTIDADE_NAO_ENCONTRADA, typeof(Grupo).Name));

            return grupo;
        }


    }
}
