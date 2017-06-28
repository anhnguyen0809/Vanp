using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;
using Vanp.Web.Models;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class ProductController : AuthController
    {
        // GET: Customer/Product
        #region Insert - Update
        public ActionResult Insert()
        {
            if (!_unitOfWork.UserRepository.IsPermissionSeller(CurrentUser.Id ?? 0))
            {
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
                return Redirect("/account/sendrequest");
            }

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(ProductModel pro, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            if (!_unitOfWork.UserRepository.IsPermissionSeller(CurrentUser.Id ?? 0))
            {
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
                return Redirect("/account/sendrequest");
            }
            else
            {
                Product p = new Product();
                p.Enable = true;
                p.BidCount = 0;
                p.ModifiedWhen = DateTime.Now;
                p.CreatedWhen = DateTime.Now;
                p.CreatedBy = CurrentUser.Id;
                p.ModifiedBy = CurrentUser.Id;
                p.DateFrom = DateTime.Now;
                p.DateTo = DateTime.Now.AddDays(7);
                p.CategoryId = pro.CategoryId;
                p.PriceDefault = p.PriceCurrent = p.PriceMax = pro.PriceDefault;
                p.ProductName = pro.ProductName;
                p.PriceStep = pro.PriceStep;
                p.IsExtended = pro.IsExtended;
                p.ProductDescription = pro.ProductDescription;
                p.ProductText = pro.ProductText;
                p.Price = pro.Price;
                _unitOfWork.ProductRepository.Insert(p);
                _unitOfWork.Save();
                p.ProductImagePath = "/images/products/" + p.Id + "/img1.jpg";
                p.ProductCode = _unitOfWork.ProductRepository.GetCode(p.Id);
                _unitOfWork.ProductRepository.Update(p);
                _unitOfWork.Save();
                var spDirPath = Server.MapPath("~/images/products");
                var targetDirPath = Path.Combine(spDirPath, p.Id.ToString());
                Directory.CreateDirectory(targetDirPath);

                var img1Name = Path.Combine(targetDirPath, "img1.jpg");
                image1.SaveAs(img1Name);

                var img2Name = Path.Combine(targetDirPath, "img2.jpg");
                image2.SaveAs(img2Name);

                var img3Name = Path.Combine(targetDirPath, "img3.jpg");
                image3.SaveAs(img3Name);
                Success = "Đăng sản phẩm thành công.";
            }
            return View(pro);
        }

        public ActionResult Update(int id, string tab)
        {
            ViewBag.Tab = tab;
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null || product.CreatedBy != CurrentUser.Id)
            {
                Failure = "Sản phẩm này không nằm trong danh sách của bạn.";
                return RedirectToAction("ListProduct");
            }
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ProductModel pro, int Id)
        {
            Product p = _unitOfWork.ProductRepository.GetById(Id);
            if (p == null || p.CreatedBy != CurrentUser.Id)
            {
                Failure = "Sản phẩm này không nằm trong danh sách của bạn.";
                return RedirectToAction("ListProduct");
            }
            p.ProductTextLogPath = $"{DAL.Utils.Constant.PathSystem._PATH_DESCRIPTION_LOG}log_{p.Id}.txt";
            //Insert ProductText cũ vào Log File
            DAL.Models.DescriptionLog desLog = new DAL.Models.DescriptionLog()
            {
                CreatedWhen = p.ModifiedWhen ?? DateTime.Now,
                ProductId = p.Id,
                ProductText = p.ProductText
            };
            desLog.Insert(Server.MapPath(p.ProductTextLogPath));

            p.ModifiedWhen = DateTime.Now;
            p.ProductDescription = pro.ProductDescription;
            p.ProductText = pro.ProductText;

            _unitOfWork.ProductRepository.Update(p);
            _unitOfWork.Save();
            Success = "Cập nhập sản phẩm thành công.";
            return Redirect("/account/product/update/" + Id + "");
        }
        #endregion
        public ActionResult ListProduct()
        {
            int userId = Convert.ToInt16(CurrentUser.Id);
            return View(_unitOfWork.ProductRepository.GetListByUser(userId));
        }
        #region Bid và Mua Ngay
        [HttpPost]
        public ActionResult Bid(int id, double priceBid = 0)
        {
            ViewBag.ProductId = id;
            if (!_unitOfWork.ProductRepository.CheckBidPermisstion(CurrentUser.Id ?? 0, id))
            {
                Failure = "Bạn không thể đấu giá cho sản phẩm này. Do điểm đánh giá (+/+-) nhỏ hơn 80% hoặc do người đăng sản phẩm này đã kích bạn ra khỏi sản phẩm này.";
            }
            else if (!_unitOfWork.ProductRepository.ValidPriceBid(id, priceBid))
            {
                Failure = "Giá bạn đặt ra không hợp lệ. Nó phải lớn hơn giá hiện tại của sản phẩm.";
            }
            else if (_unitOfWork.ProductRepository.Bid(CurrentUser.Id ?? 0, id, priceBid))
            {
                Success = "Đấu giá thành công. Cám ơn bạn đã tham gia hệ thống của chúng tôi.";
            }
            else
            {
                Failure = "Đấu giá thất bại. ";
            }
            return Redirect($"/product-detail/{id}");
        }

        [HttpPost]
        public ActionResult BidSuccessful(int id)
        {
            if (_unitOfWork.ProductRepository.BidSuccessful(CurrentUser.Id ?? 0, id))
            {
                Success = "Sản phẩm đã được bạn mua. Cám ơn bạn đã tham gia hệ thống của chúng tôi.";
            }
            else
            {
                Failure = "Mua ngay thất bại. ";
            }
            return Redirect($"/product-detail/{id}");
        }
        #endregion
        #region Kich người mua ra
        [HttpPost]
        public ActionResult Kicked(int id, int userId)
        {
            if (_unitOfWork.ProductRepository.Kicked(id, CurrentUser.Id ?? 0, userId))
            {
                Success = "Đã kích người mua ra khỏi sản phẩm";
            }
            else
            {
                Failure = "Kích người mua không thành công";
            }
            return RedirectToAction("Update", "Product", new { id = id, tab = "bidhistory" });
        }
        #endregion
        #region Người dùng
        #region Danh sách người dùng đang tham gia đấu giá
        public ActionResult ProductsUserBidding()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsUserBidding(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.BidHistories
                .Where(o => o.CreatedBy.HasValue &&
                            o.CreatedBy == CurrentUser.Id &&
                            o.ProductId.HasValue && (!o.Product.IsBid.HasValue || o.Product.IsBid == false) &&
                            o.Product.DateTo.HasValue && o.Product.DateTo.Value >= DateTime.Now)
                .GroupBy(o => o.Product)
                .Select(o => o.FirstOrDefault().Product);
            var totalRows = products.Count();
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList().Select(o => new ProductModel(o)),
                totalRows = totalRows
            });
        }
        #endregion
        #region Danh sách người dùng đấu giá thắng
        public ActionResult ProductsUserSuccessful()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsUserSuccessful(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o => o.BidCurrentBy.HasValue &&
                            o.BidCurrentBy == CurrentUser.Id &&
                            o.IsBid.HasValue && o.IsBid.Value);
            var totalRows = products.Count();
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList()
                            .Select(o => new
                            {
                                product = new ProductModel(o),
                                votes = o.Votes.Select(v => new VoteModel(v)),
                                voted = o.Votes.Any(v => v.CreatedBy.HasValue && v.CreatedBy == CurrentUser.Id)
                            })
             ,
                totalRows = totalRows
            });
        }
        #endregion
        #region Danh sách người dùng được đánh giá
        public ActionResult ProductsUserVoted()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsUserVoted(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o =>
                            o.IsBid.HasValue && o.IsBid.Value &&
                            o.Votes.Any(v => v.UserVotedId.HasValue && v.UserVotedId == CurrentUser.Id));
            var totalRows = products.Count();
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList()
                            .Select(o => new
                            {
                                product = new ProductModel(o),
                                votes = o.Votes.Where(v => v.UserVotedId == CurrentUser.Id).Select(v => new VoteModel(v))
                            }),
                totalRows = totalRows
            });
        }
        #endregion
        #region Danh sách người dùng đã tham gia đấu giá
        [HttpGet]
        public JsonResult GetProductsBid(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o => o.BidHistories.Any(h => h.CreatedBy.HasValue && h.CreatedBy == CurrentUser.Id));
            var totalRows = products.Count();

            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList()
                            .Select(o => new
                            {
                                product = new ProductModel(o),
                                bidhistories = o.BidHistories
                                            .Where(v => v.CreatedBy == CurrentUser.Id)
                                            .OrderByDescending(v => v.Id)
                                            .Select(h => new BidHistoryModel(h))
                            }),
                totalRows = totalRows
            });
        }
        #endregion
        #endregion
        #region Người bán
        #region Danh sách sản phẩm đang đấu giá và còn hạn
        public ActionResult ProductsBidding()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsBidding(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o => o.CreatedBy.HasValue &&
                            o.CreatedBy == CurrentUser.Id &&
                            (!o.IsBid.HasValue || o.IsBid == false) &&
                            o.DateTo.HasValue && o.DateTo.Value >= DateTime.Now);
            var totalRows = products.Count();
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList().Select(o => new ProductModel(o)),
                totalRows = totalRows
            });
        }
        #endregion
        #region Danh sách đã có người mua
        public ActionResult ProductsSuccessful()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsSuccessful(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o => o.CreatedBy.HasValue &&
                            o.CreatedBy == CurrentUser.Id &&
                            o.IsBid.HasValue && o.IsBid.Value);
            var totalRows = products.Count();
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(new
            {
                products = products.ToList()
                            .Select(o => new
                            {
                                product = new ProductModel(o),
                                votes = o.Votes.Select(v => new VoteModel(v)),
                                voted = o.Votes.Any(v => v.CreatedBy.HasValue && v.CreatedBy == CurrentUser.Id)
                            }),
                totalRows = totalRows
            });
        }
        #endregion
        #endregion
    }
}