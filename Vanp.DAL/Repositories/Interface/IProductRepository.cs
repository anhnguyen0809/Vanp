using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IProductRepository: IGeneralRepository<Product>
    {
        IEnumerable<Product> GetListbyProducts();
        IEnumerable<Product> DetailProducts(int productID);
        bool isExisted(string productcode);
    }
}
