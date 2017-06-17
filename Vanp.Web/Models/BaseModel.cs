using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class BaseModel
    {
       public int Id { get; set; }
       public int? ModifiedBy { get; set; }
       public int? CreatedBy { get; set; }
       public DateTime? ModifiedWhen { get; set; }
       public DateTime? CreatedWhen { get; set; }
       public bool Enable { get; set; }
    }
}