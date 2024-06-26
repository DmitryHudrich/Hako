namespace Server.Logic.Internal;

internal static class JwtData {
    public const String Issuer = "Berberovka";
    public const String Audience = "Bebra";
}

internal static class PasswordHasher {
    public static String Hash(String password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public static Boolean ValidatePassword(String input, String hash) {
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }
}

internal static class UserInfoValidator {
    public static Boolean ValidateEmail(String email) {
        return email.Contains('@');
    }

    public static Boolean ValidatePassword(String password, out PasswordProblems[] problems, out PasswordStrength passwordStrength) {
        var problemsList = new List<PasswordProblems>();
        if (password.Length < 8) {
            problemsList.Add(PasswordProblems.TooShort);
        }
        if (!password.Any(Char.IsUpper)) {
            problemsList.Add(PasswordProblems.NoUppercase);
        }
        if (!password.Any(Char.IsLower)) {
            problemsList.Add(PasswordProblems.NoLowercase);
        }
        if (!password.Any(Char.IsDigit)) {
            problemsList.Add(PasswordProblems.NoNumber);
        }
        if (!password.Any(Char.IsSymbol)) {
            problemsList.Add(PasswordProblems.NoSymbol);
        }
        problems = [.. problemsList];
        passwordStrength = problemsList.Count > 2
            ? PasswordStrength.Weak
            : PasswordStrength.Strong;
        return problemsList.Count < 2;
    }

    public static Boolean ValidateName(String name) {
        return name.Length >= 3;
    }
}