using System.Diagnostics;
using Grpc.Core;
using Server.Logic;

namespace Server.ServerGate.Services;

public class HakoService(ILogger<HakoService> logger, UserLogicService userLogic) : HakoServerGate.HakoServerGateBase {
    public override async Task<UserRegistrationResponse> RegistrationUser(UserRegistrationRequest request, ServerCallContext context) {
        var registrationRequest = new Logic.UserRegistrationRequest(request.Name, request.Login, request.Password, request.Email);
        var result = await userLogic.RegisterUser(registrationRequest);

        var response = new UserRegistrationResponse {
            Message = result.Message,
            Success = result.Success,
            Details = new RegisterUserInfo {
                UserId = result.Result.UserId,
                IsEmailValid = result.Result.IsEmailValid,
                IsLoginValid = result.Result.IsLoginValid,
                IsNameValid = result.Result.IsNameValid,
                IsPasswordValid = result.Result.IsPasswordValid,
                PasswordStrength = result.Result.PasswordStrength switch {
                    Logic.PasswordStrength.STRONG => PasswordStrength.Strong,
                    Logic.PasswordStrength.MEDIUM => PasswordStrength.Medium,
                    Logic.PasswordStrength.WEAK => PasswordStrength.Weak,
                    _ => throw new ArgumentOutOfRangeException(result.Result.PasswordStrength.ToString())
                },
            }
        };
        
        foreach (var problem in result.Result.PasswordProblems) {
            var x = problem switch {
                Logic.PasswordProblems.NONE => PasswordProblems.None,
                Logic.PasswordProblems.NO_LOWERCASE => PasswordProblems.NoLowercase,
                Logic.PasswordProblems.NO_NUMBER => PasswordProblems.NoNumber,
                Logic.PasswordProblems.NO_SYMBOL => PasswordProblems.NoSymbol,
                Logic.PasswordProblems.NO_UPPERCASE => PasswordProblems.NoUppercase,
                Logic.PasswordProblems.TOO_SHORT => PasswordProblems.TooShort,
                _ => throw new ArgumentOutOfRangeException(),
            };
            response.Details.PasswordProblems.Add(x);
        }

        return response;
    }
}
