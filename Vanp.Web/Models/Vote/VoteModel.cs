using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class VoteModel : BaseModel
    {
        public short Vote { get; set; }
        public string Content { get; set; }

        public int? UserVotedId { get; set; }

        public int? ProductId { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByFullName { get; set; }
        public VoteModel()
        {

        }
        public VoteModel(Vote vote)
        {
            this.Id = vote.Id;
            this.CreatedBy = vote.CreatedBy;
            this.CreatedWhen = vote.CreatedWhen;
            if (this.CreatedBy.HasValue)
            {
                this.CreatedByName = vote.User.UserName;
                this.CreatedByFullName = vote.User.FullName;
            }
            this.Content = vote.Content;
            this.ProductId = vote.ProductId;
            this.Vote = vote.Vote1 ?? 0;
        }
    }
}