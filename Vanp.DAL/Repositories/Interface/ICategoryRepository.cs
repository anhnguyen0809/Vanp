using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface ICategoryRepository : IGeneralRepository<Category>
    {
        IEnumerable<Category> GetListByParent(int categoryId);
        IEnumerable<Category> GetListParent();
        IEnumerable<Category> GetListParentShow();
        IEnumerable<Category> GetListAllChildByParent(int categoryId);
        IEnumerable<Category> GetListAllChildByParent(int[] categoriesId);

    }
}
