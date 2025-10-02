using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages;

public class UserProfile : PageModel
{
    private readonly UserManager<PuroUser> _userManager;
    private readonly IGroupeRepository _repository;

    public UserProfile(UserManager<PuroUser> userManager, IGroupeRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public PuroUser CurrentUser { get; set; }
    public Group UserGroup { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userName = User.Identity.Name;

        if (userName == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        CurrentUser = await _userManager.FindByNameAsync(userName);

        UserGroup = await _repository.GetGroupeNameAsync(CurrentUser.IDGroupe);

        if (CurrentUser == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        return Page();
    }
}
