using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages;

public class IndexModel : PageModel
{
    private readonly IStatsIndexRepository _repository;
    private readonly UserManager<PuroUser> _userManager;
    private readonly SignInManager<PuroUser> _signInManager;

    public int LessonCount => _repository.LessonCount;

    public int ExerciseCount => _repository.ExerciseCount;

    public int MemberCount { get; set; } = 0;

    public IndexModel(IStatsIndexRepository repository, UserManager<PuroUser> userManager, SignInManager<PuroUser> signInManager)
    {
        _repository = repository;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (_signInManager.IsSignedIn(User))
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Etudiant"))
            {
                return RedirectToPage("/Students/Index");
            }
            else if (roles.Contains("Enseignant"))
            {
                return RedirectToPage("/Teachers/Index");
            }
        }

        var usersInRole = _userManager.GetUsersInRoleAsync("Etudiant");

        MemberCount = usersInRole.Result.Count();

        return Page();
    }
}
