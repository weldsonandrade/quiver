using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Quiver.Models;
using Quiver.Core.Models;
using Quiver.Infrastructure.Configuration;
using Quiver.Service.Interfaces;
using Quiver.DTO.Empresa;
using Quiver.DTO.Usuario;
using Quiver.DTO.Perfil;
using System.IO;
using Quiver.Filters;
using Quiver.Mappers;
using System.Security.Claims;
using Quiver.Models.Email;

namespace Quiver.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IEmpresaService _empresaService;
        private IUsuarioService _usuarioService;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IEmpresaService empresaService, IUsuarioService usuarioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _empresaService = empresaService;
            _usuarioService = usuarioService;
        }
        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verifica se o e-mail foi confirmado, caso contrário 
            // redireciona para reenvio do email de confirmação.
            Usuario usuario = _userManager.FindByEmail(model.Email);
            if (usuario != null && !_userManager.IsEmailConfirmed(usuario.Id))
            {
                SendConfirmEmail(usuario.Id);

                return View("ConfirmEmailResend");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            // Aqui será verificado se o usuário é administrador
            // Se não for será realizado o logoff e exibirá a mensagem de erro.
            if (result == SignInStatus.Success)
            {
                var user = await _userManager.FindAsync(model.Email, model.Password);
                var roles = await _userManager.GetRolesAsync(user.Id);
                if (user == null)
                    result = SignInStatus.Failure;

                if (!roles.Contains("Gestor") && !roles.Contains("Administrador"))
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    result = SignInStatus.Failure;
                }
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Bloqueado");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Usuário ou senha inválido.");
                    return View(model);
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }

        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var iResult = await _userManager.ChangePasswordAsync(userId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                if (iResult.Succeeded)
                {
                    return Json(new { ok = true });
                }

                ModelState.AddModelError(string.Empty, iResult.Errors.First());
            }
            return PartialView("_ChangePassword", changePasswordViewModel);
        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/Registers    
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                EmpresaDTO empresaDTO = new EmpresaDTO()
                {
                    CNPJ = Guid.NewGuid().ToString().Substring(0,13),
                    LimiteLicencas = 5,
                    Nome = model.Nome,
                    Situacao = DTO.Enum.SituacaoEmpresa.DESATIVA
                };

                PerfilDTO perfilDTO = _usuarioService.GetPerfilGestor();

                CriarUsuarioDTO criarUsuarioDTO = new CriarUsuarioDTO()
                {
                    Email = model.Email,
                    Nome = model.Nome,
                    Perfil = perfilDTO.Id,
                    Senha = model.Password,
                    Telefone = model.Telefone
                };

                CriarEmpresaDTO criarEmpresaDTO = new CriarEmpresaDTO()
                {
                    empresaDTO = empresaDTO,
                    criarUsuarioDTO = criarUsuarioDTO
                };
                
                IdentityResult createResult = _empresaService.Insert(criarEmpresaDTO);

                if (createResult.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string userId = criarUsuarioDTO.Id;
                    SendConfirmEmail(userId);

                    return View("ConfirmEmailSend");
                }

                AddErrors(createResult);
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult RegisterGeneratePassword(UsuarioVM usuarioVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (usuarioVM.Id == null)
                    {
                        var perfilLogado = ((ClaimsIdentity)User.Identity).Claims
                              .Where(c => c.Type == ClaimTypes.Role)
                              .Select(c => c.Value).FirstOrDefault();
                        var empresaLogada = _usuarioService.GetById(User.Identity.GetUserId()).IdEmpresa;

                        CriarUsuarioDTO criarUsuarioDTO = UsuarioMapper.MapUsuarioVMToCriarUsuarioDTO(usuarioVM, perfilLogado, empresaLogada);

                        _usuarioService.Insert(criarUsuarioDTO);

                        // Envia E-mail com as instruções para entrar no sistema.
                        string code = _userManager.GenerateEmailConfirmationToken(criarUsuarioDTO.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = criarUsuarioDTO.Id, code = code }, protocol: Request.Url.Scheme);
                        _userManager.SendEmail(criarUsuarioDTO.Id, "Confirme sua conta", RenderRazorViewToString("EmailConfirmarContaComSenha", new CriarContaComSenhaVM() { Link = callbackUrl, Senha = criarUsuarioDTO.Senha }));

                        return Json(new { ok = true });
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return Json(new { ok = false, errorMessage = e.Message });
                }
            }
            var todosErros = ModelState.Values.SelectMany(v => v.Errors.FirstOrDefault().ErrorMessage);
            return Json(new { ok = false, errorMessage = todosErros });
        }

        private async Task SendConfirmEmail(String userId)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);

            await Task.Run(() =>
            {
                _userManager.SendEmailAsync(userId, "Confirme sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");
            });
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return Json(new { ok = false }); ;
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Alteração de senha", RenderRazorViewToString("EmailEsqueciSenha", callbackUrl));

                return Json(new { ok = true });
            }

            // If we got this far, something failed, redisplay form
            return Json(new { ok = false }); ;
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new Usuario { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Agenda");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}