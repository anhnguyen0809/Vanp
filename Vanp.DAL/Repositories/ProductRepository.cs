using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
        public string GetCode(int productId)
        {
            var code = "P";
            var id_divide = productId / 1000000;
            var charCode = 65;
            while (id_divide > 1)
            {
                id_divide /= 10;
                charCode++;
            }
            code += (char)charCode + (productId % 1000000).ToString("D7");
            return code;
        }
        public IEnumerable<Product> GetListByUser(int userId)
        {
            return _dbSet.Where(p => p.CreatedBy == userId).OrderByDescending(o => o.Id).ToList();
        }
        public IEnumerable<Product> GetListByCategory(int categoryId)
        {
            return _dbSet.Where(o => o.CategoryId == categoryId && o.DateTo.HasValue && o.DateTo.Value >= DateTime.Now);
        }
        public IEnumerable<Product> GetListByCategory(string search = "", int[] categories = null, int? pageNo = null, int? pageSize = 10, string orderBy = "", bool asc = true)
        {
            var query = _dbSet.AsQueryable();

            if (orderBy.ToLower() == "dateto")
            {
                query = asc ? query.OrderBy(o => o.DateTo) : query.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                query = asc ? query.OrderBy(o => o.PriceCurrent) : query.OrderByDescending(o => o.PriceCurrent);
            }

            query = asc ? query.OrderBy(o => o.Id) : query.OrderByDescending(o => o.Id);

            if (categories != null && categories.Count() > 0)
            {
                var categoryRepository = new CategoryRepository(_context);
                var categoryIds = categoryRepository.GetListAllChildByParent(categories).Select(o => o.Id).ToList();
                categoryIds.AddRange(categories);
                query = query.Where(o => categoryIds.Contains(o.CategoryId.Value));
            }
            query = query
                    .Where(o => ( string.IsNullOrEmpty(search) || o.ProductName.ToUpper().Contains(search.ToUpper()))
                                && o.DateTo.HasValue && o.DateTo.Value >= DateTime.Now && (!o.IsBid.HasValue || !o.IsBid.Value));
            if ((pageNo ?? 0) > 0 && (pageSize ?? 0) > 0)
            {
                query = query.Skip((pageNo.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return query.ToList();
        }
        public IEnumerable<Product> GetListUserBidSuccess(int userId, int pageNo, int pageSize = 10, string orderBy = "", bool asc = true)
        {
            var query = _dbSet.AsQueryable();

            if (orderBy.ToLower() == "dateto")
            {
                query = asc ? query.OrderBy(o => o.DateTo) : query.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                query = asc ? query.OrderBy(o => o.PriceCurrent) : query.OrderByDescending(o => o.PriceCurrent);
            }

            query = asc ? query.OrderBy(o => o.Id) : query.OrderByDescending(o => o.Id);

            query = query
                    .Where(o => o.BidCurrentBy.HasValue && o.BidCurrentBy.Value == userId)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);

            return query.ToList();
        }
        #region Đấu giá
        public bool CheckBidPermisstion(int userId, int productId)
        {
            var userRepository = new UserRepository(_context);
            var user = userRepository.GetById(userId);
            if (user != null)
            {
                var voteUp = (float)(user.VoteUp ?? 0);
                var voteDown = (float)(user.VoteDown ?? 0);
                var totalVote = Math.Abs(voteUp) + Math.Abs(voteDown);
                //Kiểm tra tỉ lệ điểm đánh giá (+/+-) hơn 80% thì mới cho phép ra giá
                if (totalVote == 0 || (voteUp / totalVote) >= 0.8)
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
            var priceVaid = this.GetPriceValid(productId);
            if (priceVaid > 0 && priceBid >= priceVaid)
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
                return (product.PriceCurrent ?? 0) + (product.PriceStep ?? 0);
            }
            return 0;
        }

        public bool Bid(int userId, int productId, double priceBid)
        {
            var product = this.GetById(productId);

            if (product != null && !(product.IsBid ?? false) && _context.Users.Any(o => o.Id == userId))
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
                //Gia hạn :	Có tự động gia hạn ko? Nếu có, khi có lượt đấu giá mới trước khi kết thúc 5 phút, sản phẩm tự động gia hạn thêm 10p.
                if (product.IsExtended.HasValue && product.IsExtended.Value)
                {
                    var remaindTime = product.DateTo.Value.Subtract(DateTime.Now).TotalMinutes;
                    if (remaindTime > 0 && remaindTime <= 5)
                    {
                        product.DateTo = product.DateTo.Value.AddMinutes(10);
                    }
                }
                #region Lưu lịch sử đấu giá
                //Lưu lịch sử đấu giá
                BidHistory bidHistory = new BidHistory();
                bidHistory.ModifiedBy = bidHistory.CreatedBy = userId;
                bidHistory.ModifiedWhen = bidHistory.CreatedWhen = DateTime.Now;
                bidHistory.Enable = true;
                bidHistory.ProductId = productId;
                bidHistory.PriceCurrent = product.PriceCurrent;
                bidHistory.PriceMax = product.PriceMax;
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
        public bool BidSuccessful(int userId, int productId)
        {
            var product = this.GetById(productId);
            if (product != null && !(product.IsBid ?? false) && product.Price.HasValue && _context.Users.Any(o => o.Id == userId))
            {
                product.IsBid = true;
                product.BidDateEnd = product.BidDate = DateTime.Now;
                product.PriceCurrent = product.Price;
                product.PriceBid = product.Price;
                product.BidCount = (product.BidCount ?? 0) + 1;
                product.BidCurrentBy = userId;
                #region Lưu lịch sử đấu giá
                //Lưu lịch sử đấu giá
                BidHistory bidHistory = new BidHistory();
                bidHistory.ModifiedBy = bidHistory.CreatedBy = userId;
                bidHistory.ModifiedWhen = bidHistory.CreatedWhen = DateTime.Now;
                bidHistory.Enable = true;
                bidHistory.ProductId = productId;
                bidHistory.PriceCurrent = product.PriceCurrent;
                bidHistory.PriceMax = product.PriceMax;
                bidHistory.PriceBid = product.PriceBid;
                _context.BidHistories.Add(bidHistory);
                #endregion
                this.SaveChanges();
                //Send Mail Giao Dịch Thành công
                Utils.VanpMail.ProductBidEndSuccess(product.Id, product.CreatedBy ?? 0, product.BidCurrentBy ?? 0);
                return true;

            }
            return false;
        }
        #endregion

        public bool Kicked(int productId, int userId, int userKickedId)
        {
            var product = this.GetById(productId);
            if (product != null && !(product.IsBid ?? false))
            {
                //Danh sách lịch sử đấu giá mà người bị kích đã đấu
                var bidHistoriesUpdate = product.BidHistories.Where(o => o.CreatedBy.HasValue && o.CreatedBy.Value == userKickedId).ToList();
                if (bidHistoriesUpdate.Count() > 0)
                {
                    //Nếu người mua bị kick đang giữ giá, sản phẩm chuyển cho người mua có giá lớn nhất
                    if (product.BidCurrentBy.HasValue && product.BidCurrentBy == userKickedId)
                    {
                        var bidHistoryMax = product.BidHistories.Where(o => o.CreatedBy.HasValue && o.CreatedBy != userKickedId)
                            .OrderByDescending(o => o.PriceBid)
                            .FirstOrDefault();
                        if (bidHistoryMax != null)
                        {
                            //Cập nhập lại người giữ giá và giá sản phẩm theo người có giá lớn nhất
                            product.PriceCurrent = bidHistoryMax.PriceCurrent;
                            product.PriceBid = bidHistoryMax.PriceBid;
                            product.PriceMax = bidHistoryMax.PriceMax;
                            product.BidDate = bidHistoryMax.CreatedWhen;
                            product.BidCurrentBy = bidHistoryMax.CreatedBy;
                        }
                        else
                        {
                            //Nếu không tìm thấy người nào đấu giá khác thì set lại về ban đầu
                            product.PriceCurrent = product.PriceMax = product.PriceDefault;
                            product.PriceBid = 0;
                            product.BidDate = null;
                            product.BidCurrentBy = null;
                        }
                    }
                    //Cập nhập lại tổng số người tham gia đấu giá
                    product.BidCount = (product.BidCount ?? 0) - bidHistoriesUpdate.Count();
                    product.BidCount = product.BidCount > 0 ? product.BidCount : 0;
                    //Xóa lịch sử đấu giá của người dùng cho sản phẩm này
                    bidHistoriesUpdate.ForEach(o => o.Enable = false);
                    //Thêm người dùng vào danh sách bị kích của sản phẩm này
                    ProductKicked productKicked = new ProductKicked()
                    {
                        CreatedBy = userId,
                        ModifiedBy = userId,
                        CreatedWhen = DateTime.Now,
                        ModifiedWhen = DateTime.Now,
                        Enable = true,
                        UserKickedId = userKickedId,
                        ProductId = productId
                    };
                    _context.ProductKickeds.Add(productKicked);
                    this.SaveChanges();
                    //Send Mail gửi tới người bị kích
                    Utils.VanpMail.ProductKicked(productId, userKickedId);

                    return true;
                }
            }
            return false;
        }

        #region Daily
        public void ProductBiEndByTime()
        {
            //Danh sách các product chưa giao dịch thành công  & kết thúc tại thời điểm này 
            var products = _dbSet.Where(o => !o.IsBid.HasValue
                        && DbFunctions.DiffMilliseconds(o.DateTo.Value, DateTime.Now) >= 0);
            //Sản phẩm đã có người giữ giá
            var productBidEndSuccesses = products.Where(o => o.BidCurrentBy.HasValue).ToList();
            //Sản phẩm chưa có người giữ giá
            var productBidEndFailure = products.Where(o => !o.BidCurrentBy.HasValue).ToList();
            //Cập nhập lại danh sách thành công
            productBidEndSuccesses.ForEach(o => { o.IsBid = true; o.BidDateEnd = o.DateTo; });
            productBidEndFailure.ForEach(o => { o.IsBid = false; o.BidDateEnd = o.DateTo; });
            this.SaveChanges();
            //SendMail
            productBidEndSuccesses.ForEach(o => Utils.VanpMail.ProductBidEndSuccess(o.Id, o.CreatedBy ?? 0, o.BidCurrentBy ?? 0));
            productBidEndFailure.ForEach(o => Utils.VanpMail.ProductBidEndFailure(o.Id, o.CreatedBy ?? 0));
        }
        #endregion
    }
}
