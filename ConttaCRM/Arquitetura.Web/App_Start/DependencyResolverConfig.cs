using Arquitetura.Aplicacao.Services;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Aggregates.SocioAgg;
using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Logging;
using Arquitetura.Infraestrutura.Repositories;
using Arquitetura.Infraestrutura.UnitOfWork;
using Arquitetura.Infraestrutura.Validator;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Web.Mvc;

namespace Arquitetura.Web
{
    public class DependencyResolverConfig
    {
        private static UnityContainer _container;

        public static void RegisterDependency()
        {
            // criando nosso container de dependencias e registrando uma dependencia de exemplo.
            _container = new UnityContainer();
            _container.AddNewExtension<Interception>();

            //-> Unit of Work and repositories
            _container.RegisterType(typeof(MainBCUnitOfWork), new PerResolveLifetimeManager());

            _container.RegisterType(typeof(ITypeAdapterFactory), typeof(AutomapperTypeAdapterFactory));
            var typeAdapterFactory = _container.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);

            #region RegisterType

            // Repositórios
            _container.RegisterType(typeof(IUsuarioRepository), typeof(UsuarioRepository));
            _container.RegisterType(typeof(IConfiguracaoServidorEmailRepository), typeof(ConfiguracaoServidorEmailRepository));
            _container.RegisterType(typeof(ITokenSenhaRepository), typeof(TokenSenhaRepository));
            _container.RegisterType(typeof(IClienteRepository), typeof(ClienteRepository));
            _container.RegisterType(typeof(IEntrevistaRepository), typeof(EntrevistaRepository));
            _container.RegisterType(typeof(IResponsavelRepository), typeof(ResponsavelRepository));
            _container.RegisterType(typeof(ISocioRepository), typeof(SocioRepository));
            _container.RegisterType(typeof(INumeroOficialRepository), typeof(NumeroOficialRepository));
            _container.RegisterType(typeof(IUsoSoloRepository), typeof(UsoSoloRepository));

            // Serviços
            _container.RegisterType(typeof(IUsuarioAppService), typeof(UsuarioAppService)).Configure<Interception>().SetInterceptorFor<IUsuarioAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IConfiguracaoAppService), typeof(ConfiguracaoAppService)).Configure<Interception>().SetInterceptorFor<IConfiguracaoAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IEmailAppService), typeof(EmailAppService)).Configure<Interception>().SetInterceptorFor<IEmailAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IClienteAppService), typeof(ClienteAppService)).Configure<Interception>().SetInterceptorFor<IClienteAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IEntrevistaAppService), typeof(EntrevistaAppService)).Configure<Interception>().SetInterceptorFor<INumeroOficialAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IResponsavelAppService), typeof(ResponsavelAppService)).Configure<Interception>().SetInterceptorFor<IResponsavelAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(ISocioAppService), typeof(SocioAppService)).Configure<Interception>().SetInterceptorFor<ISocioAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(INumeroOficialAppService), typeof(NumeroOficialAppService)).Configure<Interception>().SetInterceptorFor<INumeroOficialAppService>(new InterfaceInterceptor());
            _container.RegisterType(typeof(IUsoSoloAppService), typeof(UsoSoloAppService)).Configure<Interception>().SetInterceptorFor<IUsoSoloAppService>(new InterfaceInterceptor());
            #endregion

            //modificando o DependencyResolver para a nossa customização passando o container.
            DependencyResolver.SetResolver(new UnityDependencyResolver(_container));

            LoggerFactory.SetCurrent(new EmailTraceSourceLogFactory());
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }
    }
}