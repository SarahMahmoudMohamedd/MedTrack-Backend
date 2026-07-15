using MedTrack.ServicesAbstraction.Security; 
using System;
using BCryptCrypto = BCrypt.Net.BCrypt; 

namespace MedTrack.Services.Security
{
    public class PasswordService : IPasswordService 
    {
      
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;

            return BCryptCrypto.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
               
                return BCryptCrypto.Verify(password, passwordHash);
            }
            catch
            {
                return false; 
            }
        }
    }
}