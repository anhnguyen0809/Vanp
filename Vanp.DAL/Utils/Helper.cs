using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanp.DAL.Utils
{
    public class Helper
    {
        public static string Subtract(DateTime dateFrom , DateTime dateTo)
        {
            var dateSub = dateTo.Subtract(dateFrom);
            if (dateSub.TotalDays > 0)
            {
                return (int)dateSub.TotalDays + " ngày";
            } else if (dateSub.TotalHours > 0)
            {
                return (int)dateSub.TotalHours + " giờ";
            }
            else if (dateSub.TotalMinutes > 0)
            {
                return (int)dateSub.TotalMinutes + " phút";
            }
            else
            {
                return dateSub.TotalSeconds + " giây";
            }
        }
    }
}
