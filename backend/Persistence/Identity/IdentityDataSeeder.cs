using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Identity;

public class IdentityDataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityDataSeeder(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedAdminUserAsync()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            //Creating default user
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@localhost",
            };
    
            await _userManager.CreateAsync(admin, "change_password");
        }
    }
}