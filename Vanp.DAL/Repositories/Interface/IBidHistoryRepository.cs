using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IBidHistoryRepository : IGeneralRepository<BidHistory>
    { 
        IEnumerable<BidHistory> GetListByUser(int userId);
        IEnumerable<BidHistory> GetListByProduct(int productId);
    }
}
