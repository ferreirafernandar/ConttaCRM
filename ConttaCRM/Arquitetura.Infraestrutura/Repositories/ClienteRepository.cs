using Arquitetura.Dominio.Aggregates.ClienteAgg;
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
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        #region Construtor

        public ClienteRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
