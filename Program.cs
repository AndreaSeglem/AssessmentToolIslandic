using System.Globalization;
using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Data.Validation;
using LetterKnowledgeAssessment.Handlers;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSession();

builder.Services.AddDefaultIdentity<Teacher>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;         
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<IdentityErrorMessages>();

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported cultures (Norwegian and Icelandic)
var supportedCultures = new[] { "no", "is" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("no"),
    SupportedCultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList(),
    SupportedUICultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList(),
    FallBackToParentCultures = true,
    FallBackToParentUICultures = true
};

// Prioritize QueryStringRequestCultureProvider for culture switching via URL
localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create("Strings", "LetterKnowledgeAssessment");
    });

    
builder.Services.AddRazorPages();
builder.Services.AddTransient<IClassListRepository, ClassListRepository>();
builder.Services.AddTransient<IClassListHandler, ClassListHandler>();
builder.Services.AddTransient<IPupilRepository, PupilRepository>();
builder.Services.AddTransient<IPupilHandler, PupilHandler>();
builder.Services.AddTransient<ILetterTestRepository, LetterTestRepository>();
builder.Services.AddTransient<ILetterTestHandler, LetterTestHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
// Enable localization
app.UseRequestLocalization(localizationOptions);
app.UseRouting();

// Enable localization using the configured options
/* var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("no"),
    SupportedCultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList(),
    SupportedUICultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList(),
    RequestCultureProviders = new List<IRequestCultureProvider> 
    { 
        new CookieRequestCultureProvider(), // Cookie provider to persist culture across sessions
        new QueryStringRequestCultureProvider() // Query string provider to allow culture overrides in URLs
    }
};
localizationOptions.FallBackToParentCultures = true;
localizationOptions.FallBackToParentUICultures = true; */

// Output the current culture for debugging purposes
//Console.WriteLine("Current Culture: " + CultureInfo.CurrentCulture);



app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
