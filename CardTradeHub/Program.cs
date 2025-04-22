using CardTradeHub;
using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using CardTradeHub.Data;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure file upload limits
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 10 * 1024 * 1024; // 10MB
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

// Ensure upload directory exists
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

// Register the DbContext with SQLite
builder.Services.AddDbContext<CardTradeHubContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<CardTradeHubContext>()
.AddDefaultTokenProviders();

// Add authentication services
builder.Services.AddAuthentication()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
    options.SaveTokens = true;
    options.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
    options.Scope.Add("profile");
    options.Scope.Add("email");
});

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "CardTradeHub.Auth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Initialize roles and admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    // Create roles if they don't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Find or create admin user and add to Admin role
    var adminUser = await userManager.FindByEmailAsync("admin@cardtradehub.com");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "admin@cardtradehub.com",
            Email = "admin@cardtradehub.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            RegisterDate = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(adminUser, "Admin@123456");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configure static files
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 24 hours
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=86400");
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Enable session
app.UseSession();

app.MapControllerRoute(
    name: "trade",
    pattern: "Trade/MyTrades",
    defaults: new { controller = "Transactions", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.Run();