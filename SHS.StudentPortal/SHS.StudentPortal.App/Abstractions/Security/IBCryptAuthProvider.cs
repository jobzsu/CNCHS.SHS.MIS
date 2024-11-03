namespace SHS.StudentPortal.App.Abstractions.Security;

public interface IBCryptAuthProvider
{
    string EncryptPassword(string rawPassword, int workFactor = 10);

    bool VerifyPassword(string rawPassword, string hashedPassword);
}
