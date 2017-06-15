using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL;
using Vanp.DAL.Entites;
namespace Vanp.DAL
{
    public class ProductRepository : GeneralRepository<Product>, IProductRepository
    {
        public ProductRepository(Vanp_Entities context) : base(context)
        {
        }
        public IEnumerable<Product> GetListByProduct()
        {
            return _dbSet.ToList();
        }
        public IEnumerable<Product> GetListByProductOfCus(int userId)
        {
            return _dbSet.Where(p => p.CreatedBy == userId).ToList();
        }
        public bool isExisted(string code)
        {
            return _dbSet.Any(p => p.ProductCode.ToLower().Equals(code.ToLower()));
        }

        public IEnumerable<Product> GetListByCategory(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryId == categoryId);
        }
        public bool CheckBidPermisstion(int userId, int productId)
        {
            var userRepository = new UserRepository(_context);
            var user = userRepository.GetById(userId);
            if (user != null)
            {
                var voteUp = user.VoteUp ?? 0;
                var voteDown = user.VoteDown ?? 0;
                var totalVote = Math.Abs(voteUp) + Math.Abs(voteDown);
                //Kiểm tra tỉ lệ điểm đánh giá (+/+-) hơn 80% thì mới cho phép ra giá
                if (totalVote == 0 || voteUp / totalVote >= 0.8)
                {
                    //Kiểm tra user này có bị kích khỏi sản phẩm bởi người đăng hay không
                    var bProductKicked = _context.ProductKickeds.Any(o => o.ProductId == productId && o.UserKickedId == userId);
                    if (!bProductKicked)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// Kiểm tra giá của người dùng đặt ra có hợp lệ hay không
        /// </summary>
        /// <param name="productId">id sản phẩm</param>
        /// <param name="priceBid">giá đặt ra</param>
        /// <returns></returns>
        public bool ValidPriceBid(int productId, double priceBid)
        {
            var product = this.GetById(productId);
            if (product != null && priceBid > product.PriceCurrent)
            {
                return true;
            }
            return false;
        }
        public double GetPriceValid(int productId)
        {
            var product = this.GetById(productId);
            if (product != null)
            {
                return product.PriceCurrent ?? 0 + product.PriceStep ?? 0;
            }
            return 0;
        }

        public bool Bid(int userId, int productId, double priceBid)
        {
            var product = this.GetById(productId);
           
            if (product != null)
            {
                var priceCurrent = product.PriceCurrent ?? 0;
                var priceMax = product.PriceMax ?? 0;
                var bidCurrentBefore = product.BidCurrentBy;
                bool currentChanged = false;
                if (priceBid > product.PriceMax )
                {
                    product.PriceMax = priceBid;
                    product.PriceCurrent = priceMax + (product.PriceStep ?? 0);
                    product.BidCurrentBy = userId;
                    currentChanged = true;
                }
                else
                {
                    product.PriceCurrent = priceBid + (product.PriceStep ?? 0);
                }
                if (!(product.PriceCurrent < product.PriceMax))
                {
                    product.PriceCurrent = product.PriceMax;
                }
                product.PriceBid = priceBid;
                product.BidCount = (product.BidCount ?? 0) + 1;
                product.BidDate = DateTime.Now;

                #region Lưu lịch sử đấu giá
                //Lưu lịch sử đấu giá
                BidHistory bidHistory = new BidHistory();
                bidHistory.ModifiedBy = bidHistory.CreatedBy = userId;
                bidHistory.ModifiedWhen = bidHistory.CreatedWhen = DateTime.Now;
                bidHistory.Enable = true;
                bidHistory.ProductId = productId;
                bidHistory.PriceCurrent = priceCurrent;
                bidHistory.PriceMax = priceMax;
                bidHistory.PriceBid = priceBid;
                _context.BidHistories.Add(bidHistory);
                #endregion

                this.SaveChanges();

                #region Send Mail : Người bán , người ra giá thành công, người giữ giá trước đó
                if (bidCurrentBefore.HasValue && bidCurrentBefore.Value != userId && currentChanged)
                {
                    Utils.VanpMail.ProductBidSuccess(productId, product.CreatedBy ?? 0, userId, bidCurrentBefore);
                }
                else
                {
                    Utils.VanpMail.ProductBidSuccess(productId, product.CreatedBy ?? 0, userId);
                }
                #endregion

        
                return true;
            }
            return false;
        }
    }
}
