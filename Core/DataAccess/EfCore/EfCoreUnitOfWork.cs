using Microsoft.EntityFrameworkCore;
using Core.DataAccess.UnitOfWork;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Core.DataAccess.EfCore
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        public readonly DbContext _context;
        private bool _disposed;

        public EfCoreUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                _context.Database.OpenConnection();

            if (_context.Database.CurrentTransaction is null)
                _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.ChangeTracker.DetectChanges();
            SaveChanges();
            _context.Database.CurrentTransaction.Commit();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Rollback()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return (TDbContext)_context;
        }
    }
}