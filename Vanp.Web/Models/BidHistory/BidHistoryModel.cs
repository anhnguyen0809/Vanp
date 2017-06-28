using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class BidHistoryModel : BaseModel
    {
        public string CreatedByName { get; set; }
        public double PriceCurrent { get; set; }
        public double PriceBid { get; set; }
        public BidHistoryModel()
        {

        }
        public BidHistoryModel(BidHistory bidHistory)
        {
            if (bidHistory.CreatedBy.HasValue)
            {
                this.CreatedByName = DAL.Utils.Helper.GetUserHash(bidHistory.User.UserName);
            }
            this.CreatedWhen = bidHistory.CreatedWhen;
            this.PriceCurrent = bidHistory.PriceCurrent ?? 0;
            this.PriceBid = bidHistory.PriceBid ?? 0;
        }
    }
}