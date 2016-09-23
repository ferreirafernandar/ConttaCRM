using Arquitetura.Dominio.Base;
using Arquitetura.Domino.Base.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(TEntity item);

        void Remove(TEntity item);

        void Modify(TEntity item);

        void TrackItem(TEntity item);

        void Merge(TEntity persisted, TEntity current);

        void Commit();

        TEntity Get(Int32 id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);

        IEnumerable<TEntity> AllMatching<KProperty>(ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);

        long Count(ISpecification<TEntity> specification);
    }
}
