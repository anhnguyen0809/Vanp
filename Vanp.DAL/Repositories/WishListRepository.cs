using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class WishlistRepository : GeneralRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(Vanp_Entities context) : base(context)
        {
        }
        public bool IsExisted(int userId, int productId)
        {
            return _dbSet.Any(o => o.UserId == userId && o.ProductId == productId);
        }
        public IEnumerable<Wishlist> GetListByUser(int userId)
        {
            return _dbSet.Where(o => o.UserId == userId);
        }

    }
}
