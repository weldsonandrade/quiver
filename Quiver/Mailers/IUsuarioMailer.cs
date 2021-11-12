using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mailers
{
    public interface IUsuarioMailer
    {
        MvcMailMessage CadastrarSenha(string id, string email, string token);

        //MvcMailMessage ResetarSenha(string id, string email, string token);
    }
}