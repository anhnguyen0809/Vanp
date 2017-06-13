using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Vanp.DAL.Utils
{
    public static class VanpMail
    {
        #region Head Mail 
        public const string _MAIL_HEADER = "<header>VANP</header>";
        public const string _MAIL_FOOTER = "<footer>Cám ơn bạn đã tham gia hệ thống của chúng tôi.</footer>";

        public static string GetMailTemplate(string htmlContent)
        {
            return @"
                    <!DOCTYPE html> <html> <head><title></title></head>
                    <body>"
                    + _MAIL_HEADER + "<div id=content>" + htmlContent + "/div>" + _MAIL_FOOTER + @"
                    </body>
                    </html>";
        }
        #endregion
        /// <summary>
        ///  Gửi Mail cho các bên liên quan nhằm thông báo: Ra giá thành công, giá sản phẩm được cập nhật
        /// </summary>
        /// <param name="productId">id sản phấm đấu giá thành công</param>
        /// <param name="userSellerId">id Người đăng bán</param>
        /// <param name="userCurrentId">id Người giữ giá hiện tại</param>
        /// <param name="userBeforeId">Id người giữ giá trước đó</param>
        /// <returns></returns>
        public static bool ProductBidSuccess(int productId, int userSellerId, int userCurrentId, int? userBeforeId = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var subject = "Thông Báo:  RA GIÁ THÀNH CÔNG!";
            var product = unitOfWork.ProductRepository.GetById(productId);
            var userSeller = unitOfWork.UserRepository.GetById(userSellerId);
            var userCurrent = unitOfWork.UserRepository.GetById(userCurrentId);
            
            var htmlProductInfo = $"<p><b>Mã sản phẩm: </b>{ product.ProductCode} <</p><p><b>Tên sản phẩm: </b>{ product.ProductName} </p> "
                                + $"< p><b>Giá hiện tại: </b>{product.PriceCurrent} </p>";

            if (userSeller != null && Validation.IsEmail(userSeller.Email))
            {
                string htmlSeller = @"<h3>Chúc mừng bạn đã có người đặt mua sản phẩm bạn.</h3>"
                                     + htmlProductInfo;
                Mail.SendMail(GetMailTemplate(htmlSeller), new string[] { userSeller.Email }, subject);
            }
            if (userCurrent != null && Validation.IsEmail(userCurrent.Email))
            {
                string htmlCurrent = @"<h3>Chúc mừng bạn đã đặt giá thành công.</h3>"
                                     + htmlProductInfo
                                     + $"<p><b>Ngày ra giá: </b>{product.BidDate}</p>"
                                     + $"<p><b>Mức giá: </b>{product.PriceMax}</p>";
                Mail.SendMail(GetMailTemplate(htmlCurrent), new string[] { userCurrent.Email }, subject);

            }
            if (userBeforeId.HasValue)
            {
                var userBefore = unitOfWork.UserRepository.GetById(userBeforeId);
                if (userBefore != null && Validation.IsEmail(userBefore.Email))
                {
                    string htmlBefore = @"<h3>Đã có người đặt giá cao hơn bạn. Bạn có muốn đấu giá tiếp không?.</h3>"
                                     + htmlProductInfo;
                    Mail.SendMail(GetMailTemplate(htmlBefore), new string[] { userBefore.Email }, subject);
                }
            }
            return true;
        }

    }
}