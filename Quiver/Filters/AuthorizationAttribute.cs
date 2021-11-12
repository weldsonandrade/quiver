using Quiver.Controllers;
using Quiver.DTO.Avaliacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiver.Filters
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public string Id { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = ((BaseController)filterContext.Controller);
            var user = controller._user;
            var id = GetId(filterContext);

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                bool isAuthorized = false;
                if (controller is UnidadeController)
                {
                    var idEmpresaOfUnidade = ((UnidadeController) controller)._unidadeService.GetIdEmpresaOfUnidade(Convert.ToInt32(id));
                    if (idEmpresaOfUnidade == user.EmpresaId)
                        isAuthorized = true;
                }
                if (controller is AgendaController)
                {
                    int IdEmpresaOfAvaliacao = ((AgendaController)controller)._agendaService.GetIdEmpresaOfAvaliacao(Convert.ToInt32(id));
                    if (IdEmpresaOfAvaliacao == user.EmpresaId)
                        isAuthorized = true;
                }
                if (controller is UsuarioController)
                {
                    var usuario = ((UsuarioController) controller)._usuarioService.GetById(id);
                    if (usuario != null && usuario.IdEmpresa == user.EmpresaId)
                        isAuthorized = true;
                }
                if (controller is GrupoController)
                {
                    var grupo = ((GrupoController) controller)._grupoService.GetById(Convert.ToInt32(id));
                    if (grupo != null && grupo.IdEmpresa == user.EmpresaId)
                        isAuthorized = true;
                }
                if (controller is QuestionarioController)
                {
                    var questionario = ((QuestionarioController) controller)._formularioService.GetById(Convert.ToInt32(id));
                    if (questionario != null && questionario.IdEmpresa == user.EmpresaId)
                        isAuthorized = true;
                }
                if (controller is EmpresaController)
                {
                    // So tem permissão quem é administrador.
                    isAuthorized = true;
                }
                if (controller is PlanoAcaoController)
                {
                    var planoAcao = ((PlanoAcaoController)controller)._planoAcaoService.GetById(Convert.ToInt32(id));
                    if (planoAcao != null && planoAcao.Questao.Formulario.Avaliacao.IdEmpresa == user.EmpresaId)
                        isAuthorized = true;
                }
                if (!isAuthorized)
                {
                    redirectToUnathorized(filterContext);
                }
            }
        }

        private string GetId(ActionExecutingContext filterContext)
        {
            String pKey = Id;
            String pValue = string.Empty;
            if (Id.ToLowerInvariant().Contains('.'))
            {
                string[] ids = Id.Split('.');
                pKey = ids[0];
                pValue = ids[1];
            }

            if (filterContext.ActionParameters.ContainsKey(pKey))
            {
                var id = "";
                if (String.IsNullOrEmpty(pValue))
                    id = filterContext.ActionParameters[pKey] == null ? string.Empty : filterContext.ActionParameters[pKey].ToString();
                else
                    id = filterContext.ActionParameters[pKey].GetType().GetProperty(pValue).GetValue(filterContext.ActionParameters[pKey]) == null
                        ? string.Empty
                        : filterContext.ActionParameters[pKey].GetType().GetProperty(pValue).GetValue(filterContext.ActionParameters[pKey]).ToString();

                return id;
            }

            return string.Empty;
        }

        private void redirectToUnathorized(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult { Data = new { unauthorized = true } };
            }
            else
            {
                filterContext
                    .HttpContext
                    .Response
                    .RedirectToRoute(new { controller = "Error", action = "Unauthorized" });
            }
        }
    }
}