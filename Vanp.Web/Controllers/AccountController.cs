﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Vanp.Web.Models;
using Vanp.DAL.Entites;
using BotDetect.Web.Mvc;
using Vanp.DAL.Utils;

namespace Vanp.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        [HttpPost]
        public JsonResult IsNotExisted(string userName)
        {
            return Json(!_unitOfWork.UserRepository.IsExisted(userName));
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (loginModel != null)
            {
                if (AuthService.IsExisted(loginModel.UserName, loginModel.PassWord))
                {
                    var user = _unitOfWork.UserRepository.GetByUserNameOrEmail(loginModel.UserName);
                    if (!(user.Enable ?? false))
                    {
                        Failure = "Tài khoản đã bị khóa.";
                    }
                    else
                    {
                        AuthService.Login(loginModel);

                        user.LastLogon = DateTime.Now;
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.UserRepository.SaveChanges();
                        return RedirectToLocal(returnUrl);

                    }
                }
                Failure = "Tài khoản hoặc mật khẩu không chính xác.";
                return View(loginModel);
            }
            else
                return View(loginModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "RegistrationCaptcha")]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (registerModel != null)
            {
                MvcCaptcha.ResetCaptcha("RegistrationCaptcha");
                if (AuthService.IsExisted(registerModel.UserName))
                {
                    Failure = "Tài khoản đã tồn tại.";
                    return View(registerModel);
                }
                else
                {
                    User user = new User();
                    user.UserName = registerModel.UserName.ToLower();
                    user.UserPassword = Sercurity.CreateHashMD5(registerModel.PassWord);
                    user.Email = registerModel.Email.ToLower();
                    user.FullName = registerModel.FullName;
                    user.UserAddress = registerModel.Address;
                    user.UserPhone = registerModel.Phone;
                    user.IsCustomer = true;
                    user.DateOfBirth = registerModel.DateOfBirth;
                    user.Enable = true;
                    user.LastLogon = user.CreatedWhen = user.ModifiedWhen = DateTime.Now;
                    _unitOfWork.UserRepository.Insert(user);
                    _unitOfWork.Save();
                    user.CreatedBy = user.ModifiedBy = user.Id;
                    AuthService.Login(user);
                    _unitOfWork.Save();
                    return RedirectToAction("SendCode");
                }
            }
            else
                return View(registerModel);

        }
        public ActionResult ResetPassWord()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPassWord(AccountModel accountModel)
        {
            if (!string.IsNullOrEmpty(accountModel.PassWordNew) && !string.IsNullOrEmpty(accountModel.PassWordOld))
            {
                var result = AuthService.ResetPassWord(CurrentUser.UserName);
                if (result)
                {
                    Success = "Mật khẩu mới đã được gửi vào mail của bạn.";
                    return View();
                }
            }
            Failure = "Reset mật khẩu thất bại";
            return View();
        }
        #region SendCode
        [Authorize]
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
        public ActionResult Logout()
        {
            AuthService.Logout();
            return RedirectToAction("Index", "Home");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}