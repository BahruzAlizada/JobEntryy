using JobEntryy.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace JobEntryy.Application.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<string> FindUserUsernameAsync(this UserManager<AppUser> userManager, string username)
        {
            AppUser user = await userManager.FindByNameAsync(username);
            return user.UserName;
        }
    }
}
