using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Vanp.DAL.Utils
{
    public enum Role
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Người bán")]
        Seller = 2,
        [Description("Người mua")]
        Buyer = 3
    }
}
