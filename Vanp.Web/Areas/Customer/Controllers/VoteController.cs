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
    public class VoteController : AuthController
    {
        public JsonResult Insert(VoteModel voteModel)
        {
            if (voteModel.ProductId.HasValue)
            {
                var product = _unitOfWork.ProductRepository.GetById(voteModel.ProductId);
                if (product != null && product.CreatedBy.HasValue)
                {
                    Vote vote = new Vote();
                    vote.Content = voteModel.Content;
                    vote.CreatedWhen = vote.ModifiedWhen = DateTime.Now;
                    vote.Enable = true;
                    vote.ProductId = voteModel.ProductId;
                    vote.CreatedBy = vote.ModifiedBy = CurrentUser.Id;
                    var point = voteModel.Vote;
                    if (point != 0)
                    {
                        point = (byte)(point / Math.Abs(point));
                    }
                    vote.Vote1 = voteModel.Vote;
                    if (product.CreatedBy == CurrentUser.Id) //Đánh giá từ người bán
                    { 
                        vote.UserVotedId = product.BidCurrentBy;
                        vote.VoteBySeller = true;
                    }
                    else
                    {
                        vote.UserVotedId = product.CreatedBy;
                        vote.VoteBySeller = false;
                    }
                    _context.Votes.Add(vote);
                    _context.SaveChanges();
                    return JsonSuccess( message: "Đánh giá người dùng thành công.");
                }
            }
            return JsonError("Không tìm thấy sản phẩm để đánh giá.");
        }
    }
}