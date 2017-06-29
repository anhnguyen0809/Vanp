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
<<<<<<< HEAD
        public bool isExisted(string productcode)
=======
        public bool isExisted(string code)
        {
            return _dbSet.Any(p=>p.ProductCode.ToLower().Equals(code.ToLower()));
        }
        public IEnumerable<Product> GetTopNNew(int n)              //Lấy n cái đầu tiên dựa vào id giảm dần
>>>>>>> 88275256eb0a67d56c2c86f95e3d486443945314
        {
            return _dbSet.OrderByDescending(o => o.Id).Take(n);
        }
    }
}
