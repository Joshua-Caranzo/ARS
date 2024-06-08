// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Controllers.Home
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IEventService _events;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger, IConfiguration configuration, IEventService events)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
            _events = events;
        }

        public IActionResult Index()
        {
            var client = _configuration.GetSection("ClientApps").Get<List<string>>();
            return Redirect(client[0]); 
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }

        public async Task<IActionResult> ExternalError()
        {
            var vm = new ErrorViewModel();
            var client = _configuration.GetSection("ClientApps").Get<List<string>>();
            vm.Error = new IdentityServer4.Models.ErrorMessage { Error = "User Not Found..", RedirectUri = client[0] };
            return View("Error", vm);
        }

        public async void LogoutExternalError()
        {
            bool isB2c = Convert.ToBoolean(_configuration.GetSection("ApplicationSettings:azurelogin").Value);

            if (isB2c)
            {
                //signout or remove all cokies
                if (isB2c)
                {
                    await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                    await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    await HttpContext.SignOutAsync("idsrv");
                    await HttpContext.SignOutAsync("idsrv.external");
                }
            }
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

        }
    }
}