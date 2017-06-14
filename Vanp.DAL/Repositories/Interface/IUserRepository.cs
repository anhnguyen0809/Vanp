using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IUserRepository : IGeneralRepository<User>
    {
        //Kiểm tra User tồn tại
        bool IsExisted(string userNameOrEmail, int userId = 0);
        bool IsExisted(string userNameOrEmail, string passWord);
        bool IsAuthorized(string userNameOrEmail);
        bool VerifyCode(string userNameOrEmail, string code);
        bool VerifyCode(int userId, string code);
        string ResetPassWord(string userNameOrEmail);
        bool ChangePassWord(string userNameOrEmail, string passWordOld, string passWordNew);
        bool SendCode(int userId);
        User GetByUserName(string userName);
        User GetByEmail(string email);
        User GetByUserNameOrEmail(string userNameOrEmail);
        bool IsPermissionSeller(int userId);
    }
}
