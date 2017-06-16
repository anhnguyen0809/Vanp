using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class BidHistoryController : AuthController
    {
        // GET: Customer/BidHistory
        public ActionResult BidHistory()
        {
            return View();
        }
    }
}