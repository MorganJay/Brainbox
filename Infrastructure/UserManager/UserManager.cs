using Contracts.Managers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UserManager
{
    public class CustomUserManager<T> : IUserManager<T> where T : User
    {
        private readonly UserManager<T> _userManager;

        public CustomUserManager(UserManager<T> userManager)
        {
            _userManager = userManager;
        }

        public async Task<T> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<T> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> CreateAsync(T user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateAsync(T user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}