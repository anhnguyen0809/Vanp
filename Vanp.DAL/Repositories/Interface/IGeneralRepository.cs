using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanp.DAL
{
    public interface IGeneralRepository<TEntity> : IDisposable where TEntity : class
    {
        #region [Insert - Update - Delete]
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        void SaveChanges();
        #endregion

        #region [Get]
        TEntity GetById(object id);
        IEnumerable<TEntity> GetList();
        IQueryable<TEntity> GetList(string orderBy, bool asc);
        #endregion

    }
}
