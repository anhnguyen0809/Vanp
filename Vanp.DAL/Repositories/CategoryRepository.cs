using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class CategoryRepository : GeneralRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<Category> GetListByParent(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryParentId == categoryId);
        }
        public IEnumerable<Category> GetListParent()
        {
            return _dbSet.Where(o => !o.CategoryParentId.HasValue);
        }
    }
}
