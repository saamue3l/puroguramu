#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<PuroUser> _signInManager;
        private readonly UserManager<PuroUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<PuroUser> signInManager, UserManager<PuroUser> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Matricule/Adresse mail")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$|^[a-zA-Z]\d{6}$", ErrorMessage = "L'entrée doit être soit une adresse mail valide, soit une lettre suivie de six chiffres.")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var isEmail = Input.Username.Contains("@");
                PuroUser user;

                if (isEmail)
                {
                    user = await _userManager.FindByEmailAsync(Input.Username);
                }
                else
                {
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == Input.Username);
                }


                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, Input.Password);

                if (passwordValid)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    TempData["NotificationType"] = "success";
                    TempData["NotificationMessage"] = "Connexion réussie!";
                    if (roles.Contains("Etudiant"))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User logged in.");
                        return RedirectToPage("/Students/Index");
                    }
                    else if (roles.Contains("Enseignant"))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User logged in.");
                        return RedirectToPage("/Teachers/Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
