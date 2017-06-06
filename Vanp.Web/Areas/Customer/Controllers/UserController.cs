using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Utils;
using Vanp.Web.Models;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class UserController : AuthController
    {
        // GET: Customer/User
        public ActionResult Index()
        {
            var user = _unitOfWork.UserRepository.GetById(CurrentUser.Id);
            AccountModel model = new AccountModel(user);
            return View(model);
        }
        public ActionResult UserProfile()
        {
            var user = _unitOfWork.UserRepository.GetById(CurrentUser.Id);
            AccountModel model = new AccountModel(user);
            return View(model);
        }
        [HttpPost]
        public ActionResult UserProfile(AccountModel accountModel)
        {
            if (accountModel != null)
            {
                var user = _unitOfWork.UserRepository.GetById(CurrentUser.Id);
                user.FullName = accountModel.FullName;
                user.UserAddress = accountModel.Address;
                user.UserPhone = accountModel.Phone;
                user.IsCustomer = true;
                user.DateOfBirth = accountModel.DateOfBirth;
                user.Enable = true;
                user.ModifiedWhen = DateTime.Now;
                user.ModifiedBy = user.Id;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                return View(accountModel);
            }
            else
                return View(accountModel);

        }
        [HttpPost]
        public JsonResult ChangePassWord(AccountModel accountModel)
        {
            if (!string.IsNullOrEmpty(accountModel.PassWordNew) && !string.IsNullOrEmpty(accountModel.PassWordOld))
            {
                var result = AuthService.ChangePassWord(CurrentUser.UserName, accountModel.PassWordOld, accountModel.PassWordNew);
                if (result) return JsonSuccess(null, "Đổi mật khẩu thành công.");
            }
            return JsonError("Mật khẩu cũ không trùng khớp hoặc mật khẩu không hợp lệ");
        }
 
        public ActionResult VerifyCode(string userName, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (CurrentUser.Email.Equals(userName) || CurrentUser.UserName == userName)
                {
                    var result = _unitOfWork.UserRepository.VerifyCode(CurrentUser.Id.Value, code);
                    if (result)
                    {
                        Success = "Tài khoản đã được xác thực.";
                        return RedirectToAction("UserProfile", "User");
                    };
                }
                else
                {
                    Failure = "Mã xác thực không khớp! Vui lòng check email để lấy mã xác thực";
                }
            }
            else
            {
                Failure = "Mã xác thực không khớp! Vui lòng check email để lấy mã xác thực";
            }
            return View();
        }

    }
}