// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<PuroUser> _signInManager;
        private readonly UserManager<PuroUser> _userManager;
        private readonly IUserStore<PuroUser> _userStore;
        private readonly IUserEmailStore<PuroUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IGroupeRepository _repository;

        public RegisterModel(
            UserManager<PuroUser> userManager,
            IUserStore<PuroUser> userStore,
            SignInManager<PuroUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IGroupeRepository groupeRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _repository = groupeRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Email")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "L'adresse mail n'est pas valide")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation du mot de passe ne correspondent pas.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Matricule")]
            [RegularExpression(@"^[a-zA-Z]\d{6}$", ErrorMessage = "Le matricule doit être une lettre suivie de six chiffres.")]
            public string Matricule { get; set; }

            [Required]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required]
            [Display(Name = "Prenom")]
            public string Prenom { get; set; }

            [Required]
            [Display(Name = "Groupe")]
            public int IDGroupe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ResetGroupes();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/IndexStudent");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var existingUserWithEmail = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUserWithEmail != null)
                {
                    ResetGroupes();

                    ModelState.AddModelError("Input.Email", "L'email est déjà utilisé.");
                    return Page();
                }

                var existingUserWithMatricule = await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == Input.Matricule.ToLower());
                if (existingUserWithMatricule)
                {
                    ResetGroupes();

                    ModelState.AddModelError("Input.Matricule", "Le matricule est déjà utilisé.");
                    return Page();
                }

                var user = CreateUser();
                user.Nom = Input.Nom;
                user.Prenom = Input.Prenom;
                user.UserName = Input.Matricule;
                user.IDGroupe = Input.IDGroupe;

                await _userStore.SetUserNameAsync(user, Input.Matricule, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, "Etudiant");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["NotificationScript"] = $"toastr.success('Inscription réussie!');";

                    return RedirectToPage("/Students/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ResetGroupes();

            return Page();
        }

        private void ResetGroupes()
        {
            var groupes = _repository.GetAllGroupesAsync().Result;
            ViewData["Groupes"] = new SelectList(groupes, "IDGroupe", "Nom");
        }

        private PuroUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<PuroUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(PuroUser)}'. " +
                                                    $"Ensure that '{nameof(PuroUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<PuroUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<PuroUser>)_userStore;
        }
    }
}
