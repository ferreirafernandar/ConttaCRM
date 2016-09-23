using AutoMapper;

namespace Arquitetura.Infraestrutura.Adapter
{
    public class AutomapperTypeAdapter : ITypeAdapter
    {
        #region ITypeAdapter Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/>
        /// </summary>
        /// <typeparam name="TSource"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></typeparam>
        /// <typeparam name="TTarget"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></typeparam>
        /// <param name="source"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></returns>
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/>
        /// </summary>
        /// <typeparam name="TTarget"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></typeparam>
        /// <param name="source"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapter.ITypeAdapter"/></returns>
        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }

        #endregion
    }
}
