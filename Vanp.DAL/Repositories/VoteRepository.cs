using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class VoteRepository : GeneralRepository<Vote>, IVoteRepository
    {
        public VoteRepository(Vanp_Entities context) : base(context)
        {
            
        }
        public override void Insert(Vote entity)
        {
            if (entity.UserVotedId.HasValue)
            {
                var user = _context.Users.Single(o => o.Id == entity.UserVotedId.Value);
                if (entity.Vote1 > 0)
                {
                    user.VoteUp = (user.VoteUp ?? 0) + entity.Vote1;
                }
                else
                {
                    user.VoteDown = (user.VoteDown ?? 0) + (-1)*entity.Vote1;
                }
            }
            base.Insert(entity);
        }
    }
}
