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
            var categories = _unitOfWork.CategoryRepository.GetListParentShow().ToList();
            return PartialView("_CategoriesLeft", categories);
        }
        public ActionResult GetListMenu()
        {
            var categories = _unitOfWork.CategoryRepository.GetListParentShow().ToList();
            return PartialView("_CategoriesMenu", categories);
        }
        public ActionResult GetListSearchMenu(string search, string cats)
        {
            ViewBag.SearchCategories = cats;
            ViewBag.Search = search;
            var categories = _unitOfWork.CategoryRepository.GetListParentShow().ToList();
            return PartialView("_CategoriesSearchMenu", categories);
        }
    }
}