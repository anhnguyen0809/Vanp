using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Vanp.DAL.Entites;

namespace Vanp.Web
{
    public static class CurrentUser
    {
        private static HttpCookie _userCookie = null;
        public static void SetUserCookie(User user = null, int expires = 1)
        {
            if (user != null)
            {

                string roleNames = Service.GetRoleNamesByUser(user.Id);

                FormsAuthentication.SetAuthCookie(user.UserName, true);

                var authTicket = new FormsAuthenticationTicket(1, user.UserName,
                    DateTime.Now, DateTime.Now.AddDays(expires), true, roleNames);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                _userCookie = userCookie;
                userCookie["id"] = user.Id.ToString();
                userCookie["userName"] = user.UserName;
                userCookie["email"] = user.Email;
                userCookie["fullName"] = user.FullName;
                userCookie["customer"] = (user.IsCustomer ?? false).ToString();
                userCookie["roles"] = roleNames;

                HttpContext.Current.Response.Cookies.Add(userCookie);
            }
            else
            {
                FormsAuthentication.SignOut();
                _userCookie = null;
            }
        }
        public static HttpCookie GetUserCookie()
        {
            return _userCookie;
        }
        public static bool IsAuthenticated
        {
            get
            {
                if (_userCookie == null)
                    return false;
                else
                {
                    return AuthService.IsAuthorized(UserName);
                }
            }
        }
        public static bool IsCustomer
        {
            get
            {
                return Convert.ToBoolean(_userCookie["customer"]);
            }
        }
        public static int? Id
        {
            get
            {
                if (_userCookie["id"] == null)
                    return null;
                return  Convert.ToUInt16(_userCookie["id"]);
            }
        }
        public static string UserName
        {
            get
            {
                return _userCookie["userName"];
            }
        }
        public static string Email
        {
            get
            {
                return _userCookie["email"];
            }
        }
        public static string FullName
        {
            get
            {
                return _userCookie["fullName"];
            }
        }
        public static string Roles
        {
            get
            {
                return _userCookie["roles"];
            }
        }


    }
}