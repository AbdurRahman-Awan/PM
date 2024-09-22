using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PM.API.Data;
using PM.DATA.Models;
using PM.WEB.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure the HTTP client with a base address and add the AuthTokenHandler
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]); // Set the base address for API requests
})
.AddHttpMessageHandler<AuthTokenHandler>(); // Add the AuthTokenHandler for token management



// Configure session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make session cookie HTTP only
});

// Configure authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to Login if not authenticated
    });

// Register the DbContext for Entity Framework
builder.Services.AddDbContext<ProjectManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Configure SQL Server

// Register Identity services for user management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ProjectManagementDbContext>()
    .AddDefaultTokenProviders();

// Register IHttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();
// Register AuthTokenHandler as a transient service
builder.Services.AddTransient<AuthTokenHandler>();
var app = builder.Build();

// Use session, authentication, and authorization middleware
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configure exception handling for non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Redirect to error page
}

// Serve static files and enable routing
app.UseStaticFiles();
app.UseRouting();

// Define the default route for the application
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}"); // Set login as the default action
});

app.Run(); // Start the application
