using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Utils;
using Vanp.Web.Models;

namespace Vanp.Web.Areas.Customer.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: Customer/User
        public ActionResult Index()
        {
            var user = _unitOfWork.UserRepository.GetById(CurrentUser.Id);
            AccountModel model = new AccountModel(user);
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeAccount(AccountModel accountModel)
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
        #region SendCode
        public ActionResult SendCode()
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(CurrentUser.Id);
                user.VerificationCode = RandomHelper.RandomCode(10);
                user.ModifiedBy = user.Id;
                user.ModifiedWhen = DateTime.Now;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                string urlVerifyCode = Url.Action("VerifyCode", "user", new { userName = user.Email, code = user.VerificationCode }, this.Request.Url.Scheme);
                Mail.SendMail(urlVerifyCode, new string[] { user.Email }, "Xác thực tài khoản");
                Success = "Mã xác thực đã được gửi vào email của bạn! Hãy kiểm tra hòm thư để xác nhận.";
            }
            catch (Exception ex)
            {
                Failure = "Đã xảy ra lỗi trong quá trình gửi mã xác thực.";
            }
            return View();
        }
        #endregion
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
                        RedirectToAction("Index", "User");
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