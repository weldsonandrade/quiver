using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quiver.Common.Utils;
using Quiver.DTO.PlanoAcao;
using Quiver.Mappers;
using Quiver.Models;
using Quiver.Filters;
using Quiver.DTO.Enum;

namespace Quiver.Controllers
{
    public class PlanoAcaoController : BaseController
    {
        private readonly IUnidadeService _unidadeService;
        public readonly IPlanoAcaoService _planoAcaoService;

        public PlanoAcaoController(IUsuarioService usuarioService, IUnidadeService unidadeService, IPlanoAcaoService planoAcaoService) : base(usuarioService)
        {
            _unidadeService = unidadeService;
            _planoAcaoService = planoAcaoService;
        }

        // GET: PlanoAcao
        [HttpGet]
        public ActionResult Index()
        {
            LoadViewBagsToIndexPage();
            return View();
        }

        [HttpPost]
        public ActionResult Filtrar(string emailResponsavel, string IdUnidades, string IdUsuarios)
        {
            List<int> unidadesIds = new List<int>();
            if (!string.IsNullOrEmpty(IdUnidades))
            {
                unidadesIds = IdUnidades.Split(',').Select(int.Parse).ToList();
            }
            List<string> usuariosIds = new List<string>();
            if (!string.IsNullOrEmpty(IdUsuarios))
            {
                usuariosIds = IdUsuarios.Split(',').ToList();
            }

            IList<PlanoAcaoDTO> planosAcao = _planoAcaoService.GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(_user.EmpresaId, emailResponsavel, unidadesIds, usuariosIds);

            PlanoAcaoCardVM planosAcaoVM = PlanoAcaoMapper.MapPlanoAcaoDTOToPlanoAcaoCardVM(planosAcao);

            return PartialView("_Card", planosAcaoVM);
        }

        [HttpGet]
        [Authorization(Id = "idPlanoAcao")]
        public ActionResult Manipular(int idPlanoAcao = 0)
        {
            PlanoAcaoDTO planoAcaoDTO = _planoAcaoService.GetById(idPlanoAcao);
            PlanoAcaoVM planoAcaoVM = PlanoAcaoMapper.MapPlanoAcaoDTOToPlanoAcaoVM(planoAcaoDTO);
            return View(planoAcaoVM);
        }

        [HttpPost]
        [Authorization(Id = "idPlanoAcao")]
        public ActionResult Manipular(PlanoAcaoVM planoAcaoVM)
        {
            if (ModelState.IsValid)
            {
                PlanoAcaoDTO planoAcaoDTO = PlanoAcaoMapper.MapPlanoAcaoVMTOPlanoAcaoDTO(planoAcaoVM);
                _planoAcaoService.Update(planoAcaoDTO);

                return Json(new { ok = true });
            }
            planoAcaoVM.Origem = PlanoAcaoMapper.MapPlanoAcaoDTOToPlanoAcaoVM(_planoAcaoService.GetById(planoAcaoVM.Id)).Origem; ;
            return View(planoAcaoVM);
        }

        [HttpGet]
        [Authorization(Id = "idPlanoAcao")]
        public JsonResult Cancelar(int idPlanoAcao)
        {
            try
            {
                _planoAcaoService.Cancelar(idPlanoAcao);
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { ok = false }, JsonRequestBehavior.AllowGet);
            }            
        }

        private void LoadViewBagsToIndexPage()
        {
            var listaUnidades = _unidadeService.GetAtivosByEmpresa(_user.EmpresaId);
            ViewBag.Unidades = new SelectList(listaUnidades, "Id", "Nome").ToList();

            var listaUsuarios = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId);
            ViewBag.Usuarios = new SelectList(listaUsuarios, "Id", "UserName").ToList();

            var todosPlanosAcaoDaEmpresa = _planoAcaoService.GetByEmpresa(_user.EmpresaId);
            PlanoAcaoCardVM planosAcaoCardVM = PlanoAcaoMapper.MapPlanoAcaoDTOToPlanoAcaoCardVM(todosPlanosAcaoDaEmpresa);
            ViewBag.PlanosAcao = planosAcaoCardVM;
        }

    }
}