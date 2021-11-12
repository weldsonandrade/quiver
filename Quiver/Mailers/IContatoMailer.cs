using Mvc.Mailer;
using Quiver.Models.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Mailers
{
    interface IContatoMailer
    {
        MvcMailMessage Contato(EmailContato contatoMailer);
    }
}
