using System;
using System.Linq;
using Quiver.Core.Models;
using Quiver.Service.Interfaces;
using Quiver.Data.Interfaces;
using System.Collections.Generic;
using Quiver.DTO.Unidade;
using Quiver.Service.Mappers;

namespace Quiver.Service.Implementation
{
    public class UnidadeService : IUnidadeService
    {
        private IUnitOfWork _uow;

        public UnidadeService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Delete(int unidadeToDelete)
        {
            var unidade = GetByID(unidadeToDelete);

            // Remove todas as avaliações (com seus respectivos questionários) que não foram avaliadas.
            var avaliacoes = _uow.AvaliacaoRepository.GetByUnidadeAndSituacao(unidadeToDelete, SituacaoAvaliacao.NAO_AVALIADO);
            foreach (var avaliacao in avaliacoes)
            {
                avaliacao.QuestionariosAvaliacao.Clear();
                _uow.AvaliacaoRepository.Delete(avaliacao);
            }

            // Exclui logicamente.
            unidade.Excluido = true;

            _uow.UnidadeRepository.Update(unidade);
            _uow.SaveChanges();
        }

        public IList<UnidadeDTO> GetAtivosByEmpresa(int IdEmpresa)
        {
            return UnidadeMapper.MapUnidadeToUnidadeDTO(_uow.UnidadeRepository.GetAtivosByEmpresa(IdEmpresa));
        }

        public IList<UnidadeDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo)
        {
            var unidades = _uow.UnidadeRepository.GetAtivosByEmpresaAndStartWithNome(idEmpresa, termo);
            return UnidadeMapper.MapUnidadeToUnidadeDTO(unidades);
        }

        public UnidadeDTO GetById(int IdUnidade)
        {
            var unidadeDb = GetByID(IdUnidade);

            return (unidadeDb.Excluido == true) ? null : UnidadeMapper.MapUnidadeToUnidadeDTO(unidadeDb);
        }


        public UnidadeDTO GetByIdAndEmpresa(int IdUnidade, int idEmpresa)
        {
            var unidadeDb = GetByID(IdUnidade);

            return (unidadeDb.Excluido == true) || (unidadeDb.IdEmpresa != idEmpresa) ? null : UnidadeMapper.MapUnidadeToUnidadeDTO(unidadeDb);
        }


        public void Update(UnidadeDTO unidade)
        {
            var unidadeToUpdate = GetByID(unidade.Id);

            unidadeToUpdate.Nome = unidade.Nome.Trim();
            _uow.UnidadeRepository.Update(unidadeToUpdate);
            _uow.SaveChanges();
        }

        public void Insert(UnidadeDTO unidade)
        {
            var unidadeToInsert = Unidade.FactoryInsert(unidade.Nome, unidade.IdEmpresa);
            _uow.UnidadeRepository.Insert(unidadeToInsert);
            _uow.SaveChanges();
        }

        private Unidade GetByID(int IdUnidade)
        {
            if (IdUnidade <= 0)
                throw new ArgumentOutOfRangeException(Resources.Exception.ID_MENOR_OU_IGUAL_A_ZERO);

            var unidade = _uow.UnidadeRepository.GetByID(IdUnidade);

            if (unidade == null)
                throw new ArgumentOutOfRangeException(Resources.Exception.ENTIDADE_NAO_ENCONTRADA, typeof(Unidade).Name);

            return unidade;
        }

        public int GetIdEmpresaOfUnidade(int IdUnidade)
        {
            return _uow.UnidadeRepository.GetIdEmpresaByIdUnidade(IdUnidade);
        }
    }
}
