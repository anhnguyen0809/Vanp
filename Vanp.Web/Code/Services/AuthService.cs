using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL;
using Vanp.DAL.Utils;
using Vanp.Web.Models;

namespace Vanp.Web
{
    public static class AuthService
    {
        private static readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public static bool IsExisted(string userNameOrEmail)
        {
            return _unitOfWork.UserRepository.IsExisted(userNameOrEmail);
        }
        public static bool IsExisted(string userNameOrEmail, string passWord)
        {
            var passWordHash = Sercurity.CreateHashMD5(passWord);
            return IsExistedWithPassWordHash(userNameOrEmail, passWordHash);
        }
        public static bool IsExistedWithPassWordHash(string userNameOrEmail, string passWordHash)
        {
            return _unitOfWork.UserRepository.IsExisted(userNameOrEmail, passWordHash);
        }
        public static string ResetPassWord(string userNameOrEmail)
        {
            return _unitOfWork.UserRepository.ResetPassWord(userNameOrEmail);
        }
        public static bool ChangePassWord(string userNameOrEmail, string passWordOld, string passWordNew)
        {
            var passWordOldHash = Sercurity.CreateHashMD5(passWordOld);
            var passWordNewHash = Sercurity.CreateHashMD5(passWordNew);
            return _unitOfWork.UserRepository.ChangePassWord(userNameOrEmail, passWordOldHash, passWordNewHash);
        }
        public static bool VerifyCode(string userNameOrEmail, string code)
        {
            return _unitOfWork.UserRepository.VerifyCode(userNameOrEmail, code);
        }
        public static bool VerifyCode(int userId, string code)
        {
            return _unitOfWork.UserRepository.VerifyCode(userId, code);
        }
        public static bool IsAuthorized(string userNameOrEmail)
        {
            return _unitOfWork.UserRepository.IsAuthorized(userNameOrEmail);
        }
        public static void Logout()
        {
            CurrentUser.SetUserCookie(null);
        }
        public static void Login(LoginModel loginModel)
        {
            var user = _unitOfWork.UserRepository.GetByUserNameOrEmail(loginModel.UserName);
            CurrentUser.SetUserCookie(user);
        }
        public static void Login(DAL.Entites.User user)
        {
            CurrentUser.SetUserCookie(user);
        }

    }
}