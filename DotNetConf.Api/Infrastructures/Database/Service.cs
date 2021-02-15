using DotNetConf.Api.Entities.Common;
using DotNetConf.Api.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetConf.Api.Infrastructures.Database
{
    public interface IService
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbSet<T> Query<T>() where T : BaseEntity;
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate, bool isTracking = false) where T : BaseEntity;
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, bool isTracking = false) where T : BaseEntity;
        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;
        Task<List<T>> GetListAsync<T>(bool isTracking = false) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(long id, bool isTracking = false) where T : BaseEntity;
        T Add<T>(T model) where T : BaseEntity;
        T Update<T>(T model) where T : BaseEntity;
        void Delete<T>(T model) where T : BaseEntity;
        void Remove<T>(T model) where T : BaseEntity;
    }
    public class Service : IService
    {
        private readonly DotNetConfDbContext _context;
        public Service(DotNetConfDbContext context)
        {
            _context = context;
        }

        public virtual DbSet<T> Query<T>() where T : BaseEntity
        {
            return _context.Set<T>();
        }

        public virtual Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate, bool isTracking = false) where T : BaseEntity
        {
            var result = _context.Set<T>();
            return isTracking
                ? result.Where(predicate).ToListAsync<T>()
                : result.AsNoTracking().Where(predicate).ToListAsync<T>();
        }
        public virtual Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, bool isTracking = false) where T : BaseEntity
        {
            var result = _context.Set<T>();

            return isTracking
                ? result.FirstOrDefaultAsync<T>(predicate)
                : result.AsNoTracking().FirstOrDefaultAsync<T>(predicate);
        }

        public virtual Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return _context.Set<T>().AnyAsync<T>(predicate);
        }

        public virtual Task<T> GetByIdAsync<T>(long id, bool isTracking = false) where T : BaseEntity
        {
            var result = _context.Set<T>();
            return isTracking
                ? result.FirstOrDefaultAsync(x => x.Id == id && x.Status == RecordStatuses.ACTIVE)
                : result.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Status == RecordStatuses.ACTIVE);
        }

        public virtual Task<List<T>> GetListAsync<T>(bool isTracking = false) where T : BaseEntity
        {
            var result = _context.Set<T>();
            return isTracking
                ? result.Where(x => x.Status == RecordStatuses.ACTIVE).ToListAsync()
                : result.AsNoTracking().Where(x => x.Status == RecordStatuses.ACTIVE).ToListAsync();
        }
        public virtual T Add<T>(T model) where T : BaseEntity
        {
            model.Add();
            return _context.Set<T>().Add(model).Entity;
        }

        public virtual T Update<T>(T model) where T : BaseEntity
        {
            model.Update();
            return _context.Set<T>().Update(model).Entity;
        }

        public virtual void Delete<T>(T model) where T : BaseEntity
        {
            model.Delete();
            _context.Set<T>().Update(model);
        }

        public virtual void Remove<T>(T model) where T : BaseEntity
        {
            _context.Set<T>().Remove(model);
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
