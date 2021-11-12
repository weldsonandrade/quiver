
using Mvc.Mailer;
using Quiver.WebAPI.Models;

namespace Quiver.WebAPI.Mailers
{
    public interface INotificacaoMailer
    {
        MvcMailMessage Notificar(NotificacaoVM notificacaoVM);
    }
}
