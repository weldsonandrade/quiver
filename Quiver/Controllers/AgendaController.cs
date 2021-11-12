using Quiver.Core.Models;
using Quiver.Filters;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quiver.Service.Interfaces;
using Quiver.DTO.Unidade;
using Quiver.Mappers;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.Usuario;
using Quiver.DTO.Grupo;

namespace Quiver.Controllers
{
    public class AgendaController : BaseController
    {
        private IUnidadeService _unidadeService;
        public IAgendaService _agendaService;
        private IGrupoService _grupoService;

        public AgendaController(IUsuarioService usuarioService, IUnidadeService unidadeService, IAgendaService agendaService, IGrupoService grupoService) : base(usuarioService)
        {
            _unidadeService = unidadeService;
            _agendaService = agendaService;
            _grupoService = grupoService;
        }

        public ActionResult Index()
        {
            ViewBag.Unidades = GetSelectListUnidades();
            ViewBag.Usuarios = GetSelectListInspetores();

            return View();
        }

        [Authorization(Id = "agendamentoId")]
        public ActionResult AvaliacaoFinalizada(int agendamentoId = 0)
        {
            var avaliacao = _agendaService.GetAvaliacaoAvaliadaById(agendamentoId);
            if (avaliacao != null)
            {
                var avaliacaoVM = AgendaMapper.MapAvaliacaoDTOToAgendamentoVM(avaliacao);
                return View(avaliacaoVM);
            }
            return RedirectToAction("404");
        }

        #region Manipular

        [HttpGet]
        [Authorization(Id = "agendamentoId")]
        public JsonResult Manipular(int agendamentoId = 0)
        {
            ViewBag.editar = agendamentoId == 0 ? false : true;
            if (agendamentoId != 0)
            {
                var agendamentoVM = AgendaMapper.MapAvaliacaoDTOToAgendamentoVM(_agendaService.GetAvaliacaoById(agendamentoId));
                return Json(agendamentoVM, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult();
        }

        [HttpPost]
        [Authorization(Id = "agendamentoVM.Id")]
        public ActionResult Manipular(AgendamentoVM agendamentoVM)
        {
            if (ModelState.IsValid)
            {
                if (agendamentoVM.Id == null)
                {
                    if (agendamentoVM.Recorrencia == null || !agendamentoVM.Recorrencia.DataFinal.HasValue)
                    {
                        var avaliacao = AgendaMapper.MapAgendamentoVMToAvaliacaoDTO(agendamentoVM);
                        _agendaService.Insert(avaliacao);
                    }
                }
                else
                {
                    var avaliacao = AgendaMapper.MapAgendamentoVMToAvaliacaoDTO(agendamentoVM);
                    _agendaService.Update(avaliacao);
                }
                return Json(new { ok = true });
            }

            return PartialView("_Manipular", agendamentoVM);
        }

        #endregion

        [HttpGet]
        public JsonResult GetUnidadesAndGruposAndInspetores()
        {
            List<SelectListItem> unidades = GetSelectListUnidades();
            List<SelectListItem> grupos = GetSelectListGrupos();
            List<SelectListItem> inspetores = GetSelectListInspetores();
            return Json(new { unidades = unidades, grupos = grupos, inspetores = inspetores }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Id = "agendamentoID")]
        public ActionResult TrocarData(int agendamentoID, DateTime dataProgramada)
        {
            try
            {
                var avaliacao = new AvaliacaoDTO() { Id = agendamentoID, DataProgramada = dataProgramada };
                _agendaService.UpdateDataProgramada(avaliacao);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, mensagem = e.Message });
            }
        }

        [HttpGet]
        public ActionResult ListarEventos(string start, string end, int unidadeId = 0, string usuarioId = "")
        {
            DateTime dataInicial = Convert.ToDateTime(start);
            DateTime dataFinal = Convert.ToDateTime(end);

            IList<EventoDTO> eventosDTO = _agendaService.GetAvaliacaoByEmpresaAndPeriodoAndUnidadeAndUsuario(_user.EmpresaId, dataInicial, dataFinal, unidadeId, usuarioId);
          
            var eventos = AgendaMapper.MapEventoDTOToEvento(eventosDTO);
            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Id = "agendamentoID")]
        public ActionResult Excluir(int agendamentoID = 0, int excluirRecorrentes = 0)
        {
            try
            {
                _agendaService.Delete(agendamentoID);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, mensagem = e.Message });
            }
        }

        public ActionResult GetClassificacaoGrupoAvaliado(int grupoId)
        {
            var grupoDB = _grupoService.GetById(grupoId);
            IList<ClassificacaoViewModels> classificacoes = GrupoMapper.MapClassificacaoDTOTOClassificacaoViewModels(grupoDB.Classificacoes);
            return PartialView("_ClassificacaoGrupo", classificacoes);
        }

        #region Recorrencia

        public static List<DateTime> ObterRecorrencias(AgendamentoVM.RecorrenciaVM recorrencia)
        {
            List<DateTime> datasRecorrentes = new List<DateTime>();

            if (recorrencia.Frequencia == FrequenciaRecorrencia.Customizado)
            {
                datasRecorrentes = recorrencia.DatasCustomizadas;
            }
            else
            {
                int quantidadeAdicionada = 0;

                DateTime proximaData = (DateTime)recorrencia.DataInicial;

                bool repetePorQuantidade = recorrencia.QuantidadeDeRepeticoes != 0;

                List<DayOfWeek> diasDaSemanaRecorrencia = new List<DayOfWeek>();
                foreach (char dia in recorrencia.DiasDaSemana.ToString())
                {
                    switch (dia)
                    {
                        case '1':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Monday);
                            break;
                        case '2':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Tuesday);
                            break;
                        case '3':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Wednesday);
                            break;
                        case '4':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Thursday);
                            break;
                        case '5':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Friday);
                            break;
                        case '6':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Saturday);
                            break;
                        case '7':
                            diasDaSemanaRecorrencia.Add(DayOfWeek.Sunday);
                            break;
                    }
                }

                while ((repetePorQuantidade && quantidadeAdicionada <= recorrencia.QuantidadeDeRepeticoes) ||
                        (!repetePorQuantidade && proximaData <= recorrencia.DataFinal))
                {
                    switch (recorrencia.Frequencia)
                    {
                        case FrequenciaRecorrencia.Diario:
                            datasRecorrentes.Add(proximaData);
                            quantidadeAdicionada++;

                            proximaData = proximaData.AddDays((double)recorrencia.IntervaloDias);
                            break;

                        case FrequenciaRecorrencia.Semanal:
                            if (diasDaSemanaRecorrencia.Contains(proximaData.DayOfWeek))
                            {
                                datasRecorrentes.Add(proximaData);
                                quantidadeAdicionada++;
                            }

                            proximaData = proximaData.AddDays(1);
                            break;

                        case FrequenciaRecorrencia.Mensal:
                            if (recorrencia.DiasDoMes.Contains(proximaData.Day))
                            {
                                datasRecorrentes.Add(proximaData);
                                quantidadeAdicionada++;
                            }

                            proximaData = proximaData.AddDays(1);
                            break;
                    }
                }
            }

            return datasRecorrentes;
        }

        #endregion

        private List<SelectListItem> GetSelectListUnidades()
        {
            IList<UnidadeDTO> listaUnidades = _unidadeService.GetAtivosByEmpresa(_user.EmpresaId);
            listaUnidades.Insert(0, new UnidadeDTO() { Id = 0, Nome = "Selecione a Unidade..." });
            return new SelectList(listaUnidades, "Id", "Nome").ToList();
        }

        private List<SelectListItem> GetSelectListInspetores()
        {
            var listaUsuarios = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId);
            listaUsuarios.Insert(0, new UsuarioDTO() { Id = "", UserName = "Selecione o usuário..." });
           return new SelectList(listaUsuarios, "Id", "UserName").ToList();
        }

        private List<SelectListItem> GetSelectListGrupos()
        {
            IList<GrupoDTO> listaGrupos = _grupoService.GetAtivosByEmpresaWithAtLeastOneQuestionario(_user.EmpresaId);
            listaGrupos.Insert(0, new GrupoDTO() { Id = 0, Nome = "Selecione o Grupo..." });

            return new SelectList(listaGrupos, "Id", "Nome").ToList();
        }
    }
}