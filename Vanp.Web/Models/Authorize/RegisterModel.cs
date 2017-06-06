using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vanp.Web.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserRole { get; set; }
    }
}