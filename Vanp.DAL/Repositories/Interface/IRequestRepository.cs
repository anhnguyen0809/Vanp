using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IRequestRepository : IGeneralRepository<Request>
    {
        bool Approved(int requestId, int approvedBy);
    }
}
