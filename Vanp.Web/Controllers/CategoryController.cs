using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL;
using Vanp.DAL.Entites;

namespace Vanp.Web.Controllers
{
    [AllowAnonymous]
    public class CategoryController : BaseController
    {                                  
        // GET: Product
        public ActionResult GetList()
        {
            var categories = _unitOfWork.CategoryRepository.GetListParent().ToList();
            return PartialView( "_CategoriesLeft", categories);
        }
        [HttpPost]
        public ActionResult AddCategory(Category model)
        {
            using (var ctx = new Vanp_Entities())
            {
                ctx.Categories.Add(model);
                ctx.SaveChanges();
            }
                return View();
        } 
    }
}