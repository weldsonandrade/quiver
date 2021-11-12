using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;
using System.Threading.Tasks;

namespace Quiver.WebAPI
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            string userId = Context.QueryString["userId"];

            //HubUserConnection userConnection = Startup.userConnectionList.FirstOrDefault(u => u.UserID == userId);

            //if(userConnection!=null)
            //{
            //    Startup.userConnectionList.Remove(userConnection);
            //}

            HubUserConnection newUserConnection = new HubUserConnection()
            {
                UserID = userId,
                ConnectionID = Context.ConnectionId
            };

            Startup.userConnectionList.Add(newUserConnection);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            HubUserConnection userConnection = Startup.userConnectionList.FirstOrDefault(u => u.ConnectionID == Context.ConnectionId);

            Startup.userConnectionList.Remove(userConnection);

            return base.OnDisconnected(stopCalled);
        }

        public void EnviarMensagemParaTodos(string mensagem)
        {
            Clients.All.receberMensagemParaTodos(mensagem);
        }

        public void EnviarMensagemParaUsuario(string mensagem, string destinatario)
        {
            foreach (HubUserConnection userConnection in Startup.userConnectionList.Where(u => u.UserID == destinatario))
            {
                string connectionId = userConnection.ConnectionID;
                Clients.Client(connectionId).receberMensagemParaUsuario(mensagem);
            }
        }

        public void EnviarSinalParaUsuario(int sinal, string destinatario)
        {
            foreach (HubUserConnection userConnection in Startup.userConnectionList.Where(u => u.UserID == destinatario))
            {
                string connectionId = userConnection.ConnectionID;
                Clients.Client(connectionId).receberSinalParaUsuario(sinal);
            }
        }
    }
}