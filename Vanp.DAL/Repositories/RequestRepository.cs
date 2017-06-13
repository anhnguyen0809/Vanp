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
        /// <summary>
        ///  Duyệt yêu cầu đăng bán từ người dùng
        /// </summary>
        /// <param name="requestId">Mã yêu cầu</param>
        /// <param name="approvedBy">Người duyệt yêu cầu</param>
        /// <returns></returns>
        public bool Approved(int requestId, int approvedBy)
        {
            var request = this.GetById(requestId);
            if (request != null)
            {
                request.Approved = true;
                request.ApprovedBy = approvedBy;
                var user = _context.Users.SingleOrDefault(o => o.Id == request.CreatedBy);
                if (user != null)
                {
                    user.RequestId = request.Id;
                    //Thêm Role Saller cho khách hàng  nếu requesttype = 1
                    if (request.RequestTypeId == 1)
                    {
                        //Kiểm tra đã có user này có role Seller chưa? RollId = 2 
                        //Y: set Enable = 1; 
                        //N: Thêm role Seller cho user này
                        var isExistedRole = user.UserRoles.Any(o => o.RoleId == 2);
                        if (isExistedRole)
                        {
                            var userRole = user.UserRoles.FirstOrDefault(o => o.RoleId == 2);
                            userRole.Enable = true;
                            userRole.ApprovedBy = approvedBy;
                            userRole.ModifiedWhen = DateTime.Now;
                            userRole.ModifiedBy = approvedBy;
                        }
                        else
                        {
                            UserRole userRole = new UserRole() {
                                CreatedBy = approvedBy,
                                ModifiedBy = approvedBy,
                                CreatedWhen = DateTime.Now,
                                ModifiedWhen = DateTime.Now,
                                RoleId = 2,
                                UserId = user.Id,
                                Enable = true,
                                ApprovedBy = approvedBy
                            };
                            _context.UserRoles.Add(userRole);
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
