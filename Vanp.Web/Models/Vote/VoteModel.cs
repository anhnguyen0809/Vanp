using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class VoteModel : BaseModel
    {
        public byte Vote { get; set; }
        public string Content { get; set; }

        public int? UserVotedId { get; set; }

        public int? ProductId { get; set; }
        public VoteModel()
        {

        }
    }
}