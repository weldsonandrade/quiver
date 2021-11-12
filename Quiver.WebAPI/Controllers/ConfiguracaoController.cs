using System;
using System.Web.Http;
using Quiver.WebAPI.DTO;
using Quiver.Data.Repository;
using Quiver.Service.Implementation;

namespace Quiver.WebAPI.Controllers
{
    public class ConfiguracaoController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetLasMobileAppVersion()
        {
            ConfiguracaoService configService = new ConfiguracaoService(new UnitOfWork());

            return Ok(new { Valor = configService.GetLastMobileVersion() });
        }
    }
}