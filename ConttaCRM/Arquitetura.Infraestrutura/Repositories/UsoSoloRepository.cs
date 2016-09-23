using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using Arquitetura.Infraestrutura.Base;
using Arquitetura.Infraestrutura.UnitOfWork;

namespace Arquitetura.Infraestrutura.Repositories
{
    public class UsoSoloRepository : Repository<UsoSolo>, IUsoSoloRepository
    {
        #region Construtor

        public UsoSoloRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion Construtor
    }
}