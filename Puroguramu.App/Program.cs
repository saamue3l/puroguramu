using Microsoft.EntityFrameworkCore;
using Puroguramu.App.Middlewares;
using Puroguramu.Domains;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures;
using Puroguramu.Infrastructures.DbContexts;
using Puroguramu.Infrastructures.Dummies;
using Puroguramu.Infrastructures.Roslyn;
using Microsoft.AspNetCore.Identity;
using Puroguramu.App;
using Puroguramu.Infrastructures.Initializers;
using Puroguramu.Infrastructures.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddScoped<IAssessExercise, RoslynAssessor>();
builder.Services.AddScoped<IStatsIndexRepository, StatsIndexRepository>();
builder.Services.AddScoped<IGroupeRepository, GroupeRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IUpdateLessonRepository, UpdateLessonRepository>();
builder.Services.AddScoped<IProgresRepository, ProgresRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IUpdateExerciseRepository, UpdateExerciseRepository>();

builder.Services.AddDbContext<PuroguramuDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("PuroguramuDbContext")));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/");
        options.Conventions.AllowAnonymousToPage("/Index");
        options.Conventions.AllowAnonymousToPage("/Identity/Account/Login");
        options.Conventions.AllowAnonymousToPage("/Identity/Account/Register");
    });

builder.Services.AddDefaultIdentity<PuroUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PuroguramuDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/";
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/connexion";
    options.AccessDeniedPath = "/";
    options.LogoutPath = "/deconnexion";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect(options.LoginPath);
        return Task.CompletedTask;
    };
});


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions());
app.UseReverseProxyLinks();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PuroguramuDbContext>();
    context.Database.Migrate();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PuroUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityDataInitializer.SeedData(userManager, roleManager);
}

app.Run();
