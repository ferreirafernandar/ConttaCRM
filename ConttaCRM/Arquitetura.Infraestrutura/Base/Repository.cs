using Arquitetura.Dominio.Base;
using Arquitetura.Domino.Base.Specification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Infraestrutura.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        #region Members

        IQueryableUnitOfWork _UnitOfWork;

        #endregion

        #region Constructor

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            _UnitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        public virtual void Add(TEntity item)
        {

            if (item != (TEntity)null)
                GetSet().Add(item); // add new item in this set
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotAddNullEntity, typeof(TEntity).ToString());

            }

        }

        public virtual void Remove(TEntity item)
        {
            if (item != (TEntity)null)
            {
                //attach item if not exist
                _UnitOfWork.Attach(item);

                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                // LoggerFactory.CreateLog()
                //           .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual void TrackItem(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.Attach<TEntity>(item);
            else
            {
                // LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual void Modify(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.SetModified(item);
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual TEntity Get(Int32 id)
        {
            if (id > 0)
                return GetSet().Find(id);
            else
                return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        public virtual IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            return GetSet().Where(specification.SatisfiedBy())
                           .AsEnumerable();
        }

        public virtual IEnumerable<TEntity> AllMatching<KProperty>(ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.Where(specification.SatisfiedBy())
                          .OrderBy(orderByExpression)
                          .AsEnumerable();
            }
            else
            {
                return set.Where(specification.SatisfiedBy())
                          .OrderByDescending(orderByExpression)
                          .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.Where(specification.SatisfiedBy())
                          .OrderBy(orderByExpression)
                          .Skip(pageCount * (pageIndex - 1))
                          .Take(pageCount)
                          .AsEnumerable();
            }
            else
            {
                return set.Where(specification.SatisfiedBy())
                          .OrderByDescending(orderByExpression)
                          .Skip(pageCount * (pageIndex - 1))
                          .Take(pageCount)
                          .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter)
                           .AsEnumerable();
        }

        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _UnitOfWork.ApplyCurrentValues(persisted, current);
        }

        public virtual void Commit()
        {
            _UnitOfWork.Commit();
        }

        public virtual long Count(ISpecification<TEntity> specification)
        {
            return GetSet().Where(specification.SatisfiedBy()).Count();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion

        #region Private Methods

        IDbSet<TEntity> GetSet()
        {
            return _UnitOfWork.CreateSet<TEntity>();
        }

        #endregion
    }
}
