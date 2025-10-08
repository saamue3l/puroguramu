#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<PuroUser> _userManager;
        private readonly SignInManager<PuroUser> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IGroupeRepository _repository;

        public IndexModel(
            UserManager<PuroUser> userManager,
            SignInManager<PuroUser> signInManager,
            IWebHostEnvironment environment,
            IGroupeRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _repository = repository;
        }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Email")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "L'adresse mail n'est pas valide")]
            public string Email { get; set; }
            [Display(Name = "Nom")] public string Nom { get; set; }
            [Display(Name = "Prenom")] public string Prenom { get; set; }
            public string Matricule { get; set; }
            [Display(Name = "Groupe")] public int IDGroupe { get; set; }
            public IFormFile ProfileImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            ResetGroupes();

            Input = new InputModel
            {
                Email = user.Email,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Matricule = user.UserName,
                IDGroupe = user.IDGroupe
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Input.Matricule = user.UserName;
                ResetGroupes();

                return Page();
            }


            if (Input.ProfileImage != null)
            {
                var contentType = Input.ProfileImage.ContentType;
                if (contentType != "image/jpeg" && contentType != "image/png")
                {
                    Input.Matricule = user.UserName;
                    ResetGroupes();

                    ModelState.AddModelError("Input.ProfileImage", "Le format de l'image doit être JPEG ou PNG.");
                    return Page();
                }

                using (var memoryStream = new MemoryStream())
                {
                    await Input.ProfileImage.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    var isJPEG = bytes.Take(3).SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF });
                    var isPNG = bytes.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });

                    if (!isJPEG && !isPNG)
                    {
                        Input.Matricule = user.UserName;
                        ResetGroupes();
                        ModelState.AddModelError("Input.ProfileImage", "L'image n'est pas valide ou corrompue.");
                        return Page();
                    }
                }

                var sizeInMegabytes = Input.ProfileImage.Length / 1024.0 / 1024.0;
                if (sizeInMegabytes > 1)
                {
                    Input.Matricule = user.UserName;
                    ResetGroupes();
                    ModelState.AddModelError("Input.ProfileImage", "La taille de l'image doit être inférieure à 1 mégaoctet.");
                    return Page();
                }

                var fileName = Path.GetFileName(Input.ProfileImage.FileName);
                var imagesDirectory = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }
                var filePath = Path.Combine(imagesDirectory, fileName);
                var relativePath = Path.Combine("images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfileImage.CopyToAsync(fileStream);
                    user.Image = relativePath;
                }
            }

            user.Email = Input.Email;
            user.Nom = Input.Nom;
            user.Prenom = Input.Prenom;
            user.IDGroupe = Input.IDGroupe;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            TempData["NotificationType"] = "success";
            TempData["NotificationMessage"] = "Modification(s) validée(s)!";

            return RedirectToPage();
        }

        private void ResetGroupes()
        {
            var groupes = _repository.GetAllGroupesAsync().Result;
            ViewData["Groupes"] = new SelectList(groupes, "IDGroupe", "Nom");
        }
    }
}
