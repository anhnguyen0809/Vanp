using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;
using Vanp.DAL.Utils.Extentions;

namespace Vanp.DAL
{
    public class CategoryRepository : GeneralRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Vanp_Entities context) : base(context)
        {
        }
        public override IEnumerable<Category> GetList()
        {
            return _dbSet.Where(o => o.Enable.Value);
        }
        public IEnumerable<Category> GetListByParent(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryParentId == categoryId && o.Enable.Value);
        }
        public IEnumerable<Category> GetListAllChildByParent(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryParentId == categoryId && o.Enable.Value).Traverse<Category>(o=> o.Category1);
        }
        public IEnumerable<Category> GetListAllChildByParent(int[] categoriesId)
        {
            return _dbSet.Where(o => o.CategoryParentId.HasValue && categoriesId.Contains(o.CategoryParentId.Value) && o.Enable.Value).Traverse<Category>(o => o.Category1);
        }
        public IEnumerable<Category> GetListParent()
        {
            return _dbSet.Where(o => !o.CategoryParentId.HasValue && o.Enable.Value);
        }
        public IEnumerable<Category> GetListParentShow()
        {
            return _dbSet.Where(o => !o.CategoryParentId.HasValue && o.Enable.Value && o.Show.Value);
        }
    }
}
