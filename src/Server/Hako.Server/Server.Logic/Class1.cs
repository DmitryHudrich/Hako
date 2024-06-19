namespace Server.Logic;

public record class UserRegistrationRequest(String Name, String Login, String Password, String Email = "");

public record ActionResult<T>(Boolean Success, String Message, T Result);
public record ActionResult(Boolean Success, String Message);

public enum PasswordProblems {
    NONE = 0,
    TOO_SHORT = 1,
    NO_UPPERCASE = 2,
    NO_LOWERCASE = 3,
    NO_NUMBER = 4,
    NO_SYMBOL = 5,
}

public enum PasswordStrength {
    WEAK = 0,
    MEDIUM = 1,
    STRONG = 2,
}

public class RegisterUserInfo {
    public Int64 UserId { get; set; }
    public required Boolean IsEmailValid { get; init; }
    public required Boolean IsLoginValid { get; init; }
    public required Boolean IsNameValid { get; init; }
    public required Boolean IsPasswordValid { get; init; }
    public required PasswordStrength PasswordStrength { get; init; }
    public required IEnumerable<PasswordProblems> PasswordProblems { get; init; }
    public Boolean IsRegistraionSuccess => IsEmailValid && IsLoginValid && IsNameValid && IsPasswordValid;
}
