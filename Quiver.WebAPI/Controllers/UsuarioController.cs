using System;
using System.Web.Http;
using Quiver.WebAPI.DTO;

namespace Quiver.WebAPI.Controllers
{
    public class UsuarioController : ApiController
    {
        // GET: api/Usuario/GetSync?userName={id}&password={password}
        [AllowAnonymous]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetSync(string userName, string password)
        {
            try
            {
                GetSyncContainer container = new GetSyncContainer(userName, password);

                if (container == null)
                {
                    return Ok(new
                    {
                        Sucesso = false,
                        Mensagem = "Usuário ou senha inválido"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Sucesso = true,
                        Mensagem = "Consulta realizada com sucesso",
                        Container = container
                    });
                }
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }
        }

        // GET: api/Usuario/SendSync
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult SendSync(SendSyncContainer container)
        {
            try
            {
                bool processado = container.Sincronizar();

                if (!processado)
                {
                    return Ok(new
                    {
                        Sucesso = false,
                        Mensagem = "Usuário ou senha inválido"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Sucesso = true,
                        Mensagem = "Processado com sucesso"
                    });
                }
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    Sucesso = false,
                    // Print com inner exceptions
                    Mensagem = ex.ToString()
                });
            }
        }

    }
}