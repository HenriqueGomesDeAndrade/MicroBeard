using System;
using System.Security.Cryptography;
using System.Text;

namespace MicroBeard.Helpers
{
    public class PasswordManager
    {
        public static string EncryptPassword(string password)
        {
            var algorithm = SHA512.Create();

            var encodedValue = Encoding.UTF8.GetBytes(password);
            var encryptedPassword = algorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var character in encryptedPassword)
            {
                sb.Append(character.ToString("X2"));
            }

            return sb.ToString();
        }

        public static bool ValidatePassword(string newPassword, string oldPassword)
        {
            var algorithm = SHA512.Create();

            if (string.IsNullOrEmpty(oldPassword))
                throw new NullReferenceException("There is no old password.");

            var encryptedPassword = algorithm.ComputeHash(Encoding.UTF8.GetBytes(newPassword));

            var sb = new StringBuilder();
            foreach (var caractere in encryptedPassword)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString() == oldPassword;
        }
    }
}
