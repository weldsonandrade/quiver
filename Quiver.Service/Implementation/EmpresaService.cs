using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiver.Core.Models;
using Quiver.DTO.Empresa;
using Quiver.Service.Mappers;
using Microsoft.AspNet.Identity;

namespace Quiver.Service.Implementation
{
    public class EmpresaService : IEmpresaService
    {
        private IUnitOfWork _uow;
        private IUsuarioService _usuarioService;

        public EmpresaService(IUnitOfWork uow, IUsuarioService usuarioService)
        {
            this._uow = uow;
            this._usuarioService = usuarioService;
        }

        public EmpresaDTO GetById(int IdEmpresa)
        {
            var empresa = _uow.EmpresaRepository.GetByID(IdEmpresa);
            return EmpresaMapper.MapEmpresaToEmpresaDTO(empresa);
        }

        public IList<EmpresaDTO> GetStartWithNome(string termo)
        {
            IEnumerable<Empresa> empresas = _uow.EmpresaRepository.GetStartWithNome(termo);
            return EmpresaMapper.MapEmpresaToEmpresaDTO(empresas);
        }

        public IdentityResult Insert(CriarEmpresaDTO criarEmpresaDTO)
        {
            // Vai ser uma combinação do insert do serviço com o insert em usuario service.

            // Criando a empresa.
            int idEmpresa = Insert(criarEmpresaDTO.empresaDTO);

            criarEmpresaDTO.criarUsuarioDTO.IdEmpresaLogada = idEmpresa;

            // Criando o usuário.
            return _usuarioService.Insert(criarEmpresaDTO.criarUsuarioDTO);
        }

        public int Insert(EmpresaDTO empresa)
        {
            var empresaToInsert = new Empresa()
            {
                CNPJ = empresa.CNPJ,
                Nome = empresa.Nome,
                Icone = "u32h4832h84814328",
                Situacao = ((int) empresa.Situacao) == 1 ? SituacaoEmpresa.ATIVA : SituacaoEmpresa.DESATIVA,
                LimiteLicencas = empresa.LimiteLicencas
            };
            _uow.EmpresaRepository.Insert(empresaToInsert);
            _uow.SaveChanges();
            return empresaToInsert.Id;
        }

        public void Update(EmpresaDTO empresa)
        {
            var empresaToUpdate = _uow.EmpresaRepository.GetByID(empresa.Id);

            empresaToUpdate.CNPJ = empresa.CNPJ;
            empresaToUpdate.Nome = empresa.Nome;
            empresaToUpdate.Situacao = ((int)empresa.Situacao) == 1 ? SituacaoEmpresa.ATIVA : SituacaoEmpresa.DESATIVA;
            empresaToUpdate.LimiteLicencas = empresa.LimiteLicencas;

            _uow.EmpresaRepository.Update(empresaToUpdate);
            _uow.SaveChanges();
        }
    }
}
