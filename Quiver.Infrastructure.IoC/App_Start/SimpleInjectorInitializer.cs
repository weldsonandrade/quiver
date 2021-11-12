[assembly: WebActivator.PostApplicationStartMethod(typeof(Quiver.Infraestrutura.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Quiver.Infraestrutura.App_Start
{
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Data.Repository;
    using Quiver.Infrastructure.Configuration;
    using Core.Models;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin;
    using Service;
    using Service.Interfaces;
    using Service.Implementation;
    using Data.Interfaces;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataHandler.Encoder;
    using Microsoft.Owin.Security.DataHandler.Serializer;
    using Microsoft.Owin.Security.DataHandler;
    using Microsoft.Owin.Security.DataProtection;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = InitializeContainer();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static Container InitializeContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterPerWebRequest(() =>
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["owin.Environment"] == null && container.IsVerifying)
                {
                    return new OwinContext().Authentication;
                }
                return HttpContext.Current.GetOwinContext().Authentication;
            });

            container.Register<IUnitOfWork, UnitOfWork>();
            container.RegisterPerWebRequest<IUserStore<Usuario>>(() => new UserStore<Usuario>(new QuiverDbContext()));
            container.Register<IUnidadeService, UnidadeService>();
            container.Register<IGrupoService, GrupoService>();
            container.Register<IFormularioService, FormularioService>();
            container.Register<IAgendaService, AgendaService>();
            container.Register<IUsuarioService, UsuarioService>();
            container.Register<IEmpresaService, EmpresaService>();
            container.Register<INotificacaoService, NotificacaoService>();
            container.Register<IPlanoAcaoService, PlanoAcaoService>();
            container.Register<IConfiguracaoService, ConfiguracaoService>();

            return container;
        }
    }
}