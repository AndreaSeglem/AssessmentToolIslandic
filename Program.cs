using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Data.Validation;
using LetterKnowledgeAssessment.Handlers;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
