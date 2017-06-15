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
        public IEnumerable<CategoryModel> ParentCategories { get; set; }

        public CategoryModel()
        {

        }

        public CategoryModel(Category category)
        {
            this.Id = category.Id;
            this.CategoryName = category.CategoryName;
            this.CategoryParentId = category.CategoryParentId;
            if (this.CategoryParentId.HasValue)
            {
                this.CategoryParentName = category.Category2.CategoryName;
                this.ParentCategories = new List<CategoryModel>();
                GetParentCategories(category, (List<CategoryModel>)this.ParentCategories);
            }
            this.SubCategories = category.Category1.Select(o => new CategoryModel()
            {
                Id = o.Id,
                CategoryName = o.CategoryName,
                CategoryParentId = o.CategoryParentId
            });
        }
        private void GetParentCategories(Category category, List<CategoryModel> categories)
        {
            if (category.Category2 == null)
                return;
            categories.Add(new CategoryModel()
            {
                Id = category.Category2.Id,
                CategoryName = category.Category2.CategoryName,
                CategoryParentId = category.Category2.CategoryParentId
            });
            GetParentCategories(category.Category2, categories);
        }
    }
}