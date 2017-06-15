﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var user = httpContext.User;

            if (request.IsAjaxRequest())
            {
                if (user.Identity.IsAuthenticated == false)
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                else
                    response.StatusCode = (int)HttpStatusCode.Forbidden;

                response.SuppressFormsAuthenticationRedirect = true;
                response.End();
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}