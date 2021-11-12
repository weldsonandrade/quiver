using Mvc.Mailer;
using Quiver.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiver.WebAPI.Mailers
{
    public class NotificacaoMailer : MailerBase, INotificacaoMailer
    {
        public virtual MvcMailMessage Notificar(NotificacaoVM notificacaoVM)
        {
            return Populate(x =>
            {
                x.Subject = "Quiver - Não conformidade";
                x.To.Add(notificacaoVM.Email);
                x.ViewName = "Notificar";
                ViewBag.Aplicador = notificacaoVM.Aplicador;
                ViewBag.DataProgramada = notificacaoVM.DataProgramada;
                ViewBag.Grupo = notificacaoVM.Grupo;
                ViewBag.Unidade = notificacaoVM.Unidade;
            });
        }
    }
}