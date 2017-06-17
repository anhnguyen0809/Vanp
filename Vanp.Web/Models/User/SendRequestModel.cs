using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class SendRequestModel
    {
        public string UserName { get; set; }
        public string RequestContent { get; set; }
    }
}