using Arquitetura.Infraestrutura.Base;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.Infraestrutura.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arquitetura.Dominio.Aggregates.SocioAgg;

namespace Arquitetura.Infraestrutura.Repositories
{
    public class SocioRepository : Repository<Socio>, ISocioRepository
    {
        #region Construtor

        public SocioRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
