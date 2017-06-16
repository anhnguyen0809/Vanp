using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IWishlistRepository : IGeneralRepository<Wishlist>
    {
        bool IsExisted(int userId, int productId);
        Wishlist GetByUserAndProduct(int userId, int productId);
        IEnumerable<Wishlist> GetListByUser(int userId);
    }
}
