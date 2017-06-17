using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;
using Vanp.DAL.Repositories.Interface;

namespace Vanp.DAL.Repositories
{
    public class VoteRepository:GeneralRepository<Vote>,IVoteRepository
    {
        public VoteRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<Vote> GetListByVote(int id)
        {
            return _dbSet.Where(v => v.UserVotedId == id).ToList();
        }
    }
}
