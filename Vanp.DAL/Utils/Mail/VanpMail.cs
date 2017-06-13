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
        /// <summary>
        ///  Gửi Mail cho các bên liên quan nhằm thông báo: Ra giá thành công, giá sản phẩm được cập nhật
        /// </summary>
        /// <param name="productId">id sản phấm đấu giá thành công</param>
        /// <param name="userSellerId">id Người đăng bán</param>
        /// <param name="userCurrentId">id Người giữ giá hiện tại</param>
        /// <param name="userBeforeId">Id người giữ giá trước đó</param>
        /// <returns></returns>
        public static bool  ProductBidSuccess(int productId, int userSellerId, int userCurrentId , int? userBeforeId = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var userSeller = unitOfWork.UserRepository.GetById(userSellerId);
            var userCurrent = unitOfWork.UserRepository.GetById(userCurrentId);
            var userBefore = unitOfWork.UserRepository.GetById(userBeforeId);
            return true;
        }   
    }
}