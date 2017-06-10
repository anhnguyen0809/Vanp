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
        public IEnumerable<Product> GetListbyProducts()
        {
            return _dbSet.ToList();
        }
        public IEnumerable<Product> DetailProducts(int productID)
        {
            return _dbSet.Where(p => p.Id == productID);
        }
        public bool isExisted(string productcode)
        {
            return _dbSet.Any(p=>p.ProductCode.ToLower().Equals(productcode.ToLower()));
        }
    }
}
