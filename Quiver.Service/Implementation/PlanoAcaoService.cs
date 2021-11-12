using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiver.DTO.PlanoAcao;
using Quiver.Core.Models;
using Quiver.Service.Mappers;
using Quiver.Common.Utils;

namespace Quiver.Service.Implementation
{
    public class PlanoAcaoService : IPlanoAcaoService
    {
        private IUnitOfWork _uow;

        public PlanoAcaoService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Cancelar(int idPlanoAcao)
        {
            var planoAcao = GetByID(idPlanoAcao);

            planoAcao.Situacao = SituacaoPlanoAcao.CANCELADO;

            _uow.PlanoAcaoRepository.Update(planoAcao);
            _uow.SaveChanges();
        }

        public IList<PlanoAcaoDTO> GetByEmpresa(int idEmpresa)
        {
            IEnumerable<PlanoAcao> planosAcao = _uow.PlanoAcaoRepository.GetByEmpresa(idEmpresa);
            return PlanoAcaoMapper.MapPlanoAcaoToPlanoAcaoDTO(planosAcao);
        }

        public IList<PlanoAcaoDTO> GetByEmpresaAndFiltro(int idEmpresa, DateTime dataInicial, DateTime dataFinal, string emailResponsavel, List<int> unidades, List<int> usuarios)
        {
            throw new NotImplementedException();
        }

        public IList<PlanoAcaoDTO> GetByEmpresaAndPeriodo(int idEmpresa, Nullable<DateTime> dataInicial, Nullable<DateTime> dataFinal)
        {
            IEnumerable<PlanoAcao> planosAcao = _uow.PlanoAcaoRepository.GetByEmpresaAndPeriodo(idEmpresa, dataInicial, dataFinal);
            return PlanoAcaoMapper.MapPlanoAcaoToPlanoAcaoDTO(planosAcao);
        }

        public IList<PlanoAcaoDTO> GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(int idEmpresa, string emailResponsavel, List<int> unidades, List<string> usuarios)
        {
            IEnumerable<PlanoAcao> planosAcao = _uow.PlanoAcaoRepository.GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(idEmpresa, emailResponsavel, unidades, usuarios);
            return PlanoAcaoMapper.MapPlanoAcaoToPlanoAcaoDTO(planosAcao);
        }

        public PlanoAcaoDTO GetById(int idPlanoAcao)
        {
            PlanoAcao planoAcao = _uow.PlanoAcaoRepository.GetByID(idPlanoAcao);
            return PlanoAcaoMapper.MapPlanoAcaoToPlanoAcaoDTO(planoAcao);
        }

        public void Update(PlanoAcaoDTO planoAcaoToUpdate)
        {
            var planoAcao = GetByID(planoAcaoToUpdate.Id);

            planoAcao.Atrasado = planoAcaoToUpdate.Quando.Date < TZUtil.GetDataDeBrasilia();
            planoAcao.Como = planoAcaoToUpdate.Como;
            if (planoAcaoToUpdate.Situacao == DTO.Enum.SituacaoPlanoAcao.ENCERRADO) {
                planoAcao.DataConclusao = TZUtil.GetDataHoraDeBrasilia();
            }
            planoAcao.Justificativa = planoAcaoToUpdate.Justificativa;
            planoAcao.Onde = planoAcaoToUpdate.Onde;
            planoAcao.OQue = planoAcaoToUpdate.OQue;
            planoAcao.Porque = planoAcaoToUpdate.Porque;
            planoAcao.Quando = planoAcaoToUpdate.Quando;
            planoAcao.Quanto = planoAcaoToUpdate.Quanto;
            planoAcao.Quem = planoAcaoToUpdate.Quem;
            planoAcao.Responsavel = planoAcaoToUpdate.Responsavel;
            planoAcao.Situacao = ((SituacaoPlanoAcao) (int) planoAcaoToUpdate.Situacao);            

            _uow.PlanoAcaoRepository.Update(planoAcao);
            _uow.SaveChanges();
        }

        private PlanoAcao GetByID(int IdPlanoAcao)
        {
            if (IdPlanoAcao <= 0)
                throw new ArgumentOutOfRangeException(Resources.Exception.ID_MENOR_OU_IGUAL_A_ZERO);

            var planoAcao = _uow.PlanoAcaoRepository.GetByID(IdPlanoAcao);

            if (planoAcao == null)
                throw new ArgumentOutOfRangeException(String.Format(Resources.Exception.ENTIDADE_NAO_ENCONTRADA, typeof(Grupo).Name));

            return planoAcao;
        }
    }
}
