using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class UserRoleRepository : GeneralRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<UserRole> GetListByUser(int userId)
        {
            return _dbSet.Where(o => o.UserId == userId);
        }
        public string GetRoleNamesByUser(int userId)
        {
            var roleNames = _dbSet.Where(o => o.UserId == userId).Select(o => o.Role.RoleName);
            if (roleNames.Count() > 0)
                return string.Join( ",", roleNames);
            return string.Empty;
        }
    }
}
