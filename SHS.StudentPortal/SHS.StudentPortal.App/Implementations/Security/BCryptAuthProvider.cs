using SHS.StudentPortal.App.Abstractions.Security;

namespace SHS.StudentPortal.App.Implementations.Security;

public class BCryptAuthProvider : IBCryptAuthProvider
{
    const string _salt = "1CD399DF-3A7E-4574-965B-BA71D71C7348";
    const string passCannotBeNull = "Password cannot be empty or null";

    public string EncryptPassword(string rawPassword, int workFactor = 10)
    {
        if(string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException(passCannotBeNull);

        // Append Salt to prefix & suffix
        var saltedPassword = $"{_salt}{rawPassword}{_salt}";

        return BCrypt.Net.BCrypt.HashPassword(saltedPassword, workFactor <= 30 && workFactor > 10 ? workFactor : 10);
    }

    public bool VerifyPassword(string rawPassword, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException(passCannotBeNull);

        // Append Salt to prefix & suffix
        var saltedPassword = $"{_salt}{rawPassword}{_salt}";

        return BCrypt.Net.BCrypt.Verify(saltedPassword, hashedPassword);
    }
}
