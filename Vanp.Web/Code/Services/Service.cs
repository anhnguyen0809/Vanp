using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL;
using Vanp.DAL.Utils;
using Vanp.Web.Models;

namespace Vanp.Web
{
    public static class Service
    {
        private static readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public static string GetRoleNamesByUser(int userId)
        {
            return _unitOfWork.UserRoleRepository.GetRoleNamesByUser(userId);
        }

    }
}