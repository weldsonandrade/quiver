using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Quiver.Service.Interfaces;

namespace Quiver.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public readonly IUsuarioService _usuarioService;
        public UserInfo _user;

        public BaseController(IUsuarioService usuarioService)
            : base()
        {
            this._usuarioService = usuarioService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            InitParams();
        }

        private void InitParams()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Caso tenham os dados armazenados na sessão.
                if (HttpContext != null && HttpContext.Session != null && Session["UsuarioId"] != null)
                {
                    _user = new UserInfo()
                    {
                        EmpresaId = (int)Session["EmpresaId"],
                        Role = (string)Session["Role"],
                        UsuarioId = (string)Session["UsuarioId"],
                        Email = (string)Session["Email"]
                    };
                }
                else
                {
                    // Caso contrário vai no banco e atualiza a sessão.
                    _user = new UserInfo()
                    {
                        UsuarioId = User.Identity.GetUserId(),
                        Role = ((ClaimsIdentity)User.Identity).Claims
                                    .Where(c => c.Type == ClaimTypes.Role)
                                    .Select(c => c.Value).FirstOrDefault(),
                        EmpresaId = _usuarioService.GetById(User.Identity.GetUserId()).IdEmpresa,
                        Email = _usuarioService.GetById(User.Identity.GetUserId()).Email
                    };

                    if (HttpContext != null && HttpContext.Session != null)
                    {
                        Session["UsuarioId"] = _user.UsuarioId;
                        Session["Role"] = _user.Role;
                        Session["EmpresaId"] = _user.EmpresaId;
                        Session["Email"] = _user.Email;
                    }
                }
            }
        }

        public static string getAllBeforeChar(string s, char caracter)
        {
            int final = s.IndexOf(caracter);
            if (final > 0)
            {
                return s.Substring(0, final);
            }
            return s;
        }
    }

    public class UserInfo
    {
        public string UsuarioId { get; set; }

        public string Role { get; set; }

        public int EmpresaId { get; set; }

        public string Email { get; set; }
    }

}