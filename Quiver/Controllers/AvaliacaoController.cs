using System;
using System.Linq;
using System.Web.Mvc;
using Quiver.Service.Interfaces;
using Quiver.DTO.Enum;
using Quiver.Mappers;
using Quiver.Common.Utils;

namespace Quiver.Controllers
{
    public class AvaliacaoController : BaseController
    {

        private readonly IUnidadeService _unidadeService;
        private readonly IGrupoService _grupoService;
        private readonly IAgendaService _agendaService;

        public AvaliacaoController(IUsuarioService usuarioService, IAgendaService agendaService, IUnidadeService unidadeService, IGrupoService grupoService) : base(usuarioService)
        {
            _agendaService = agendaService;
            _unidadeService = unidadeService;
            _grupoService = grupoService;
        }

        // GET: Avaliacao
        public ActionResult Index()
        {
            var listaUnidades = _unidadeService.GetAtivosByEmpresa(_user.EmpresaId);
            ViewBag.Unidades = new SelectList(listaUnidades, "Id", "Nome").ToList();

            var listaUsuarios = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId);
            ViewBag.Usuarios = new SelectList(listaUsuarios, "Id", "UserName").ToList();

            var listaGrupos = _grupoService.GetAtivosByEmpresa(_user.EmpresaId);
            ViewBag.Grupos = new SelectList(listaGrupos, "Id", "Nome").ToList();

            return View();
        }

        public ActionResult GetAvaliacoes(DateTime dataInicial, DateTime dataFinal,
             int unidadesAvaliacaoID = 0, string usuariosAvaliacaoID = "0",
             int gruposAvaliacaoID = 0 , bool? filtroAgendada = null, bool? filtroConformidade = null)
        {
            dataInicial = new DateTime(dataInicial.Year, dataInicial.Month, dataInicial.Day, 0, 0, 0);
            dataFinal = new DateTime(dataFinal.Year, dataFinal.Month, dataFinal.Day, 23, 59, 59);

            var avaliacoesDB = _agendaService.GetAvaliacaoByFilter
                (_user.EmpresaId, dataInicial, dataFinal, 
                unidadesAvaliacaoID, usuariosAvaliacaoID, 
                gruposAvaliacaoID, filtroAgendada, filtroConformidade);

            var dataAtual = TZUtil.GetDataDeBrasilia();

            ViewBag.qtdFinalizadas = avaliacoesDB.Where(a => a.Situacao == SituacaoAvaliacao.AVALIADO).Count();
            ViewBag.qtdAndamento = avaliacoesDB.Where(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.DataProgramada >= dataAtual).Count();
            ViewBag.qtdAtrasadas = avaliacoesDB.Where(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.DataProgramada < dataAtual).Count();
            ViewBag.qtdAgendadas = avaliacoesDB.Where(a => a.Situacao == SituacaoAvaliacao.AVALIADO && a.Agendada == true).Count();
            ViewBag.qtdNaoAgendadas = avaliacoesDB.Where(a => a.Situacao == SituacaoAvaliacao.AVALIADO && a.Agendada != true).Count();

            var avaliacoesVM = AgendaMapper.MapAvaliacaoDTOToAvaliacaoVM(avaliacoesDB.ToList());

            return PartialView("_Tabela", avaliacoesVM);
        }

    }
}

