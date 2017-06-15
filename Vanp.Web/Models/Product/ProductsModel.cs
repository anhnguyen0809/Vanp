using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class ProductsModel
    {
        public string Q { get; set; }

        public string Sort { get; set; }
        public double PriceFrom { get; set; }
        public double PriceThru { get; set; }
        public int[] CategorieIds { get; set; }
        public bool Reset { get; set; }

        public IDictionary<string, string> SortItems { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}