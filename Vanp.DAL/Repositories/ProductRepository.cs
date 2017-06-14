using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL;
using Vanp.DAL.Entites;
namespace Vanp.DAL
{
    public class ProductRepository : GeneralRepository<Product>, IProductRepository
    {
        public ProductRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<Product> GetListByProduct()
        {
            return _dbSet.ToList();
        }
        public IEnumerable<Product> GetListByCProduct(int userId)
        {
            return _dbSet.Where(p=>p.CreatedBy==userId).ToList();
        }
        public bool isExisted(string code)
        {
            return _dbSet.Any(p => p.ProductCode.ToLower().Equals(code.ToLower()));
        }

    }
}
