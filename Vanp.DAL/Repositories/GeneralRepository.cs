using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity>
        where TEntity : class
    {
        #region Properties
        protected internal readonly Vanp_Entities _context;
        protected internal readonly DbSet<TEntity> _dbSet;
        public GeneralRepository()
        {
            this._context = new Vanp_Entities();
            this._dbSet = _context.Set<TEntity>();
        }
        public GeneralRepository(Vanp_Entities context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }
        #endregion

        #region [Insert - Update - Delete]
        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        public virtual void Delete(object id)
        {
            TEntity entity = _dbSet.Find(id);
            this.Delete(entity);
        }
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
        #endregion

        #region [Get]
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public virtual IEnumerable<TEntity> GetList()
        {
            return _dbSet.ToList();
        }

        public virtual IQueryable<TEntity> GetList(string orderBy, bool asc)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var propertyInfo = typeof(TEntity).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    IOrderedQueryable<TEntity> order = null;
                    if (asc)
                    {
                        order = _dbSet.OrderBy(o => propertyInfo.GetValue(o));
                    }
                    else
                    {
                        order = _dbSet.OrderByDescending(o => propertyInfo.GetValue(o));
                    }
                    return order;
                }
            }
            return null;
        }
        #endregion

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
