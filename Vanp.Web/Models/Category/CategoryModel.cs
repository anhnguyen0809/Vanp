using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class CategoryModel : BaseModel
    {
        public string CategoryName { get; set; }  
        public int? CategoryParentId { get; set; }  
        public string CategoryParentName { get; set; } 
        public IEnumerable<CategoryModel> SubCategories { get; set; }
    }
}