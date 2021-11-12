using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quiver.Filters;
using System.Collections;
using Quiver.Common.Utils;
using Quiver.Service.Interfaces;
using Quiver.Mappers;
using Quiver.DTO.Perfil;
using Quiver.DTO.Usuario;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;

namespace Quiver.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly IAgendaService _agendaService;

        public UsuarioController(IUsuarioService usuarioService, IAgendaService agendaService)
            : base(usuarioService)
        {
            _agendaService = agendaService;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pesquisar(string termo = "")
        {
            var usuarios = _usuarioService.GetAtivosByEmpresaAndStartWithNome(_user.EmpresaId, termo);

            IList<UsuarioRow> usuariosRows = UsuarioMapper.MapUsuarioDTOToUsuarioRow(usuarios, _user.UsuarioId);
            return PartialView("_Tabela", usuariosRows);
        }

        [HttpGet]
        [Authorization(Id = "usuarioId")]
        public ActionResult Remover(string usuarioId)
        {
            if (_user.UsuarioId != usuarioId)
            {
                _usuarioService.Delete(usuarioId);
                return Json(new { ok = true });
            } else
            {
                return Json(new { ok = false });
            }            
        }

        public ActionResult Perfil(string usuarioId = null)
        {
            if (usuarioId != null)
            {
                UsuarioDTO usuarioDB = _usuarioService.GetById(usuarioId);
                if (usuarioDB != null)
                {
                    UsuarioPerfilVM usuarioPerfilVM = UsuarioMapper.MapUsuarioDTOToUsuarioPerfilVM(usuarioDB);
                    IList<AvaliacaoDTO> avaliacoes = _agendaService.GetAvaliacoesByUsuario(usuarioId);
                    IList<AvaliacaoVM> avaliacoesVM = AgendaMapper.MapAvaliacaoDTOToAvaliacaoVM(avaliacoes);
                    usuarioPerfilVM.avaliacoes = avaliacoesVM;
                    return View("Perfil", usuarioPerfilVM);
                }
                return RedirectToAction("404", "Usuario");
            }
            return RedirectToAction("404", "Usuario");
        }

        public ActionResult PerfilAvaliacoes(string termo = "", string usuarioId = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DateTime dtInicial = (dataInicial != null) ? dtInicial = (DateTime)dataInicial : dtInicial = DateTime.MinValue;
            DateTime dtFinal = (dataFinal != null) ? dtFinal = (DateTime)dataFinal : dtFinal = DateTime.MaxValue;

            dtInicial = new DateTime(dtInicial.Year, dtInicial.Month, dtInicial.Day, 0, 0, 0);
            dtFinal = new DateTime(dtFinal.Year, dtFinal.Month, dtFinal.Day, 23, 59, 59);

            IList<AvaliacaoDTO> avaliacoes = _agendaService.GetAvaliacoesByUsuarioAndPeriodo(usuarioId, dtInicial, dtFinal);
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

        public ActionResult GetEvolutivo(string usuarioId = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DateTime dtInicial = (dataInicial != null) ? dtInicial = (DateTime)dataInicial : dtInicial = DateTime.MinValue;
            DateTime dtFinal = (dataFinal != null) ? dtFinal = (DateTime)dataFinal : dtFinal = DateTime.MaxValue;

            dtInicial = new DateTime(dtInicial.Year, dtInicial.Month, dtInicial.Day, 0, 0, 0);
            dtFinal = new DateTime(dtFinal.Year, dtFinal.Month, dtFinal.Day, 23, 59, 59);

            // Pego as inspeções finalizadas desse usuário
            IList<AvaliacaoDTO> avaliacoesDB = _agendaService.GetAvaliacoesFinalizadasComPontosByUsuarioAndPeriodo(usuarioId, dtInicial, dtFinal); //_uow.AvaliacaoRepository.Get(filter: a => a.IdUsuario == usuarioId && a.Situacao == SituacaoAvaliacao.AVALIADO && a.DataFim >= dtInicial && a.DataFim <= dtFinal).OrderBy(a => a.DataFim);
        

            if (avaliacoesDB.Count() > 0)
            {
                var vm = new ArrayList();

                DateTime dataPrimeiraAvaliacao = avaliacoesDB.FirstOrDefault().DataFim ?? TZUtil.GetDataHoraDeBrasilia();
                dataPrimeiraAvaliacao = new DateTime(dataPrimeiraAvaliacao.Year, dataPrimeiraAvaliacao.Month, dataPrimeiraAvaliacao.Day, 0, 0, 0);
                DateTime dataUltimaAvaliacao = avaliacoesDB.LastOrDefault().DataFim ?? TZUtil.GetDataHoraDeBrasilia();
                dataUltimaAvaliacao = new DateTime(dataUltimaAvaliacao.Year, dataUltimaAvaliacao.Month, dataUltimaAvaliacao.Day, 0, 0, 0);

                List<PontoEvolutivoGeralVM> pontosNoGraficoDoUsuario = AgendaMapper.MapAvaliacaoDTOToPontoEvolutivoGeralVM(avaliacoesDB, "efetividade", dataPrimeiraAvaliacao, dataUltimaAvaliacao);

                var myData = pontosNoGraficoDoUsuario.Select(a => new object[]{a.TimeStampDataExecutada,
                                        getAllBeforeChar(a.EfetividadeMedia, '.') });
                var linha = new
                {
                    label = "Inspeções finalizadas",
                    data = myData,
                    color = "#1B5E20",
                    avaliacoesPonto = pontosNoGraficoDoUsuario.Select(p => new object[] {
                                    p.avaliacoesDia
                                    }),
                };

                vm.Add(linha);
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetQuantitativo(string usuarioId = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            DateTime dtInicial = (dataInicial != null) ? dtInicial = (DateTime)dataInicial : dtInicial = DateTime.MinValue;
            DateTime dtFinal = (dataFinal != null) ? dtFinal = (DateTime)dataFinal : dtFinal = DateTime.MaxValue;

            dtInicial = new DateTime(dtInicial.Year, dtInicial.Month, dtInicial.Day, 0, 0, 0);
            dtFinal = new DateTime(dtFinal.Year, dtFinal.Month, dtFinal.Day, 23, 59, 59);

            // Pego as inspeções finalizadas desse usuário
            IList<AvaliacaoDTO> avaliacoesDB = _agendaService.GetAvaliacoesFinalizadasByUsuarioAndPeriodo(usuarioId, dtInicial, dtFinal); //_uow.AvaliacaoRepository.Get(filter: a => a.IdUsuario == usuarioId && a.Situacao == SituacaoAvaliacao.AVALIADO && a.DataFim >= dtInicial && a.DataFim <= dtFinal).OrderBy(a => a.DataFim);

            if (avaliacoesDB.Count() > 0)
            {
                var vm = new ArrayList();

                DateTime dataPrimeiraAvaliacao = avaliacoesDB.FirstOrDefault().DataFim ?? TZUtil.GetDataHoraDeBrasilia();
                dataPrimeiraAvaliacao = new DateTime(dataPrimeiraAvaliacao.Year, dataPrimeiraAvaliacao.Month, dataPrimeiraAvaliacao.Day, 0, 0, 0);
                DateTime dataUltimaAvaliacao = avaliacoesDB.LastOrDefault().DataFim ?? TZUtil.GetDataHoraDeBrasilia();
                dataUltimaAvaliacao = new DateTime(dataUltimaAvaliacao.Year, dataUltimaAvaliacao.Month, dataUltimaAvaliacao.Day, 0, 0, 0);

                List<PontoEvolutivoGeralVM> pontosNoGraficoDoUsuario = AgendaMapper.MapAvaliacaoDTOToPontoQuantidadeGeralVM(avaliacoesDB, dataPrimeiraAvaliacao, dataUltimaAvaliacao);

                int MaiorQtdAvaliacoes = 0;  // Serve para o tamanho do EixoY do gráfico
                MaiorQtdAvaliacoes = pontosNoGraficoDoUsuario.OrderByDescending(a => a.avaliacoesDia.Count()).FirstOrDefault().avaliacoesDia.Count;

                ViewBag.alturaEixoY = MaiorQtdAvaliacoes + 1;

                var myData = pontosNoGraficoDoUsuario.Select(a => new object[]{a.TimeStampDataExecutada,
                                        a.QtdAvaliacoes });

                var linha = new
                {
                    label = "Inspeções finalizadas",
                    data = myData,
                    color = "#1B5E20",
                    avaliacoesPonto = pontosNoGraficoDoUsuario.Select(p => new object[] {
                                    p.avaliacoesDia
                                    }),
                    eixoY = MaiorQtdAvaliacoes + 1
                };
                vm.Add(linha);
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            var roles = new List<PerfilDTO>();
            roles.Add(new PerfilDTO() { Id = "0", Nome = "Selecione..." });
            roles.AddRange(_usuarioService.GetPerfisExcetoAdministrador());
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        private bool AvaliacaoPossuiNaoConformidade(ICollection<AvaliacaoQuestionarioGrupoDTO> questionarios)
        {
            // Verificar se existe em algum item com NÃO CONFORMIDADE. 
            foreach (var formulario in questionarios)
            {
                foreach (var questao in formulario.QuestionarioGrupo.Questionario.Questoes)
                {
                    foreach (var item in questao.Itens)
                    {
                        if (item.Alternativa.NaoConformidade == true)
                        {
                            return true;  // Encontrei uma não conformidade
                        }
                    }
                }
            }
            return false;
        }



    }
}