using System;
using System.Linq;
using System.Web.Mvc;
using Quiver.Models;
using Quiver.Service.Interfaces;
using Quiver.DTO.Notificacao;
using System.Collections.Generic;
using Quiver.Mappers;
using System.Configuration;

namespace Quiver.Controllers
{
    public class HeaderController : BaseController
    {
        private readonly INotificacaoService _notificacaoService;

        public HeaderController(IUsuarioService usuarioService, INotificacaoService notificacaoService) : base(usuarioService)
        {
            _notificacaoService = notificacaoService;
        }

        public string GetUserID()
        {
            return _user.UsuarioId;
        }

        public ActionResult Notificacoes() {
            return View("~/Views/Relatorio/Notificacoes.cshtml");
        }

        public ActionResult TabelaNotificacoes(string termo = "")
        {
            IList<NotificacaoDTO> notificacoes = _notificacaoService.GetByEmpresaAndEmailUsuarioStartWithNome(_user.EmpresaId, _user.Email);
            return PartialView("~/Views/Relatorio/_TabelaNotificacoes.cshtml", notificacoes);
        }


        [HttpGet ]
        public ActionResult ConsultarNotificacoes()
        {
            try
            {
                VisualizarNotificacoesDTO visualizarNotificacoesDTO = _notificacaoService.VisualizarNotificacoes(_user.UsuarioId);
                HeaderViewModel headerViewModel = NotificacaoMapper.MapVisualizarNotificacoesDTOToHeaderViewModel(visualizarNotificacoesDTO);

                return PartialView("_NotificacaoBody", headerViewModel);
            }
            catch (Exception ex)
            {
                string f = ex.Message;
                return PartialView("_NotificacaoBody");
            }
        }

        [HttpPost]
        public ActionResult ConsultarQuantidadeNaoLida()
        {
            try
            {
                int quantidadeNaoLida = _notificacaoService.GetQuantidadeNaoLidaByUsuario(_user.UsuarioId);
                return Json(quantidadeNaoLida);
            }
            catch (Exception ex)
            {
                string re = ex.Message;
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult ExcluirNotificacoes(HeaderViewModel headerVM)
        {
            try
            {
                _notificacaoService.Delete(headerVM.Notificacoes.Select(n => n.Id));

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                string re = ex.Message;
                return Json(new { success = false });
            }
        }
    }
}