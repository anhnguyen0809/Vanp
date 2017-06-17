using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web.Areas.Customer.Controllers
{
    [Authorize]
    public class InfoVoteController : BaseController
    {
        // GET: Customer/Info
        public ActionResult Appraise()
        {
            int id = Convert.ToInt16(CurrentUser.Id);
            return View(_unitOfWork.VoteRepository.GetListByVote(id));
        }
    }
}