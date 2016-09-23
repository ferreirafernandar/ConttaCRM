using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;
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
    public class ConfiguracaoServidorEmailRepository : Repository<ConfiguracaoServidorEmail>, IConfiguracaoServidorEmailRepository
    {
        #region Construtor

        public ConfiguracaoServidorEmailRepository(MainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
