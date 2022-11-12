using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Managers
{
    public interface IUserManager<T> where T : User
    {
        Task<IdentityResult> CreateAsync(T user);

        Task<IdentityResult> CreateAsync(T user, string password);

        Task<T> FindByEmailAsync(string email);

        Task<T> FindByIdAsync(Guid id);

        Task<T> FindByNameAsync(string userName);
    }
}