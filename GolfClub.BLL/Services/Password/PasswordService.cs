using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.BLL.Services.Password
{
    public static class PasswordService
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(providedPassword) || string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentException("Password or hashed password cannot be null or empty.");
            }

            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
