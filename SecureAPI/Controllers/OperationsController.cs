﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace SecureAPI.Controllers
{
    public class OperationsController : Controller
    {
        private IAuthorizationService _authorizationService;

        public OperationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Open()
        {
            var cookieJar = new CookieJar();
          
            await _authorizationService.AuthorizeAsync(User, cookieJar, CookieJarAuthOperations.Open);
            return View();
        }
    }

    public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, CookieJar cookieJar)
        {
            if (requirement.Name == CookieJarOpertions.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == CookieJarOpertions.ComeNear)
            {
                if (context.User.HasClaim("Friend", "Good"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }

    public static class CookieJarAuthOperations
    {
        public static OperationAuthorizationRequirement Open => new OperationAuthorizationRequirement
        {
            Name = CookieJarOpertions.Open
        };
    }

    public static class CookieJarOpertions
    {
        public static string Open = "Open";
        public static string TakeCookie = "TakeCookie";
        public static string ComeNear = "ComeNear";
        public static string Look = "Look";
    }

    public class CookieJar
    {
        public string Name { get; set; }
    }
}