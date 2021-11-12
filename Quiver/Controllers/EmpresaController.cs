using Quiver.Models;
using System.Linq;
using System.Web.Mvc;
using Quiver.Filters;
using Quiver.Mappers;
using Quiver.Service.Interfaces;

namespace Quiver.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpresaController : BaseController
    {
        private IEmpresaService _empresaService;

        public EmpresaController(IUsuarioService usuarioService, IEmpresaService empresaService)
            : base(usuarioService)
        {
            this._empresaService = empresaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pesquisar(string termo = "")
        {
            var empresas = _empresaService.GetStartWithNome(termo)
                .OrderBy(u => u.Nome)
                .Select(e => new EmpresaRowVM()
                {
                    Id = e.Id,
                    Nome = e.Nome, 
                    Cnpj = e.CNPJ,
                    Situacao = e.Situacao == Quiver.DTO.Enum.SituacaoEmpresa.ATIVA ? "Ativa" : "Não ativa",
                    LimiteLicencas = e.LimiteLicencas.ToString()
                });
            return PartialView("_Tabela", empresas);
        }

        [HttpGet]
        [Authorization(Id = "empresaId")]
        public ActionResult Manipular(int empresaId = 0)
        {
            if (empresaId != 0)
            {
                var empresa = _empresaService.GetById(empresaId);
                var empresaVM = EmpresaMapper.MapEmpresaDTOToEmpresaEditorVM(empresa);
                return Json(empresaVM, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Id = "empresaEditorVM.Id")]
        public ActionResult Manipular(EmpresaEditorVM empresaEditorVM)
        {
            if (ModelState.IsValid)
            {
                var empresa = EmpresaMapper.MapEmpresaEditorVMToEmpresaDTO(empresaEditorVM);
                if (empresaEditorVM.Id == null)
                {
                    _empresaService.Insert(empresa);
                } else
                {
                    _empresaService.Update(empresa);
                }
                return Json(new { ok = true });
            }
            return PartialView("_Manipular", empresaEditorVM);
        }
    }
}