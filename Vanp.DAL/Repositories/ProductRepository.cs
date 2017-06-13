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
        public bool isExisted(string code)
        {
            return _dbSet.Any(p=>p.ProductCode.ToLower().Equals(code.ToLower()));
        }
        public IEnumerable<Product> GetTopNNew(int n)              //Lấy n cái đầu tiên dựa vào id giảm dần
        {
            return _dbSet.OrderByDescending(o => o.Id).Take(n);
        }
    }
}
