using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using Arquitetura.Infraestrutura.Base;
using Arquitetura.Infraestrutura.UnitOfWork;

namespace Arquitetura.Infraestrutura.Repositories
{
    public class NumeroOficialRepository : Repository<NumeroOficial>, INumeroOficialRepository
    {
        #region Construtor

        public NumeroOficialRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion Construtor
    }
}