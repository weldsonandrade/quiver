using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mailers
{
    public class UsuarioMailer : MailerBase, IUsuarioMailer
    {
        public virtual MvcMailMessage CadastrarSenha(string id, string email, string token)
        {
            return Populate(x =>
            {
                x.Subject = "Quiveer - Acesso";
                x.To.Add(email);
                x.ViewName = "CadastrarSenha";
                x.IsBodyHtml = true;
                ViewBag.Id = id;
                ViewBag.Token = token;
            });
        }
    }
}