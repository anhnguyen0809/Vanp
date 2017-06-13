using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class RequestModel : BaseModel
    {
        public string RequestContent { get; set; }  
        public DateTime DateFrom { get; set; }  
        public DateTime DateTo { get; set; }  
    }
}