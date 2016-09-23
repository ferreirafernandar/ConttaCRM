using AutoMapper;
using Arquitetura.DTO;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Aggregates.SocioAgg;
using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using Arquitetura.Dominio.Aggregates.UsoSoloAgg;

namespace Arquitetura.Infraestrutura.Adapter
{
    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            // Mapeamentos
            Mapper.CreateMap<Usuario, UsuarioDTO>();
            Mapper.CreateMap<Usuario, UsuarioListDTO>();
            Mapper.CreateMap<ConfiguracaoServidorEmail, ConfiguracaoServidorEmailDTO>();
            Mapper.CreateMap<Cliente, ClienteDTO>();
            Mapper.CreateMap<Cliente, ClienteListDTO>();
            Mapper.CreateMap<Entrevista, EntrevistaDTO>();
            Mapper.CreateMap<Entrevista, EntrevistaListDTO>();
            Mapper.CreateMap<Responsavel, ResponsavelDTO>();
            Mapper.CreateMap<Responsavel, ResponsavelListDTO>();
            Mapper.CreateMap<Socio, SocioDTO>();
            Mapper.CreateMap<Socio, SocioListDTO>();
            Mapper.CreateMap<NumeroOficial, NumeroOficialDTO>();
            Mapper.CreateMap<NumeroOficial, NumeroOficialListDTO>();
            Mapper.CreateMap<UsoSolo, UsoSoloDTO>();
            Mapper.CreateMap<UsoSolo, UsoSoloListDTO>();
        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}
