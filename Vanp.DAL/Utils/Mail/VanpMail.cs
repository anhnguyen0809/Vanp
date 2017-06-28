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
                    + _MAIL_HEADER + "<div id=content>" + htmlContent + "</div>" + _MAIL_FOOTER + @"
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
            var subject = $"Thông Báo:  RA GIÁ THÀNH CÔNG! - [{productId}]";
            var product = unitOfWork.ProductRepository.GetById(productId);
            var userSeller = unitOfWork.UserRepository.GetById(userSellerId);
            var userCurrent = unitOfWork.UserRepository.GetById(userCurrentId);

            var htmlProductInfo = $"<p><b>Mã sản phẩm: </b>{ product.ProductCode} </p><p><b>Tên sản phẩm: </b>{ product.ProductName} </p> "
                                + $"<p><b>Giá hiện tại: </b>{string.Format("{0:n0}", product.PriceCurrent)} </p>";

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
                                     + $"<p><b>Ngày ra giá: </b>{string.Format("{0:dd/MM/yyyy hh:mm:ss}", product.BidDate)}</p>"
                                     + $"<p><b>Mức giá: </b>{string.Format("{0:n0}", product.PriceBid)}</p>";
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
        public static bool ProductKicked(int productId, int userKickedId)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var subject = $"Thông Báo:  BẠN ĐÃ BỊ KÍCH RA KHỎI SẢN PHẨM! - [{productId}]";
            var product = unitOfWork.ProductRepository.GetById(productId);
            var userKicked = unitOfWork.UserRepository.GetById(userKickedId);

            var htmlProductInfo = $"<p><b>Mã sản phẩm: </b>{ product.ProductCode} </p><p><b>Tên sản phẩm: </b>{ product.ProductName} </p> "
                                + $"<p><b>Giá hiện tại: </b>{string.Format("{0:n0}", product.PriceCurrent)} </p>";

            if (userKicked != null && Validation.IsEmail(userKicked.Email))
            {
                string htmlSeller = @"<h3>Bạn đã bị kích ra khỏi sản phẩm và sẽ không thể tham gia đấu giá sản phẩm này được nữa</h3>"
                                     + htmlProductInfo;
                Mail.SendMail(GetMailTemplate(htmlSeller), new string[] { userKicked.Email }, subject);
            }
            return true;
        }
        public static bool ProductBidEndSuccess(int productId, int userSellerId, int userCurrentId)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var subject = $"Thông Báo:  ĐẤU GIÁ KẾT THÚC! - [{productId}]";
            var product = unitOfWork.ProductRepository.GetById(productId);
            var userSeller = unitOfWork.UserRepository.GetById(userSellerId);
            var userCurrent = unitOfWork.UserRepository.GetById(userCurrentId);

            var htmlProductInfo = $"<p><b>Mã sản phẩm: </b>{ product.ProductCode} </p><p><b>Tên sản phẩm: </b>{ product.ProductName} </p> "
                                + $"<p><b>Giá: </b>{string.Format("{0:n0}", product.PriceCurrent)} </p>"
                                + $"<p><b>Ngày kết thúc: </b>{string.Format("{0:dd/MM/yyyy hh:mm:ss}", product.BidDateEnd)}</p>";

            if (userSeller != null && Validation.IsEmail(userSeller.Email) && userCurrent != null && Validation.IsEmail(userCurrent.Email))
            {
                string htmlSeller = @"<h3>Đấu giá kết thúc! Chúc mừng bạn đã có người mua sản phẩm này.</h3>"
                                     + htmlProductInfo
                                     + "<h4>THÔNG TIN NGƯỜI MUA</h4>"
                                     + $"<p><b>Họ Tên:</b> {userCurrent.FullName}</p>"
                                     + $"<p><b>Tài Khoản:</b> {userCurrent.UserName}</p>"
                                     + $"<p><b>Điện Thoại:</b> {userCurrent.UserPhone}</p>";


                Mail.SendMail(GetMailTemplate(htmlSeller), new string[] { userSeller.Email }, subject);

                string htmlCurrent = @"<h3>Đấu giá kết thúc! Chúc mừng bạn đã mua được sản phẩm này.</h3>"
                                     + htmlProductInfo
                                     + $"<p><b>Ngày ra giá: </b>{string.Format("{0:dd/MM/yyyy hh:mm:ss}", product.BidDate)}</p>"
                                     + $"<p><b>Mức giá: </b>{string.Format("{0:n0}", product.PriceBid)}</p>"
                                     + "<h4>THÔNG TIN NGƯỜI BÁN</h4>"
                                     + $"<p><b>Họ Tên:</b> {userSeller.FullName}</p>"
                                     + $"<p><b>Tài Khoản:</b> {userSeller.UserName}</p>"
                                     + $"<p><b>Điện Thoại:</b> {userSeller.UserPhone}</p>";
                Mail.SendMail(GetMailTemplate(htmlCurrent), new string[] { userCurrent.Email }, subject);

            }
            return true;
        }
        public static bool ProductBidEndFailure(int productId, int userSellerId)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var subject = $"Thông Báo:  ĐẤU GIÁ KẾT THÚC! - [{productId}]";
            var product = unitOfWork.ProductRepository.GetById(productId);
            var userSeller = unitOfWork.UserRepository.GetById(userSellerId);

            var htmlProductInfo = $"<p><b>Mã sản phẩm: </b>{ product.ProductCode} </p><p><b>Tên sản phẩm: </b>{ product.ProductName} </p> "
                                + $"<p><b>Giá: </b>{string.Format("{0:n0}", product.PriceCurrent)} </p>"
                                + $"<p><b>Ngày kết thúc: </b>{string.Format("{0:dd/MM/yyyy hh:mm:ss}", product.DateTo)}</p>";

            if (userSeller != null && Validation.IsEmail(userSeller.Email))
            {
                string htmlSeller = @"<h3>Đấu giá kết thúc! Không có ai tham gia sản phẩm này.</h3>"
                                     + htmlProductInfo;
                Mail.SendMail(GetMailTemplate(htmlSeller), new string[] { userSeller.Email }, subject);
            }
            return true;
        }
    }
}