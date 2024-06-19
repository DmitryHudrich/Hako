using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;
using Server.Logic.Internal;
using Server.Persistence;

namespace Server.Logic;

public class UserLogicService(ILogger<UserLogicService> logger, HakoDbContext dbContext) {
    public async Task<ActionResult<RegisterUserInfo>> RegisterUser(UserRegistrationRequest request) {
        var result = Validate(request);
        System.Console.WriteLine(request);
        if (!result.IsRegistraionSuccess) {
            return new ActionResult<RegisterUserInfo>(false, "Registration failed.", result);
        }

        var user = new User {
            Name = request.Name,
            Email = request.Email,
            Login = request.Login,
            Password = request.Password, // PasswordHasher.Hash(request.Password),
            Registration = DateTime.UtcNow,
        };

        if (await dbContext.Users.AnyAsync(u => u.Login == user.Login)) {
            return new ActionResult<RegisterUserInfo>(false, "Login already exists.", result);
        }

        if (await dbContext.Users.AnyAsync(u => u.Email == user.Email)) {
            return new ActionResult<RegisterUserInfo>(false, "Email already exists.", result);
        }

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        result.UserId = user.Id;
        logger.LogDebug("User {Name} registered.", user.Name);
        return new ActionResult<RegisterUserInfo>(true, "Registration succeeded.", result);
    }

    private static RegisterUserInfo Validate(in UserRegistrationRequest request) {
        return new RegisterUserInfo {
            UserId = -1,
            IsEmailValid = UserInfoValidator.ValidateEmail(request.Email),
            IsLoginValid = true, // TODO: Validate login
            IsNameValid = true, //  maybe validate name
            IsPasswordValid = UserInfoValidator.ValidatePassword(request.Password, out var problems, out var strength),
            PasswordStrength = strength,
            PasswordProblems = problems,
        };

    }
}
