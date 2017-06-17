﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public interface IProductRepository : IGeneralRepository<Product>
    {
        IEnumerable<Product> GetListByProduct();
      //  IEnumerable<Product> GetListByProduct(int[] productIds);
        bool isExisted(string code);
        IEnumerable<Product> GetListByCategory(int categoryId);
        IEnumerable<Product> GetListByCategory(int pageNo, int pageSize = 10, string orderBy = "", bool asc = true, int? category=null);
        /// <summary>
        /// Kiểm tra người dùng được phép đầu giá sản phẩm 
        /// </summary>
        /// <param name="userId">id người dùng</param>
        /// <param name="productId">id sản phẩm</param>
        /// <returns></returns>
        bool CheckBidPermisstion(int userId, int productId);

        /// <summary>
        /// Kiểm tra giá của người dùng đặt ra có hợp lệ hay không
        /// </summary>
        /// <param name="productId">id sản phẩm</param>
        /// <param name="priceBid">giá đặt ra</param>
        /// <returns></returns>
        bool ValidPriceBid(int productId, double priceBid);
        double GetPriceValid(int productId);
        /// <summary>
        /// Đặt giá 
        /// </summary>
        /// <param name="userId">id người đăt giá</param>
        /// <param name="productId">id sản phẩm</param>
        /// <param name="priceBid">giá đặt ra</param>
        /// <returns></returns>
        bool Bid(int userId, int productId, double priceBid);
        /// <summary>
        /// Mua ngay 
        /// </summary>
        /// <param name="userId">id người đăt giá</param>
        /// <param name="productId">id sản phẩm</param>
        /// <param name="priceBuy">giá mua ngay</param>
        /// <returns></returns>
        bool BuyNow(int userId, int productId, double priceBuy);
    }
}
