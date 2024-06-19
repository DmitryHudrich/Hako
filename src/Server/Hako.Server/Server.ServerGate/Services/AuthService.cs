using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Domain.Entities;
using Server.Logic;
using Server.Logic.Internal;
using Server.Persistence;

namespace Server.ServerGate.Services;

public static class HakoServerGateExtensions {
    public static Boolean RegistrationValidationCheck(this RegisterUserInfo userInfo) {
        return userInfo.IsEmailValid && userInfo.IsLoginValid && userInfo.IsNameValid && userInfo.IsPasswordValid;
    }
}

public class AuthService(ILogger<AuthService> logger, HakoDbContext dbContext) : HakoAuthService.HakoAuthServiceBase {
    public override async Task<UserRegistrationResponse> RegistrationUser(UserRegistrationRequest request, ServerCallContext context) {
        var result = Validate(request);

        if (!result.RegistrationValidationCheck()) {
            return new UserRegistrationResponse {
                Message = "Registration failed.",
                Success = false,
                Details = result,
            };
        }

        var user = new User {
            Name = request.Name,
            Email = request.Email,
            Login = request.Login,
            Password = PasswordHasher.Hash(request.Password),
            Registration = DateTime.UtcNow,
        };

        if (await dbContext.Users.AnyAsync(u => u.Login == user.Login)) {
            return new UserRegistrationResponse {
                Message = "Login already exists.",
                Success = false,
                Details = result,
            };
        }

        if (await dbContext.Users.AnyAsync(u => u.Email == user.Email)) {
            return new UserRegistrationResponse {
                Message = "Email already exists.",
                Success = false,
                Details = result,
            };
        }

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        result.UserId = user.Id;
        logger.LogDebug("User {Name} registered.", user.Name);
        return new UserRegistrationResponse {
            Message = "Registration successful.",
            Success = true,
            Details = result,
        };
    }

    public override async Task<UserLoginResponse> LoginUser(UserLoginRequest request, ServerCallContext context) {
        var user = await dbContext.Users.Select(u => u).Where(u => u.Login == request.Login).FirstOrDefaultAsync();
        if (user == null) {
            return new UserLoginResponse {
                Message = "User not found.",
                Success = false,
                Details = new UserLoginInfo {
                    Bearer = "",
                    Refresh = "",
                    IsLoginExist = false,
                    IsPasswordCorrect = false,
                }
            };
        }

        if (!PasswordHasher.ValidatePassword(request.Password, user.Password)) {
            return new UserLoginResponse {
                Message = "Wrong password.",
                Success = false,
                Details = new UserLoginInfo {
                    Bearer = "",
                    Refresh = "",
                    IsLoginExist = true,
                    IsPasswordCorrect = false,
                }
            };
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

        var jwt = new JwtSecurityToken(
                issuer: JwtData.Issuer,
                audience: JwtData.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_DUPER_SECRET_KEY_BEBRA_1488_SPASIBO_DANE_ZA_FRONT!!!123")), SecurityAlgorithms.HmacSha256));

        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = Guid.NewGuid().ToString();
        user.RefreshToken = refreshToken;
        await dbContext.SaveChangesAsync();
        return new UserLoginResponse {
            Message = "Login successful.",
            Success = true,
            Details = new UserLoginInfo {
                Bearer = jwtString,
                Refresh = refreshToken,
                IsLoginExist = true,
                IsPasswordCorrect = true,
            }
        };
    }

    public override async Task<RefreshTokenResponse> RefreshToken(RefreshRequest request, ServerCallContext context) {
        var user = await dbContext.Users.Select(u => u).Where(u => u.RefreshToken == request.Refresh).FirstOrDefaultAsync();
        if (user == null) {
            return new RefreshTokenResponse {
                Message = "User not found.",
                Success = false,
                Details = new RefreshTokenInfo {
                    Bearer = "",
                    Refresh = "",
                }
            };
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

        var jwt = new JwtSecurityToken(
                issuer: JwtData.Issuer,
                audience: JwtData.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_DUPER_SECRET_KEY_BEBRA_1488_SPASIBO_DANE_ZA_FRONT!!!123")), SecurityAlgorithms.HmacSha256));

        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
        user.RefreshToken = Guid.NewGuid().ToString();
        await dbContext.SaveChangesAsync();
        return new RefreshTokenResponse {
            Message = "Refresh successful.",
            Success = true,
            Details = new RefreshTokenInfo {
                Bearer = jwtString,
                Refresh = user.RefreshToken,
            }
        };
    }
    private static RegisterUserInfo Validate(in UserRegistrationRequest request) {
        var userInfo = new RegisterUserInfo {
            UserId = -1,
            IsEmailValid = UserInfoValidator.ValidateEmail(request.Email),
            IsLoginValid = true, // TODO: Validate login
            IsNameValid = true, //  maybe validate name
            IsPasswordValid = UserInfoValidator.ValidatePassword(request.Password, out var problems, out var strength),
            PasswordStrength = strength,
        };
        userInfo.PasswordProblems.AddRange(problems);
        return userInfo;
    }
}
