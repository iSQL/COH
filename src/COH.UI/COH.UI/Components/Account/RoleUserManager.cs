using COH.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace COH.UI.Components.Account;

public class RoleUserManager
{
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;

  public RoleUserManager(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
  {
    _roleManager = roleManager;
    _userManager = userManager;
  }

  public async Task CreateRoleAsync(string roleName)
  {
    if (!await _roleManager.RoleExistsAsync(roleName))
    {
      await _roleManager.CreateAsync(new IdentityRole(roleName));
    }
  }

  public async Task AssignRoleToUserAsync(string userId, string roleName)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user != null)
    {
      if (!await _roleManager.RoleExistsAsync(roleName))
      {
        throw new Exception($"Role {roleName} does not exist");
      }

      var result = await _userManager.AddToRoleAsync(user, roleName);
      if (!result.Succeeded)
      {
        throw new Exception("Failed to add user to role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
      }
    }
    else
    {
      throw new Exception("User not found");
    }
  }
}
