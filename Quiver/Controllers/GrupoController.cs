using Quiver.Filters;
using Quiver.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Quiver.Service.Interfaces;
using Quiver.Mappers;
using Quiver.DTO.Grupo;

namespace Quiver.Controllers
{
    public class GrupoController : BaseController
    {
        public IGrupoService _grupoService;
        public IFormularioService _formularioService;

        public GrupoController(IUsuarioService usuarioService, IGrupoService grupoService, IFormularioService formularioService) : base(usuarioService)
        {
            _grupoService = grupoService;
            _formularioService = formularioService;
        }

        // GET: Grupo
        public ActionResult Index()
        {
            ViewBag.CoresClassificacao = GrupoVM.CorClassificacao.Select(c => new SelectListItem()
            {
                Value = c
            });
            return View();
        }

        public ActionResult Tabela(string termo = "")
        {
            var grupos = _grupoService.GetAtivosByEmpresaAndStartWithNome(_user.EmpresaId, termo);
            return PartialView("_Tabela", grupos);
        }

        #region Manipular Grupo
        [HttpGet]
        [Authorization(Id = "grupoId")]
        public ActionResult Manipular(int grupoId = 0)
        {
            if (grupoId != 0)
            {
                var grupo = _grupoService.GetById(grupoId);
                var grupoVM = GrupoMapper.MapGrupoDTOToGrupoVM(grupo);

                return Json(grupoVM, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult();
        }

        [HttpPost]
        [Authorization(Id = "grupoVM.Id")]
        public ActionResult Manipular(GrupoVM grupoVM)
        {
            if (ModelState.IsValid)
            {
                GrupoDTO grupo = GrupoMapper.MapGrupoVMToGrupoDTO(grupoVM);
                
                if (grupoVM.Id == null)
                {
                    grupo.IdEmpresa = _user.EmpresaId;
                    _grupoService.Insert(grupo);
                }
                else
                {
                    _grupoService.Update(grupo);
                }
                return Json(new { ok = true });
            }
            return PartialView("_Manipular", grupoVM);
        }

        [HttpPost]
        [Authorization(Id = "grupoId")]
        public ActionResult Remover(int grupoId)
        {
            _grupoService.Delete(grupoId);
            return Json(new { ok = true });
        }

        #endregion


        [HttpGet]
        [Authorization(Id = "grupoId")]
        public ActionResult ManipularFormulariosGrupo(int grupoId = 0)
        {
            if (grupoId != 0)
            {
                var grupo = _grupoService.GetById(grupoId);
                var grupoVM = GrupoMapper.MapGrupoDTOToGrupoVM(grupo);
                return Json(grupoVM, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult();
        }

        [HttpPost]
        [Authorization(Id = "grupoFormulariosVM.Id")]
        public ActionResult ManipularFormulariosGrupo(GrupoFormulariosVM grupoFormulariosVM)
        {
            if (ModelState.IsValid)
            {
                grupoFormulariosVM.Formularios = grupoFormulariosVM.Formularios.Where(f => f.Selected == "on").ToArray();
                var grupoDTO = GrupoMapper.MapGrupoFormulariosVMToGrupoDTO(grupoFormulariosVM);
                _formularioService.AtualizarGrupo(grupoDTO);
                return Json(new { ok = true });
            }
            return null;
        }

        [HttpGet]
        public Boolean ExisteGrupo()
        {
            int qtdGrupo = _grupoService.GetAtivosByEmpresa(_user.EmpresaId).Count();
            return (qtdGrupo == 0) ? false : true;
        }
    }
}