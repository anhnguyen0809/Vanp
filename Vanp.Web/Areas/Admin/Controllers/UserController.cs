using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Utils;

namespace Vanp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetList()
        {
            return JsonSuccess(_unitOfWork.UserRepository.GetList().ToList().Select(o => new
            {
                o.Id,
                o.CreatedBy,
                o.CreatedWhen,
                o.UserName,
                o.Email,
                o.LastLogon,
                o.Authorized,
                o.VoteUp,
                o.VoteDown
            }));
        }
     
    }
}