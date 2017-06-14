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
        public bool isExisted(string code)
        {
            return _dbSet.Any(p => p.ProductCode.ToLower().Equals(code.ToLower()));
        }

        public IEnumerable<Product> GetListByCategory(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryId == categoryId);
        }
        public IEnumerable<Product> GetListByCategory(int pageNo, int pageSize = 10, string orderBy = "", bool asc = true, int? category = null)
        {
            var query = _dbSet.AsQueryable();
            if (category.HasValue)
            {
                query = query.Where(o => o.CategoryId == category);
            }
            if (orderBy.ToLower() == "dateto")
            {
                query = asc ? query.OrderBy(o => o.DateTo) : query.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                query = asc ? query.OrderBy(o => o.PriceCurrent) : query.OrderByDescending(o => o.PriceCurrent);
            }
            else
            {
                query = asc ? query.OrderBy(o => o.Id) : query.OrderByDescending(o => o.Id);
            }
            query = query
                 .Take(pageSize)
                 .Skip((pageNo - 1) * pageSize);

            return query.ToList();
        }

        #region Đấu giá
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

            if (product != null && _context.Users.Any(o => o.Id == userId))
            {
                var priceCurrent = product.PriceCurrent ?? 0;
                var priceMax = product.PriceMax ?? 0;
                var bidCurrentBefore = product.BidCurrentBy;
                bool currentChanged = false;
                if (priceBid > product.PriceMax)
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
        public bool BuyNow(int userId, int productId, double priceBuy)
        {
            var product = this.GetById(productId);

            if (product != null && _context.Users.Any(o => o.Id == userId))
            {
                if (product.Price <= priceBuy)
                {
                    #region Lưu lịch sử đấu giá
                    //Lưu lịch sử đấu giá
                    BidHistory bidHistory = new BidHistory();
                    bidHistory.ModifiedBy = bidHistory.CreatedBy = userId;
                    bidHistory.ModifiedWhen = bidHistory.CreatedWhen = DateTime.Now;
                    bidHistory.Enable = true;
                    bidHistory.ProductId = productId;
                    bidHistory.PriceCurrent = product.PriceCurrent;
                    bidHistory.PriceMax = product.PriceMax;
                    bidHistory.PriceBid = priceBuy;
                    _context.BidHistories.Add(bidHistory);
                    #endregion

                    product.IsBid = true;
                    product.BidDate = DateTime.Now;
                    product.PriceMax = priceBuy;
                    product.PriceBid = priceBuy;
                    product.BidCount = (product.BidCount ?? 0) + 1;
                    product.BidCurrentBy = userId;
                    this.SaveChanges();
                    //Send Mail Giao Dịch Thành công

                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
