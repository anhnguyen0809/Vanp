using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class AccountModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PassWordOld { get; set; }
        public string PassWordNew { get; set; }
        public string VerificationCode { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }
        public string AvartarPath { get; set; }
        public string RoleName { get; set; }
        public AccountModel()
        {

        }
        public AccountModel(User user)
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.FullName = user.FullName;
            this.Phone = user.UserPhone;     
            this.Address = user.UserAddress;
            this.DateOfBirth = user.DateOfBirth;
            this.VoteDown = user.VoteDown ?? 0;
            this.VoteUp = user.VoteUp ?? 0;
            this.AvartarPath = user.AvartarPath;
        }
    }
}