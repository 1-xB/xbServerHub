using API.BackgroundServices;
using API.Services;
using Application.Handlers;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Glances;
using Persistence.Identity;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "xbServerHub_cookie";
    options.Cookie.HttpOnly = true; // JavaScript nie ma dostępu do ciasteczka
    options.Cookie.SameSite = SameSiteMode.Strict; // Ochrona przed CSRF
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Czas życia sesji
    options.SlidingExpiration = true; 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173"   // Frontend HTTP 
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHttpClient<IGlancesClient, GlancesClient>(client =>
    {
        var baseUrl = builder.Configuration["Glances:BaseUrl"];
        client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(5);
    });

builder.Services.AddScoped<IMetricsBroadcaster, SignalMetricsBroadcaster>();

builder.Services.AddScoped(typeof(CollectSystemMetricsHandler));
builder.Services.AddHostedService<GlancesMetricsBackgroundService>();


builder.Services.AddScoped<IdentityDataSeeder>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();
    await seeder.SeedAdminUserAsync();
}
    
app.Run();
