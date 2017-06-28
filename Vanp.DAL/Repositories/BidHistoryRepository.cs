using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class BidHistoryRepository : GeneralRepository<BidHistory>, IBidHistoryRepository
    {
        public BidHistoryRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<BidHistory> GetListByUser(int userId)
        {
            return _dbSet.Where(o => o.CreatedBy.HasValue && o.CreatedBy.Value == userId);
        }
        public IEnumerable<BidHistory> GetListByProduct(int productId)
        {
            return _dbSet.Where(o => o.ProductId.HasValue && o.ProductId.Value == productId && o.Enable.HasValue && o.Enable == true);
        }
    }
}
