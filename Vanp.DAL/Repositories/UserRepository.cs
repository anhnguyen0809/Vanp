using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;
using Vanp.DAL.Utils;

namespace Vanp.DAL
{
    public class UserRepository : GeneralRepository<User>, IUserRepository
    {
        public UserRepository(Vanp_Entities context) : base(context)
        {
        }
        public bool IsExisted(string userNameOrEmail, int userId = 0)
        {
            return _dbSet.Any(o => userId != o.Id
                               && (o.UserName.ToUpper().Equals(userNameOrEmail.ToUpper())
                               || o.Email.ToUpper().Equals(userNameOrEmail.ToUpper())));
        }
        public bool IsExisted(string userNameOrEmail, string passWord)
        {
            return _dbSet.Any(o => ((o.UserName.ToUpper().Equals(userNameOrEmail.ToUpper())
                                || o.Email.ToUpper().Equals(userNameOrEmail.ToUpper()))
                                && o.UserPassword.Equals(passWord)));
        }
        public bool VerifyCode(string userNameOrEmail, string code)
        {
            var user = this.GetByUserNameOrEmail(userNameOrEmail);
            return VerifyCode(user.Id, code);
        }
        public bool VerifyCode(int userId, string code)
        {
            var user = this.GetById(userId);
            if (user != null)
            {
                if (user.VerificationCode.Equals(code))
                {
                    user.Authorized = true;
                    this.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool ChangePassWord(string userNameOrEmail, string passWordOld, string passWordNew)
        {
            var user = this.GetByUserNameOrEmail(userNameOrEmail);
            var passWordOldHash = Sercurity.CreateHashMD5(passWordOld);
            var passWordNewHash = Sercurity.CreateHashMD5(passWordNew);
            if (user != null)
            {
                if (user.UserPassword.Equals(passWordOldHash))
                {
                    user.UserPassword = passWordNewHash;
                    this.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public string ResetPassWord(string userNameOrEmail)
        {
            var user = this.GetByUserNameOrEmail(userNameOrEmail);
            if (user != null)
            {
                var passwordNew = RandomHelper.RandomString(10, true);
                user.UserPassword = Sercurity.CreateHashMD5(passwordNew);
                this.SaveChanges();
                Mail.SendMail("Mật khẩu mới của bạn: " + passwordNew, new string[] { user.Email }, "Reset Mật Khẩu");
                return passwordNew;
            }
            return string.Empty;
        }
        public bool SendCode(int userId)
        {
            var user = this.GetById(userId);
            if (user != null)
            {
                user.VerificationCode = RandomHelper.RandomCode(10);
                this.Update(user);
                this.SaveChanges();
                return true;
            }
            return false;
        }
        public bool IsAuthorized(string userNameOrEmail)
        {
            var user = this.GetByUserNameOrEmail(userNameOrEmail);
            if (user != null)
            {
                return user.Authorized ?? false;
            }
            return false;
        }
        public User GetByUserNameOrEmail(string userNameOrEmail)
        {
            return _dbSet.FirstOrDefault(o => o.UserName.ToUpper().Equals(userNameOrEmail.ToUpper())
                                            || o.Email.ToUpper().Equals(userNameOrEmail.ToUpper()));
        }
        public User GetByUserName(string userName)
        {
            return _dbSet.FirstOrDefault(o => o.UserName.ToUpper().Equals(userName.ToUpper()));
        }
        public User GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(o => o.Email.ToUpper().Equals(email.ToUpper()));
        }

        public bool IsPermissionSeller(int userId)
        {
            var user = this.GetById(userId);
            if (user != null)
            {
                if (user.RequestId.HasValue)
                {
                    if (user.Request.Approved.HasValue && user.Request.Approved.Value)
                    {
                        if (user.Request.DateTo.HasValue && user.Request.DateTo.Value.Date > DateTime.Now.Date)
                        {
                            return true;
                        }
                        else
                        {
                            var userRole = user.UserRoles.FirstOrDefault(o => o.RoleId == (int)Utils.Role.Seller && o.Enable == true);
                            if (userRole != null)
                            {
                                userRole.ModifiedWhen = DateTime.Now;
                                userRole.Enable = false;
                                this.SaveChanges();
                            }
                        }
                    }
                }
            }
            return false;
        }
        public override IEnumerable<User> GetList()
        {
            return _context.Users.Where(o => o.Enable == true);
        }

    }
}
