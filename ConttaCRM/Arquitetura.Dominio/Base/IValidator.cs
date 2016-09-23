using System.Collections.Generic;

namespace Arquitetura.Dominio.Base
{
    public interface IValidator
    {
        IEnumerable<string[]> Validate();
    }
}
