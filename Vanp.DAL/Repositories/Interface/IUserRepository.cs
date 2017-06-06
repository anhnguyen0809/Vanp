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
        bool IsExisted(string userNameOrEmail);
        bool IsExisted(string userNameOrEmail, string passWord);
        bool IsAuthorized(string userNameOrEmail);
        bool VerifyCode(string userNameOrEmail, string code);
        bool VerifyCode(int userId, string code);
        bool ResetPassWord(string userNameOrEmail);
        bool ChangePassWord(string userNameOrEmail, string passWordOld, string passWordNew);
        bool SendCode(int userId);
        User GetByUserName(string userName);
        User GetByEmail(string email);
        User GetByUserNameOrEmail(string userNameOrEmail);
    }
}
