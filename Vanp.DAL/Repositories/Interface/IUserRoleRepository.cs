using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IUserRoleRepository : IGeneralRepository<UserRole>
    {
        IEnumerable<UserRole> GetListByUser(int userId);
        string GetRoleNamesByUser(int userId);
    }
}
