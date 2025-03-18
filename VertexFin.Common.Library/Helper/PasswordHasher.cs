using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexFin.Common.Library.Helper
{ 
    public static class PasswordHasher
    {
        private static readonly PasswordHasher<string> _hasher = new();

        public static string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }

}
