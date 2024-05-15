using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReversiMVCApplication.Data;
using ReversiMVCApplication.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var reversiDbConnection = builder.Configuration.GetConnectionString("ReversiDbConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//! Reversi DB context
builder.Services.AddDbContext<ReversiDbContext>(options => options.UseSqlServer(reversiDbConnection));


//! added the HTTPClinet service connection to make the communication between the MVC and RestAPI

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44349/"); // uri of the restAPI
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// using the session to prevent users from bypassing the app flow using the linkk
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout of 30 min
//     options.Cookie.HttpOnly = true; // Setting the cookie as HttpOnly for security purposes
//     options.Cookie.IsEssential = true; // making the cookoies as essential
//
// });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    string[] roleNames = { "Admin", "User", "Mediator" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Create the roles and seed them to the database
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


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

//use httpS redirection to handle the sensitive data between the server-to-server commuinaction
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseHttpsRedirection();
app.UseStaticFiles();

//! CSP must be added here
//! because the CSP should be applied to all static files served by your application,
//! and it should be set before the routing decisions are made.

// Add CSP middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src *;");
    await next();
});


app.UseRouting();
// app.UseSession();

app.UseAuthentication(); // who are u?
app.UseAuthorization(); // what are u allowed?

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();