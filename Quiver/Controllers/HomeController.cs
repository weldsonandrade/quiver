using System.Web.Mvc;
using System.Net.Mail;
using Quiver.Mailers;
using Quiver.Models.Mailer;

namespace Quiver.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        [AllowAnonymous]
        [OutputCache(Duration = (60 * 60), VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Contato()
        {
            return PartialView("_Contato");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Contato(EmailContato contatoMailer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IContatoMailer _mailer = new ContatoMailer();
                    _mailer.Contato(contatoMailer).Send();
                    return Json(new { success = true });
                }
                catch (SmtpException e)
                {
                    string re = e.Message;
                    return Json(new { success = false });
                }
            }
            return PartialView("_Contato", contatoMailer);
        }

    }
}