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
        /// <summary>
        ///  Duyệt yêu cầu đăng bán từ người dùng
        /// </summary>
        /// <param name="requestId">Mã yêu cầu</param>
        /// <param name="approvedBy">Người duyệt yêu cầu</param>
        /// <returns></returns>
        bool Approved(int requestId, int approvedBy);
        /// <summary>
        ///  Danh sách yêu cầu chưa được duyệt
        /// </summary>
        IEnumerable<Request> GetListNotApproved();
    }
}
