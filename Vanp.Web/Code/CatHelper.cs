using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

namespace Vanp.Web.Code
{
    
    public static class CatHelper
    {
        public static Vanp_Entities _context = new Vanp_Entities();
        public static List<SelectListItem> GetNameCat(HtmlHelper html)
        {
            var listCat = new List<SelectListItem>();
            foreach (var c in _context.Categories.ToList())
            {
                listCat.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName
                });
            }
            return listCat;
        }
    }
}