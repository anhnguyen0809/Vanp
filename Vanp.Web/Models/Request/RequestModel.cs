using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class RequestModel : BaseModel
    {
        public string CreatedByName { get; set; }
        public string RequestContent { get; set; }  
        public DateTime? DateFrom { get; set; }  
        public DateTime? DateTo { get; set; }
        public RequestModel()
        {

        }
        public RequestModel(Request request)
        {
            this.Id = request.Id;
            this.CreatedWhen = request.CreatedWhen;
            if (CreatedWhen.HasValue)
            {
                this.CreatedByName = request.User.UserName;
            }
            this.RequestContent = request.RequestContent;
            this.DateFrom = request.DateFrom;
            this.DateTo = request.DateTo;
        }
    }
}