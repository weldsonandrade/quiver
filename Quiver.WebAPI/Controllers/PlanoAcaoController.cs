using System;
using System.Web.Http;
using Quiver.WebAPI.DTO;

namespace Quiver.WebAPI.Controllers
{
    public class PlanoAcaoController : ApiController
    {
        // GET: api/PlanoAcao/ProcessaStatus
        [AllowAnonymous]
        [HttpPost]
        public void ProcessaStatus()
        {
            try
            {
                PlanoAcaoDTO planoAcao = new PlanoAcaoDTO();

                planoAcao.MarcarAtrasados();

                planoAcao.EncerrarResolvidos();
            }
            catch (Exception ex)
            {

            }
        }
    }
}