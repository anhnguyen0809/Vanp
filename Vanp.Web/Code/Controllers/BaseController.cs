using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web
{
    using DAL.Entites;
    using DAL.Utils;
    using Vanp.DAL;
    public class BaseController : Controller
    {
        public Vanp_Entities _context = new Vanp_Entities();
        public UnitOfWork _unitOfWork = new UnitOfWork();
        public string Success { set { ViewData["Success"] = value; } }
        public string Failure { set { ViewData["Failure"] = value; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TempData["Success"] != null) ViewData["Success"] = TempData["Success"];
            if (TempData["Failure"] != null) ViewData["Failure"] = TempData["Failure"];
            AuthService._unitOfWork = _unitOfWork;
            base.OnActionExecuting(filterContext);
        }
        public JsonResult JsonError(string message = "", JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return Json(new { error = StatusResult.Error, message = message}, behavior);
        }
        public JsonResult JsonSuccess(object data = null, string message = "", JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return Json(new { error = StatusResult.Success, message = message, data = data } , behavior);
        }
    }
}