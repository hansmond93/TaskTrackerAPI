using System;
using System.Threading.Tasks;

namespace Core.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        Task<int> SaveChangesAsync();

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}