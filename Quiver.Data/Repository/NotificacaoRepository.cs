using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Repository
{
    public class NotificacaoRepository : GenericRepository<Notificacao>, INotificacaoRepository
    {
        public NotificacaoRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Notificacao> GetByEmpresaAndEmailUsuarioStartWithNome(int idEmpresa, string startWithEmail)
        {
            return Get(filter: n => n.UsuarioNotificado.Email.StartsWith(startWithEmail) && n.UsuarioNotificado.Empresa.Id == idEmpresa).OrderByDescending(n => n.Id);
        }

        public IEnumerable<Notificacao> GetAsCincoMaisRecentesByUsuario(string idUsuario)
        {
            return Get(n => n.UsuarioNotificado.Id == idUsuario, orderBy: n => n.OrderByDescending(x => x.Data)).Take(5);
        }

        public int GetQuantidadeNaoLidaByUsuario(string idUsuario)
        {
            return Get(n => n.UsuarioNotificado.Id == idUsuario && !n.Lida).Count();
        }

        public IEnumerable<Notificacao> GetAsNaoLidasByUsuario(string idUsuario)
        {
            return Get(n => n.UsuarioNotificado.Id == idUsuario && !n.Lida);
        }
    }
}
