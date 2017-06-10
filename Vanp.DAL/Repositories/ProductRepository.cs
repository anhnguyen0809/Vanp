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
        public Product GetById(int productId)
        {
            return _dbSet.Single(p => p.Id == productId);
        }
        public bool isExisted(string productcode)
        {
            return _dbSet.Any(p=>p.ProductCode.ToLower().Equals(productcode.ToLower()));
        }
    }
}
