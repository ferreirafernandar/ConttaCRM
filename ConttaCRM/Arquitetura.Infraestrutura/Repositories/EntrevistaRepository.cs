using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Infraestrutura.Base;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.Infraestrutura.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Infraestrutura.Repositories
{
    public class EntrevistaRepository : Repository<Entrevista>, IEntrevistaRepository
    {
        #region Construtor

        public EntrevistaRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
