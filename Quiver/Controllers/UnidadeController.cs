using AutoMapper;
using Quiver.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quiver.Models;
using Quiver.Mappers;
using Quiver.Service.Interfaces;
using Quiver.DTO.Unidade;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;

namespace Quiver.Controllers
{
    public class UnidadeController : BaseController
    {

        public IUnidadeService _unidadeService;
        private readonly IAgendaService _agendaService;


        public UnidadeController(IUsuarioService usuarioService, IUnidadeService unidadeService, IAgendaService agendaService) 
            : base(usuarioService) {
            _unidadeService = unidadeService;
            _agendaService = agendaService;
        }

        // GET: Unidade
        public ActionResult Index()
        {
            return View();       
        }

        public ActionResult Pesquisar(string termo = "")
        {
            var unidades = _unidadeService.GetAtivosByEmpresaAndStartWithNome(_user.EmpresaId, termo);
            return PartialView("_Tabela", unidades);
        }

        [HttpGet]
        [Authorization(Id = "unidadeId")]
        public ActionResult Manipular(int unidadeId = 0)
        {
            if (unidadeId != 0)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UnidadeDTO, UnidadeVM>();
                });
                IMapper mapper = config.CreateMapper();
                var unidade = _unidadeService.GetById(unidadeId);
                return Json(mapper.Map<UnidadeDTO, UnidadeVM>(unidade), JsonRequestBehavior.AllowGet);
            }
            return new JsonResult();
        }

        [HttpPost]
        [Authorization(Id = "unidadeVM.Id")]
        public ActionResult Manipular(UnidadeVM unidadeVM)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UnidadeVM, UnidadeDTO>();
                });
                IMapper mapper = config.CreateMapper();
                var unidade = mapper.Map<UnidadeVM, UnidadeDTO>(unidadeVM);
                if (unidadeVM.Id == null)
                {
                    unidade.IdEmpresa = _user.EmpresaId;
                    _unidadeService.Insert(unidade);
                }
                else
                {
                    _unidadeService.Update(unidade);
                }
                return Json(new { ok = true });
            }
            return PartialView("_Manipular", unidadeVM);
        }

        [HttpPost]
        [Authorization(Id = "unidadeID")]
        public ActionResult Excluir(int unidadeID = 0)
        {
            _unidadeService.Delete(unidadeID);
            return Json(new { ok = true });
        }

        [Authorization(Id = "unidadeId")]
        public ActionResult Perfil(int unidadeId = 0)
        {
            if (unidadeId != 0)
            {
                UnidadeDTO unidaddeDB = _unidadeService.GetById(unidadeId);
               
                if (unidaddeDB != null)
                {                    
                    UnidadePerfilVM unidadePerfilVM = UnidadeMapper.MapUnidadeDTOToUnidadePerfilVM(unidaddeDB);
                    IList<AvaliacaoDTO> avaliacoes = _agendaService.GetAvaliacoesByUnidade(unidadeId);
                    IList<AvaliacaoVM> avaliacoesVM = AgendaMapper.MapAvaliacaoDTOToAvaliacaoVM(avaliacoes);
                    unidadePerfilVM.avaliacoes = avaliacoesVM;
                    return View("Perfil", unidadePerfilVM);
                }
            }
            return RedirectToAction("404", "Usuario");
        }

        public ActionResult PerfilAvaliacoes(string termo = "", string unidadeId = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DateTime dtInicial = (dataInicial != null) ? dtInicial = (DateTime)dataInicial : dtInicial = DateTime.MinValue;
            DateTime dtFinal = (dataFinal != null) ? dtFinal = (DateTime)dataFinal : dtFinal = DateTime.MaxValue;

            dtInicial = new DateTime(dtInicial.Year, dtInicial.Month, dtInicial.Day, 0, 0, 0);
            dtFinal = new DateTime(dtFinal.Year, dtFinal.Month, dtFinal.Day, 23, 59, 59);

            IList<AvaliacaoDTO> avaliacoes = _agendaService.GetAvaliacoesByUnidadeAndPeriodo(unidadeId, dtInicial, dtFinal);

            IList<AvaliacaoVM> avaliacoesVM = AgendaMapper.MapAvaliacaoDTOToAvaliacaoVM(avaliacoes);


            IEnumerable<AvaliacaoVM> atrasadas = avaliacoesVM.Where(a => a.Situacao == "ATRASADA");
            IEnumerable<AvaliacaoVM> finalizadas = avaliacoesVM.Where(a => a.Situacao == "AVALIADO");
            IEnumerable<AvaliacaoVM> emAndamento = avaliacoesVM.Where(a => a.Situacao == "ANDAMENTO");
            // Modificando valor do campo Situação.
            atrasadas.ToList().ForEach(a => a.Situacao = "ATRASADA");
            finalizadas.ToList().ForEach(a => a.Situacao = "AVALIADO");
            emAndamento.ToList().ForEach(a => a.Situacao = "ANDAMENTO");

            ViewBag.qtdAtrasadas = atrasadas.Count();
            ViewBag.qtdFinalizadas = finalizadas.Count();
            ViewBag.qtdAndamentos = emAndamento.Count();
            ViewBag.qtdAgendadas = avaliacoesVM.Where(a => a.Agendada).Count();
            ViewBag.qtdNaoAgendadas = avaliacoesVM.Where(a => !a.Agendada).Count();

            return PartialView("_PerfilTabelaAvaliacoes", avaliacoesVM);
        }


    }
}
