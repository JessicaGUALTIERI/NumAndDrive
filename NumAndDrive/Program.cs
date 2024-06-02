using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NumAndDrive.Database;
using NumAndDrive.Models;
using NumAndDrive.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true); // https:\//stackoverflow.com/questions/72060349/form-field-is-required-even-if-not-defined-so

builder.Services.AddScoped<NumAndDriveDbContext>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IJourneyRepository, JourneyRepository>();

var connectionString = builder.Configuration.GetConnectionString("Project_NumAndDriveDbContextConnection") ?? throw new InvalidOperationException("Connection string 'Project_NumAndDriveDbContextConnection' not found.");

builder.Services.AddDbContext<NumAndDriveDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NumAndDriveDbContext>().AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.MapControllerRoute(
    name: "PageNotFound",
    pattern: "{controller=Error}/{action=PageNotFound}"
);

app.MapControllerRoute(
    name: "AccessDenied",
    pattern: "{controller=Error}/{action=AccessDenied}");




//app.MapRazorPages();

app.Run();
