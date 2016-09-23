using System;

namespace Arquitetura.Dominio.Base
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void CommitAndRefreshChanges();

        void RollbackChanges();
    }
}
