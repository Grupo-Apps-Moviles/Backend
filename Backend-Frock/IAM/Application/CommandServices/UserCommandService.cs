using Backend_Frock.IAM.Application.OutboundServices;
using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.IAM.Domain.Model.Commands;
using Backend_Frock.IAM.Domain.Repositories;
using Backend_Frock.IAM.Domain.Services;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.IAM.Application.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid email or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task Handle(SignUpCommand command)
    {
        /*    if (await userRepository.ExistsByName(command.Name))
                throw new Exception($"Name '{command.Nname}' is already taken");
        */
        if (await userRepository.ExistsByEmail(command.Email))
            throw new Exception($"Email '{command.Email}' is already registered");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Email, command.Username, hashedPassword, command.Role);

        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
}