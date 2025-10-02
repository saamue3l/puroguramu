// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<PuroUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<PuroUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

        /*public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            _logger.LogInformation("Logout");
            returnUrl ??= Url.Content("~/");
            _logger.LogInformation("Logout apres URL");

            TempData["NotificationScript"] = $"toastr.success('Déconnexion confirmée!');";

            await _signInManager.SignOutAsync();
            _logger.LogInformation("Logout apres signout");

            if (returnUrl != null)
            {
                _logger.LogInformation("Redirect local");
                return LocalRedirect(returnUrl);
            }
            else
            {
                _logger.LogInformation("Rediract Page");
                return RedirectToPage("/accueil");
            }
        }*/
    }
}
