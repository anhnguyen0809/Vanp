using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class RequestRepository : GeneralRepository<Request>, IRequestRepository
    {
        public RequestRepository(Vanp_Entities context) : base(context)
        {
        }

    }
}
