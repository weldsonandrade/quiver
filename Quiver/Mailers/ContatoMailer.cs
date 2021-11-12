using Mvc.Mailer;
using Quiver.Models.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mailers
{
    public class ContatoMailer : MailerBase, IContatoMailer
    {

        public virtual MvcMailMessage Contato(EmailContato contatoMailer)
        {
            return Populate(x =>
            {
                x.Subject = "Contato do Quiver";
                x.To.Add("andrey@helloworldsoft.com");
                x.ViewName = "Contato";
                ViewBag.Nome = contatoMailer.Nome;
                ViewBag.Email = contatoMailer.Email;
                ViewBag.Mensagem = contatoMailer.Mensagem;
            });
        }
    }
}