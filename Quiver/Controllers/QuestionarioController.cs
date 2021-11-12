using AutoMapper;
using Quiver.Core.Models;
using Quiver.Filters;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quiver.Service.Interfaces;
using Quiver.Mappers;
using Quiver.DTO.Questionario;

namespace Quiver.Controllers
{
    public class QuestionarioController : BaseController
    {
        public IFormularioService _formularioService;
        private IGrupoService _grupoService;

        public QuestionarioController(IUsuarioService usuarioService, IFormularioService formularioService, IGrupoService grupoService) : base(usuarioService)
        {
            _formularioService = formularioService;
            _grupoService = grupoService;
        }

        // GET: Questionario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tabela(string termo = "")
        {
            var formularios = FormularioMapper.MapQuestionarioDTOToQuestionarioRowVM(_formularioService.GetQuestionariosAtivosByEmpresaAndStartWithNome(_user.EmpresaId, termo));
            return PartialView("_Tabela", formularios);
        }

        [HttpGet]
        [Authorization(Id = "questionarioId")]
        public ActionResult Manipular(int questionarioId = 0)
        {
            if (questionarioId != 0)
            {
                var questionarioVM = FormularioMapper.MapQuestionarioDTOToQuestionarioVM(_formularioService.GetAtivoById(questionarioId));
                questionarioVM.Questoes = questionarioVM.Questoes.OrderBy(m => m.Ordem).ToList();

                return View("Manipular", questionarioVM);
            }
            return View("Manipular", new QuestionarioVM());
        }

        [HttpPost]
        [Authorization(Id = "questionarioVM.Id")]
        public ActionResult Manipular(QuestionarioVM questionarioVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    QuestionarioDTO questionario = FormularioMapper.MapQuestionarioVMToQuestionarioDTO(questionarioVM);
                    questionario.IdEmpresa = _user.EmpresaId;
                    _formularioService.Insert(questionario);
                    return Json(new { ok = true });
                }
                catch (Exception e)
                {
                    return Json(new { ok = false });
                }
            }
            ViewBag.Grupos = GetGrupos();

            return View("Manipular", questionarioVM);
        }

        [HttpPost]
        [Authorization(Id = "questionarioID")]
        public ActionResult Excluir(int questionarioID = 0)
        {
            _formularioService.Delete(questionarioID);
            return Json(new { ok = true });
        }

        private SelectList GetGrupos()
        {
            var listaGrupos = _grupoService.GetAtivosByEmpresa(_user.EmpresaId);

            listaGrupos.Insert(0, new DTO.Grupo.GrupoDTO() { Id = 0, Nome = "Selecione o Grupo..." });

            return new SelectList(listaGrupos, "Id", "Nome");
        }

        [HttpGet]
        public ActionResult GetTodosPertencentesAoGrupo(int grupoId = 0)
        {
            // Aqui pegamos todos os formulários e marcamos aqueles que pertencem ou não ao grupo.
            if (grupoId != 0)
            {
                List<QuestionarioGrupoVM> questionariosDoGrupo =
                    _formularioService.GetQuestionariosAtivosByEmpresaAndStartWithNome(_user.EmpresaId, "")
                                      .Select(f => new QuestionarioGrupoVM
                                      {
                                          Id = f.Id,
                                          Nome = f.Nome,
                                          Marcado = f.Grupos.Any(g => g.IdGrupo == grupoId)
                                      }).ToList();

                return Json(questionariosDoGrupo, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult();
        }
    }
}