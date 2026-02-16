using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services for controllers with views. Necessary for MVC functionality in the application.
builder.Services.AddControllersWithViews();

// Set False for now
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

// ---------------------------------------------------
// ROLE + ADMIN SEEDING
// This section is responsible for initializing default roles and creating an initial admin user
// if they do not already exist in the database. This is a common practice for setting up
// initial security configurations in ASP.NET Core Identity applications.
// ---------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    // Retrieve RoleManager and UserManager services from the service provider.
    // These services are essential for interacting with the Identity system for roles and users.
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Define the application's roles. These roles will be created if they don't already exist.
    string[] roles = { "Admin", "User" };

    // Iterate through each defined role.
    foreach (var role in roles)
    {
        // Check if the role already exists. If not, create it.
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Create default admin user
    // This section attempts to find an admin user by a predefined email.
    // If no admin user is found, a new one is created and assigned the "Admin" role.
    var adminEmail = "main@main.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    // If the admin user does not exist, create a new IdentityUser instance.
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // Mark email as confirmed to allow immediate login.
        };

        // Create the admin user with a specified password.
        await userManager.CreateAsync(admin, "P@ssw0rd");
        // Assign the newly created user to the "Admin" role.
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();