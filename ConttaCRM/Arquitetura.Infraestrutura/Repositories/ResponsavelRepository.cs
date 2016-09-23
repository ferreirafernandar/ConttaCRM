using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
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
    public class ResponsavelRepository : Repository<Responsavel>, IResponsavelRepository
    {
        #region Construtor

        public ResponsavelRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
