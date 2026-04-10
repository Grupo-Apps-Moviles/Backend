using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    /**
     * <summary>
     *     Find a user by id
     * </summary>
     * <param name="username">The username to search</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByUsernameAsync(string username);

    Task<User?> FindByEmailAsync(string email);

    Task<bool> ExistsByEmail(string email);
}