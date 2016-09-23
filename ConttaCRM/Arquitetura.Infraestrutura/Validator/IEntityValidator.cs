using System.Collections.Generic;

namespace Arquitetura.Infraestrutura.Validator
{
    public interface IEntityValidator
    {
        bool IsValid<TEntity>(TEntity item)
            where TEntity : class;

        IEnumerable<ValidationResult> GetInvalidMessages<TEntity>(TEntity item)
            where TEntity : class;
    }
}
