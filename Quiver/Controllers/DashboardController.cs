using Quiver.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quiver.Common.Utils;
using Quiver.Service.Interfaces;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.Mappers;
using Quiver.DTO.Usuario;

namespace Quiver.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IAgendaService _agendaService;
        private readonly IUnidadeService _unidadeService;

        public DashboardController(IUsuarioService usuarioService, IAgendaService agendaService, IUnidadeService unidadeService) : base(usuarioService)
        {
            _agendaService = agendaService;
            _unidadeService = unidadeService;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getAvaliacoesDados()
        {
            var qtdDashboard = new quantitativoAvaliacoesDashboard();
            var totalAvaliacoesDB = _agendaService.GetAvaliacoesByEmpresa(_user.EmpresaId);
            var dataAtual = TZUtil.GetDataDeBrasilia();

            qtdDashboard.QtdFinalizadas = totalAvaliacoesDB.Where(a => a.Situacao == Quiver.DTO.Enum.SituacaoAvaliacao.AVALIADO).Count();
            qtdDashboard.QtdAtrasadas = totalAvaliacoesDB.Where(a => a.Situacao == Quiver.DTO.Enum.SituacaoAvaliacao.NAO_AVALIADO && a.DataProgramada < dataAtual).Count();
            qtdDashboard.QtdAndamentos = totalAvaliacoesDB.Count() - (qtdDashboard.QtdAtrasadas + qtdDashboard.QtdFinalizadas);
            qtdDashboard.QtdAgendadas = totalAvaliacoesDB.Where(a => a.Situacao == Quiver.DTO.Enum.SituacaoAvaliacao.AVALIADO && a.Agendada == true).Count();
            qtdDashboard.QtdNaoAgendadas = qtdDashboard.QtdFinalizadas - qtdDashboard.QtdAgendadas;
            qtdDashboard.QtdConformes = countConformeOrNaoConforme("Conforme", totalAvaliacoesDB.Where(a => a.Situacao == Quiver.DTO.Enum.SituacaoAvaliacao.AVALIADO));
            qtdDashboard.QtdComNaoConformidade = qtdDashboard.QtdFinalizadas - qtdDashboard.QtdConformes;

            return PartialView("_InformacoesGerais", qtdDashboard);
        }


        private int countConformeOrNaoConforme(string conforme, IEnumerable<AvaliacaoDTO> avaliacoesFinalizadas)
        {
            int qtd = 0;
            foreach (var avaliacao in avaliacoesFinalizadas)
            {
                if (eventoPossuiNaoConformidade(avaliacao.QuestionariosGrupo) == true && conforme == "NaoConforme")
                {
                    qtd++;
                }
                else if (eventoPossuiNaoConformidade(avaliacao.QuestionariosGrupo) == false && conforme == "Conforme")
                {
                    qtd++;
                }
            }
            return qtd;
        }

        public ActionResult AvaliacoesPorTipo(string tipo)
        {
            IEnumerable<AvaliacaoDTO> avaliacoesDB = null;
            var dataAtual = TZUtil.GetDataDeBrasilia();

            if (tipo == "Atrasadas")
            {
                avaliacoesDB = _agendaService.GetAvaliacoesAtrasadasByEmpresa(_user.EmpresaId);
            }
            else if (tipo == "Andamentos")
            {
                avaliacoesDB = _agendaService.GetAvaliacoesEmAndamentoByEmpresa(_user.EmpresaId);
            }

            if (avaliacoesDB != null && avaliacoesDB.Count() > 0)
            {
                var avaliacoesVM = AgendaMapper.MapAvaliacaoDTOToAvaliacaoVM(avaliacoesDB);

                return PartialView("_TabelaAvaliacoesTipo", avaliacoesVM.ToList());
            }


            return PartialView("_TabelaAvaliacoesTipo");
        }


        public ActionResult graficoQuantitativo(DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            var qtdUsuarios = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId).Count;

            var vmGrafico = new ArrayList();

            if (qtdUsuarios <= 5)
            {
                // Pegando apenas os usuário ligados a empresa que não esteja deletados e nem sejam administradores
                IList<UsuarioDTO> usuariosDB = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId);

                foreach (var usuario in usuariosDB)
                {
                    // Aplicando os filtros de tempo nas inspeções do usuário
                    var avaliacoesDB = _agendaService.GetAvaliacoesFinalizadasByUsuarioAndPeriodo(usuario.Id, (DateTime) dataInicial, (DateTime) dataFinal); // usuario.Avaliacoes.Where(a => a.DataFim >= dataInicial && a.DataFim <= dataFinal && a.Situacao == Core.Models.SituacaoAvaliacao.AVALIADO);
                    if (avaliacoesDB.Count() > 0)
                    {
                        vmGrafico.Add(getLinhasGraficoQTD(avaliacoesDB,"Quantitativo", usuario.Email));
                    }
                }

            }
            else
            {
                // Pegando apenas os usuário ligados a empresa que não esteja deletados e nem sejam administradores
                var unidadesDB = _unidadeService.GetAtivosByEmpresa(_user.EmpresaId);

                foreach (var unidade in unidadesDB)
                {
                    // Aplicando os filtros de tempo nas inspeções do usuário
                    var avaliacoesDB = _agendaService.GetAvaliacoesFinalizadasByUnidadeAndPeriodo(unidade.Id, (DateTime) dataInicial, (DateTime) dataFinal);
                    if (avaliacoesDB.Count() > 0)
                    {
                        vmGrafico.Add(getLinhasGraficoQTD(avaliacoesDB, "Quantitativo", unidade.Nome));
                    }
                }
            }
            return Json(vmGrafico, JsonRequestBehavior.AllowGet);
        }


        private object getLinhasGraficoQTD(IList<AvaliacaoDTO> _avaliacoesDB, string tipo ,string labelString)
        {
            var arrayListGrafico = new ArrayList();

            // Adicionando as inspeções no Grafico

            var avaliacoesDB = _avaliacoesDB.Where(a => a.Situacao == Quiver.DTO.Enum.SituacaoAvaliacao.AVALIADO).OrderBy(a => a.DataFim).ToList();

            if (avaliacoesDB.Count() > 0)
            {
                DateTime dataPrimeiraAvaliacao = avaliacoesDB.First().DataFim ?? TZUtil.GetDataHoraDeBrasilia() ;
                dataPrimeiraAvaliacao = new DateTime(dataPrimeiraAvaliacao.Year, dataPrimeiraAvaliacao.Month, dataPrimeiraAvaliacao.Day, 0, 0, 0);
                DateTime dataUltimaAvaliacao = avaliacoesDB.Last().DataFim ?? TZUtil.GetDataHoraDeBrasilia();
                dataUltimaAvaliacao = new DateTime(dataUltimaAvaliacao.Year, dataUltimaAvaliacao.Month, dataUltimaAvaliacao.Day, 0, 0, 0);

                var pontosNoGraficoDoUsuario = AgendaMapper.MapAvaliacaoDTOToPontoEvolutivoGeralVM(avaliacoesDB, tipo,dataPrimeiraAvaliacao, dataUltimaAvaliacao);

                int MaiorQtdAvaliacoes = 0; 
                MaiorQtdAvaliacoes = pontosNoGraficoDoUsuario.OrderByDescending(a => a.avaliacoesDia.Count()).FirstOrDefault().avaliacoesDia.Count;
                int eixoYTamanho = MaiorQtdAvaliacoes + 1;
                var myData = pontosNoGraficoDoUsuario.Select(a => new object[]{a.TimeStampDataExecutada,
                                        a.QtdAvaliacoes });

                Random corLinha = new Random();
                // Criando a linha do gráfico
                var linhaUnidadesGrafico = new
                {
                    label = labelString,
                    data = myData,
                    eixoY = eixoYTamanho,
                    color = String.Format("#{0:X6}", corLinha.Next(0x1000000)),
                    dataAvaliacao = pontosNoGraficoDoUsuario.Select(p => new object[] {
                                    p.DataExecutada }).First()
                };
                return linhaUnidadesGrafico;
            }
            return null;
        }

        public ActionResult rankingQuantidadeUsuarios()
        {
            ViewBag.tipo = "Quantidade";
            return PartialView("_RankingUsuarios", RankingAdicionarPosiCoes(
                geListUsuarios().OrderByDescending(r => r.QtdAvaliacoes).Take(5).ToList(),
                "quantidade"));
        }


        public ActionResult rankingEfetividadeUsuarios()
        {
            ViewBag.tipo = "efetividade";
            return PartialView("_RankingUsuarios", RankingAdicionarPosiCoes(
                geListUsuarios().OrderByDescending(r => r.Efetividade).Take(5).ToList(),
                "efetividade"));
        }




        private List<RankingGeralVM> geListUsuarios()
        {
            var rankingUsuariosVM = new List<RankingGeralVM>();
            var adminRoleId = _usuarioService.GetPerfilGestor().Id;
            var usuarioDB = _usuarioService.GetAtivosByEmpresa(_user.EmpresaId); 

            var posicaoRankingVM = new RankingGeralVM();
            double efetividade = 0;

            foreach (var usuario in usuarioDB)
            {
                var avaliacoesFinalizadasDoUsuario = _agendaService.GetAvaliacoesByUsuario(usuario.Id).Where(a => a.Situacao == DTO.Enum.SituacaoAvaliacao.AVALIADO);
                int qtdAvalicoesAtivasDoUsuario = avaliacoesFinalizadasDoUsuario.Count();
                posicaoRankingVM = new RankingGeralVM();
                posicaoRankingVM.QtdAvaliacoes = qtdAvalicoesAtivasDoUsuario.ToString();
                posicaoRankingVM.idSelecionado = usuario.Id;
                posicaoRankingVM.Categoria = usuario.Email;

                int qtdEfetividade = qtdAvalicoesAtivasDoUsuario;

                if (qtdEfetividade > 0)
                {
                    foreach (var avaliacao in avaliacoesFinalizadasDoUsuario)
                    {
                        efetividade = efetividade + calcularEfetividade(avaliacao);
                    }

                    efetividade = (efetividade / qtdEfetividade);
                }

                posicaoRankingVM.Efetividade = Math.Round(efetividade, 2);
                efetividade = 0;
                rankingUsuariosVM.Add(posicaoRankingVM);
            }
            return rankingUsuariosVM;
        }



        private List<RankingGeralVM> RankingAdicionarPosiCoes(List<RankingGeralVM> listaVM, string tipo)
        {
            var rankingUsuariosVM = listaVM;
            int ultimaPosicaoDiferente = 1;
            for (var atual = 0; atual < rankingUsuariosVM.Count(); atual++)
            {
                // Lógica feita para repetir o número da posição caso a efetividade seja a mesma.
                if (atual == 0)
                {
                    rankingUsuariosVM[atual].Posicao = (ultimaPosicaoDiferente) + "°";
                }
                else
                {
                    if (tipo == "quantidade")
                    {
                        if (rankingUsuariosVM[atual].QtdAvaliacoes != rankingUsuariosVM[atual - 1].QtdAvaliacoes)
                        {
                            ultimaPosicaoDiferente++;
                            rankingUsuariosVM[atual].Posicao = (ultimaPosicaoDiferente) + "°";
                        }
                        else
                        {
                            rankingUsuariosVM[atual].Posicao = (ultimaPosicaoDiferente) + "°";
                        }
                    }
                    else if (tipo == "efetividade")
                    {
                        if (rankingUsuariosVM[atual].Efetividade != rankingUsuariosVM[atual - 1].Efetividade)
                        {
                            ultimaPosicaoDiferente++;
                            rankingUsuariosVM[atual].Posicao = (ultimaPosicaoDiferente) + "°";
                        }
                        else
                        {
                            rankingUsuariosVM[atual].Posicao = (ultimaPosicaoDiferente) + "°";
                        }
                    }


                }
            }
            return rankingUsuariosVM.ToList();
        }

        private double calcularEfetividade(AvaliacaoDTO a)
        {
            var total = a.PontuacaoMaxima;
            if (total > 0)
            {
                var percentual = (100 * a.PontuacaoEfetuada) / total;
                return percentual;
            }
            return 0;
        }

        public bool eventoPossuiNaoConformidade(ICollection<AvaliacaoQuestionarioGrupoDTO> questionarios)
        {
            // Verificar se existe em algum formulário algum item com NÃO CONFORMIDADE. 
            foreach (var formulario in questionarios)
            {
                foreach (var questao in formulario.QuestionarioGrupo.Questionario.Questoes)
                {
                    if (questao.Tipo != Quiver.DTO.Enum.TipoQuestao.Subjetiva)
                    {
                        foreach (var item in questao.Itens)
                        {
                            if (item.Alternativa.NaoConformidade == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}