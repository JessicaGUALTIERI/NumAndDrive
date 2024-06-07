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

builder.Services.AddIdentity<User, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<NumAndDriveDbContext>().AddDefaultTokenProviders();



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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string mail = "admin@gmail.com";
    string password = "Administrator!1";

    if (await userManager.FindByEmailAsync(mail) == null)
    {
        User user = new User()
        {

            LastName = "admin",
            FirstName = "admin",
            IsFirstLogin = 1,
            Email = mail,
            PhoneNumber = "0707070707",
            StatusId = 1,
            DepartmentId = 1,
            UserTypeId = 1,
            ProfilePicturePath = "/img/profile-pic-blue.png",
            UserName = mail,
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}


//app.MapRazorPages();

app.Run();
