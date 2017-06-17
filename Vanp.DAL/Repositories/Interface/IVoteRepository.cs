using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL.Repositories.Interface
{
    public interface IVoteRepository : IGeneralRepository<Vote>
    {
        IEnumerable<Vote> GetListByVote(int id);
    }
}
